using LFE.KeyboardShortcuts.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LFE.KeyboardShortcuts.Commands
{
    public class AtomSelect : AtomCommandBase
    {
        private string _atomUid;
        private string _controllerName;

        public AtomSelect(FreeControllerV3 controller) : this((a) => a.uid.Equals(controller?.containingAtom?.uid)) {
            if(controller != null)
            {
                _atomUid = controller.containingAtom.uid;
                _controllerName = controller.name;
            }
        }
        public AtomSelect(Atom atom) : this((FreeControllerV3)atom?.mainController) { }
        public AtomSelect(Func<Atom, bool> predicate) : base(predicate) { }

        public override bool Execute(CommandExecuteEventArgs args)
        {
            var controller = SuperController.singleton.GetFreeController(_atomUid, _controllerName) ?? TargetAtom(args)?.mainController;
            SuperController.singleton.SelectController(controller);
            return true;
        }
    }
}
