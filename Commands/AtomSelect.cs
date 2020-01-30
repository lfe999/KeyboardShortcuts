using LFE.KeyboardShortcuts.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LFE.KeyboardShortcuts.Commands
{
    public class AtomSelect : AtomCommandBase
    {
        private FreeControllerV3 _controller;
        public AtomSelect(FreeControllerV3 controller) : this((a) => a.uid.Equals(controller.containingAtom.uid)) {
            _controller = controller;
        }
        public AtomSelect(Atom atom) : this((FreeControllerV3)atom.mainController) { }
        public AtomSelect(Func<Atom, bool> predicate) : base(predicate) { }

        public override bool Execute(CommandExecuteEventArgs args)
        {
            if(_controller == null)
            {
                _controller = TargetAtom(args)?.mainController;
            }
            SuperController.singleton.SelectController(_controller);
            return true;
        }
    }
}
