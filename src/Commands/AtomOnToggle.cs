using System;

namespace LFE.KeyboardShortcuts.Commands
{
    public class AtomOnToggle : AtomCommandBase
    {
        public AtomOnToggle() : this((Func<Atom, bool>)null) { }
        public AtomOnToggle(Atom atom) : this((a) => a.uid.Equals(atom.uid)) { }
        public AtomOnToggle(Func<Atom, bool> predicate) : base(predicate) { }

        public override bool Execute(CommandExecuteEventArgs args)
        {
            var target = TargetAtom(args);
            if (target != null) { target.SetOn(!target.on); }
            return true;
        }
    }
}
