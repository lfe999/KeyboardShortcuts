using LFE.KeyboardShortcuts.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace LFE.KeyboardShortcuts.Models
{
    public class KeyRecorder
    {
        public bool InProgress { get; private set; } = false;

        private Action<KeyChord> _onFinish;
        private HashSet<KeyCode> _recordedKeys = new HashSet<KeyCode>();
        private string _recordedAxis;

        public KeyRecorder(Action<KeyChord> onFinish = null)
        {
            _onFinish = onFinish;
        }

        public void Record()
        {
            var noticedAxisThreshold = 0.5f; // don't consider an axis noticed for record for "blips"

            var noticedKeys = InputWrapper.KeyCodes
                .Where(InputWrapper.GetKey)
                .Where((k) => !KeyChord.IGNORED_KEYS.Contains(k))
                .Where((k) => !Regex.IsMatch(k.ToString(), "^Joystick\\d")) // joy buttons register twice -- ignore most specific one
                .ToList();
            var noticedAxis = InputWrapper.AxisNames
                .Where((n) => Mathf.Abs(InputWrapper.GetAxis(n)) >= noticedAxisThreshold)
                .FirstOrDefault();

            if (InProgress && noticedKeys.Count == 0 && noticedAxis == null)
            {
                // looks like we just finished recording... don't
                // set the internal state anymore
                return;
            }

            if (noticedKeys.Count > 0)
            {
                InProgress = true;
                foreach (var k in noticedKeys)
                {
                    _recordedKeys.Add(k);
                }
            }

            if(noticedAxis != null)
            {
                InProgress = true;
                _recordedAxis = noticedAxis;
            }

            return;
        }

        public KeyChord ToKeyChord()
        {
            var chordParts = _recordedKeys.Select((k) => k.ToString()).ToList();
            if(_recordedAxis != null)
            {
                chordParts.Add(_recordedAxis);
            }
            var chord = string.Join("-", chordParts.ToArray());
            return new KeyChord(chord);
        }

        public void OnFinish()
        {
            if (_onFinish != null)
            {
                _onFinish(ToKeyChord()); ;
            }
        }
    }

}
