using LFE.KeyboardShortcuts.Extensions;
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
        private IEnumerable<HashSet<KeyCode>> _keyPressDefinitions { get; }
        private string _axisDefinition;

        private IEnumerable<HashSet<string>> _namedDefinitions { get; }

        /// <summary>
        /// Number of keys in this key chord
        /// </summary>
        public int Length => _keyPressDefinitions.FirstOrDefault().Count + (_axisDefinition != null ? 1 : 0);

        public bool HasAxis => _axisDefinition != null; 

        public KeyChord(string chord)
        {
            if (chord == null) {
                throw new ArgumentNullException(nameof(chord));
            }
            var chordParts = chord.Split(DELIMITER).ToList();
            var axisNamesInChord = chordParts.Where((n) => InputWrapper.AxisNames.Contains(n));
            if (axisNamesInChord.Count() > 1)
            {
                throw new ArgumentException("only one axis allowed", nameof(chord));
            }

            _axisDefinition = axisNamesInChord.FirstOrDefault(); // it's ok if this is null
            _originalChord = chord;
            _keyPressDefinitions = ConcreteChordsFromVirtual(chordParts);

            // create an internal copy of the keycode listings but by string/name (and include the axis)
            var namedDefinitions = new List<HashSet<string>>();
            var axisEnumerable = _axisDefinition == null ? Enumerable.Empty<string>() : new string[] { _axisDefinition };
            if (_keyPressDefinitions.Count() > 0)
            {
                foreach (var definition in _keyPressDefinitions)
                {
                    namedDefinitions.Add(new HashSet<string>(definition.Select((code) => code.ToString()).Concat(axisEnumerable)));
                }
            }
            else
            {
                namedDefinitions.Add(new HashSet<string>(axisEnumerable));
            }
            _namedDefinitions = namedDefinitions;
        }

        public IEnumerable<HashSet<string>> GetChordSets()
        {
            return _namedDefinitions;
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
            if(HasAxis)
            {
                // always run axis commands as fast as possible
                return false;
            }
            return _keyPressDefinitions.Any((definition) => definition.All((c) => !InputWrapper.GetKeyDown(c)));
        }

        /// <summary>
        /// Are all of the keys being held down?
        /// </summary>
        /// <returns></returns>
        public bool IsBeingPressed()
        {
            var isButtonsPressed = _keyPressDefinitions.Any((definition) => definition.All(InputWrapper.GetKey));
            var isAxisPressed = (_axisDefinition == null) ? true : InputWrapper.GetAxis(_axisDefinition) != 0;
            return isButtonsPressed && isAxisPressed;
        }

        /// <summary>
        /// If pressing a key you will get a 1f. If this includes an axis, then you will
        /// get -1f to 1f (but sometimes higher ranges depending on the axis)
        /// </summary>
        /// <returns></returns>
        public float GetPressedValue()
        {
            if (!IsBeingPressed())
            {
                return 0;
            }

            if (_axisDefinition != null)
            {
                return InputWrapper.GetAxis(_axisDefinition);
            }

            return 1;
        }

        public bool IsProperSubsetOf(KeyChord chord)
        {
            return GetChordSets().Any((mine) => chord.GetChordSets().Any((theirs) => mine.IsProperSubsetOf(theirs)));
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
                // if there is an axis name in here, thats ok.. just parse out things that
                // we know are button presses... later on we will pull our axis names
                var chordKeys = chordParts
                    .Where((x) => !InputWrapper.AxisNames.Contains(x))
                    .Select((x) => (KeyCode)Enum.Parse(typeof(KeyCode), x));
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
