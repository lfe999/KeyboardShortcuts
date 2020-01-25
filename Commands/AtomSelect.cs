using LFE.KeyboardShortcuts.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LFE.KeyboardShortcuts.Commands
{
    public class AtomSelect : Command
    {
        protected Func<Atom, bool> _predicate;
        public AtomSelect(Func<Atom, bool> predicate) {
            _predicate = predicate;
        }

        public virtual Atom TargetAtom(CommandExecuteEventArgs args)
        {
            return SelectableAtoms().Where(_predicate).First() ?? SuperController.singleton.GetSelectedAtom();
        }

        public override bool Execute(CommandExecuteEventArgs args)
        {
            SuperController.singleton.SelectController(TargetAtom(args)?.mainController);
            return true;
        }

        protected IEnumerable<Atom> SelectableAtoms()
        {
            return SuperController.singleton.GetSelectableAtoms().OrderBy((a) => a.uid);
        }
    }
}
