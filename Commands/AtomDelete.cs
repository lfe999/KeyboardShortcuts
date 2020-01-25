namespace LFE.KeyboardShortcuts.Commands
{
    public class AtomDelete : AtomCommandBase
    {
        public AtomDelete(Atom atom = null) : base(atom) { }
        public override bool Execute(CommandExecuteEventArgs args)
        {
            var selected = GetAtomTarget();
            if (selected != null) { SuperController.singleton.RemoveAtom(selected); }
            return true;
        }
    }
}
