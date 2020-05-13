using System;
using LFE.KeyboardShortcuts.Commands;

namespace LFE.KeyboardShortcuts.Models
{
    public class KeyBinding
    {
        public KeyChord KeyChord { get; private set; }
        public Func<CommandExecuteEventArgs, bool> Action { get; private set; }
        public string Name { get; private set; }
        public bool Enabled { get; set; }

        private KeyBinding(string name, KeyChord chord, Func<CommandExecuteEventArgs, bool> action)
        {
            KeyChord = chord;
            Action = action;
            Name = name;
            Enabled = true;
        }

        public static KeyBinding Build(Plugin plugin, string name, KeyChord chord, Command command)
        {
            return new KeyBinding(name, chord, (e) => command.Execute(e));
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
