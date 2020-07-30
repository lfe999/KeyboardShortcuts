using LFE.KeyboardShortcuts.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LFE.KeyboardShortcuts.Commands
{
    internal abstract class ControllerCommandBase : Command {
        protected Func<FreeControllerV3, bool> _predicate;
        protected ControllerCommandBase(Func<FreeControllerV3, bool> predicate)
        {
            _predicate = predicate;
        }

        public virtual FreeControllerV3 TargetController(CommandExecuteEventArgs args)
        {
            if(_predicate == null)
            {
                return SuperController.singleton.GetSelectedController();
            }
            else
            {
                return SelectableControllers().Where(_predicate).FirstOrDefault();
            }
        }

        protected IEnumerable<FreeControllerV3> SelectableControllers()
        {
            return SuperController.singleton.GetSelectableControllers().OrderBy((c) => c.containingAtom.uid);
        }
    }
}
