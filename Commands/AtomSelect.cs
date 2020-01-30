using LFE.KeyboardShortcuts.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LFE.KeyboardShortcuts.Commands
{
    public class AtomSelect : AtomCommandBase
    {
        public AtomSelect(Atom atom) : this((a) => a.uid.Equals(atom.uid)) { }
        public AtomSelect(Func<Atom, bool> predicate) : base(predicate) { }

        public override bool Execute(CommandExecuteEventArgs args)
        {
            SuperController.singleton.SelectController(TargetAtom(args)?.mainController);
            return true;
        }
    }
}
