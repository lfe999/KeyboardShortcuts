namespace LFE.KeyboardShortcuts.Commands
{
    public abstract class AtomCommandBase : Command {
        protected Atom Atom; // null means get selected
        protected AtomCommandBase(Atom atom = null)
        {
            Atom = atom;
        }

        public Atom GetAtomTarget()
        {
            if(Atom == null) { return SuperController.singleton.GetSelectedAtom(); }
            return Atom;
        }
    }
}
