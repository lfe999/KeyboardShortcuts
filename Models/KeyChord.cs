using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LFE.KeyboardShortcuts.Models
{
    /// <summary>
    /// Represent multiple key combinations (chords).
    /// 
    /// Also allows for supporting "virtual" keycodes like "Control" which will
    /// internally match both "LeftControl" and "RightControl"
    /// </summary>
    public class KeyChord
    {
        public static readonly HashSet<KeyCode> IGNORED_KEYS = new HashSet<KeyCode>() { KeyCode.Mouse0 };
        private static readonly char[] DELIMITER = { '+', '-' };
        private static readonly Dictionary<string, List<KeyCode>> _virtualKeyNameToCodes = new Dictionary<string, List<KeyCode>>(StringComparer.OrdinalIgnoreCase)
        {
            { "Shift", new List<KeyCode> { KeyCode.LeftShift, KeyCode.RightShift } },
            { "Control", new List<KeyCode> { KeyCode.LeftControl, KeyCode.RightControl } },
            { "Alt", new List<KeyCode> { KeyCode.LeftAlt, KeyCode.RightAlt } }
        };

        private string _originalChord;
        private IEnumerable<HashSet<KeyCode>> _definitions { get; }

        /// <summary>
        /// Number of keys in this key chord
        /// </summary>
        public int Length => _definitions.FirstOrDefault().Count;

        public KeyChord(string chord)
        {
            if (chord == null) {
                throw new ArgumentNullException(nameof(chord));
            }
            var chordParts = chord.Split(DELIMITER).ToList();
            _originalChord = chord;
            _definitions = ConcreteChordsFromVirtual(chordParts);

            foreach (var d in _definitions)
            {
                var s = d.Select(x => x.ToString()).ToArray();
            }

        }

        /// <summary>
        /// Have all of the keys been held down for a while?
        /// </summary>
        /// <returns></returns>
        public bool IsBeingRepeated()
        {
            if (!IsBeingPressed())
            {
                return false;
            }
            return _definitions.Any((definition) => definition.All((c) => !Input.GetKeyDown(c)));
        }

        /// <summary>
        /// Are all of the keys being held down?
        /// </summary>
        /// <returns></returns>
        public bool IsBeingPressed()
        {
            return _definitions.Any((definition) => definition.All(Input.GetKey));
        }

        public override string ToString()
        {
            return _originalChord;
        }

        public override bool Equals(object obj)
        {
            KeyChord kc = obj as KeyChord;
            if (kc == null) { return false; }
            return kc.ToString().Equals(this.ToString());
        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        /// <summary>
        /// Take chord parts containing virtual key names and return a list
        /// of all concrete chords that would match
        /// 
        /// Example:
        ///   "Control-C" -> "LeftControl-C", "RightControl-C"
        /// </summary>
        /// <param name="chordParts"></param>
        /// <returns></returns>
        private static IEnumerable<HashSet<KeyCode>> ConcreteChordsFromVirtual(List<string> chordParts)
        {
            bool hasVirtualKeyCode = chordParts.Any((p) => _virtualKeyNameToCodes.ContainsKey(p));
            if (hasVirtualKeyCode)
            {
                foreach (var virtualCode in _virtualKeyNameToCodes)
                {
                    var modifierIndexInChord = chordParts.IndexOf(virtualCode.Key);
                    if (modifierIndexInChord >= 0)
                    {
                        foreach (var realCode in virtualCode.Value)
                        {
                            var newChord = new List<string>(chordParts);
                            newChord[modifierIndexInChord] = realCode.ToString();
                            var concretes = ConcreteChordsFromVirtual(newChord);
                            foreach (var concrete in concretes)
                            {
                                yield return concrete;
                            }
                        }
                    }
                }
            }
            else
            {
                var chordKeys = chordParts.Select((x) => (KeyCode)Enum.Parse(typeof(KeyCode), x));
                var hash = new HashSet<KeyCode>(chordKeys);
                if (hash.Intersect(IGNORED_KEYS).ToList().Count > 0)
                {
                    throw new Exception($"These keys are not allowed {IGNORED_KEYS}");
                }
                yield return hash;
            }
        }
    }
}
