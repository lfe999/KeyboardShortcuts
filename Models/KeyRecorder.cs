using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LFE.KeyboardShortcuts.Models
{
    public class KeyRecorder
    {
        public string ActionName { get; }
        public HashSet<KeyCode> Recorded { get; private set; } = new HashSet<KeyCode>();
        public bool InProgress { get; private set; } = false;

        private Action<KeyChord> _onFinish;

        public KeyRecorder(string actionName, Action<KeyChord> onFinish = null)
        {
            ActionName = actionName;
            _onFinish = onFinish;
        }

        public HashSet<KeyCode> Record()
        {
            var noticed = Enum.GetValues(typeof(KeyCode))
                .Cast<KeyCode>()
                .Where(Input.GetKey)
                .Where((k) => !KeyChord.IGNORED_KEYS.Contains(k))
                .ToList();
            if (InProgress && noticed.Count == 0)
            {
                // looks like we just finished recording... don't
                // set the internal state anymore
                return Recorded;
            }

            if (noticed.Count > 0)
            {
                InProgress = true;
                foreach (var k in noticed)
                {
                    Recorded.Add(k);
                }
            }

            return Recorded;
        }

        public KeyChord ToKeyChord()
        {
            var chord = string.Join("-", Recorded.Select((k) => k.ToString()).ToArray());
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
