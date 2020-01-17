using System;

namespace LFE.KeyboardShortcuts.Models
{
    public class KeyBinding
    {
        public KeyChord KeyChord { get; private set; }
        public Action Action { get; private set; }
        public string Name { get; private set; }
        public bool Enabled { get; set; }

        private KeyBinding(string name, KeyChord chord, Action action)
        {
            KeyChord = chord;
            Action = action;
            Name = name;
            Enabled = true;
        }

        public static KeyBinding Build(Plugin plugin, string name, KeyChord chord)
        {
            return new KeyBinding(name, chord, () => plugin.CallAction(name));
        }

        public override string ToString()
        {
            return Name;
        }

        public override bool Equals(object obj)
        {
            KeyBinding b = obj as KeyBinding;
            if (b == null) { return false; }
            return b.ToString().Equals(this.ToString());
        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }
    }
}
