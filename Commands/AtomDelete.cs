using System.Collections;

namespace LFE.KeyboardShortcuts.Commands
{
    public class AtomDelete : AtomCommandBase
    {
        public AtomDelete(Atom atom = null) : base(atom) { }
        public override bool Execute(CommandExecuteEventArgs args)
        {
            var selected = GetAtomTarget();
            if (selected != null) {
                SuperController.singleton.StartCoroutine(DeleteAtom(selected));
            }
            return true;
        }

        private IEnumerator DeleteAtom(Atom atom)
        {
            SuperController.singleton.RemoveAtom(atom);
            yield return null;
        }
    }
}
