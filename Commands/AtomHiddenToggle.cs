namespace LFE.KeyboardShortcuts.Commands
{
    public class AtomHiddenToggle : AtomCommandBase
    {
        public AtomHiddenToggle(Atom atom = null) : base(atom) { }

        public override bool Execute(CommandExecuteEventArgs args)
        {
            var target = GetAtomTarget();
            if (target != null) { target.hidden = !target.hidden; }
            return true;
        }
    }
}
