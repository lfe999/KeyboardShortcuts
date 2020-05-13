using System;
using System.Collections;

namespace LFE.KeyboardShortcuts.Commands
{
    public class AtomDelete : AtomCommandBase
    {
        public AtomDelete() : this(null) { }
        public AtomDelete(Func<Atom, bool> predicate) : base(predicate) { }
        public override bool Execute(CommandExecuteEventArgs args)
        {
            var selected = TargetAtom(args);
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
