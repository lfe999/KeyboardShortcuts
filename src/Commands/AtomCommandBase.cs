using LFE.KeyboardShortcuts.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LFE.KeyboardShortcuts.Commands
{
    public abstract class AtomCommandBase : Command {
        protected Func<Atom, bool> _predicate;
        protected AtomCommandBase(Func<Atom, bool> predicate)
        {
            _predicate = predicate;
        }

        public virtual Atom TargetAtom(CommandExecuteEventArgs args)
        {
            if(_predicate == null)
            {
                return SuperController.singleton.GetSelectedAtom();
            }
            else
            {
                return SelectableAtoms().Where(_predicate).FirstOrDefault();
            }
        }

        protected IEnumerable<Atom> SelectableAtoms()
        {
            return SuperController.singleton.GetSelectableAtoms().OrderBy((a) => a.uid);
        }
    }
}
