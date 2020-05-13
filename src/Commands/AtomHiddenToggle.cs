using System;

namespace LFE.KeyboardShortcuts.Commands
{
    public class AtomHiddenToggle : AtomCommandBase
    {
        public AtomHiddenToggle() : this((Func<Atom, bool>)null) { }
        public AtomHiddenToggle(Atom atom) : this((a) => a.uid.Equals(atom.uid)) { }
        public AtomHiddenToggle(Func<Atom, bool> predicate) : base(predicate) { }

        public override bool Execute(CommandExecuteEventArgs args)
        {
            var target = TargetAtom(args);
            if (target != null) { target.hidden = !target.hidden; }
            return true;
        }
    }
}
