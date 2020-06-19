using System;

namespace LFE.KeyboardShortcuts.Commands
{
    public class UIButtonTriggerCommand : AtomCommandBase
    {
        public UIButtonTriggerCommand() : this((Func<Atom, bool>)null) { }
        public UIButtonTriggerCommand(Atom atom) : this((a) => a.uid.Equals(atom.uid)) { }
        public UIButtonTriggerCommand(Func<Atom, bool> predicate) : base(predicate) { }

        public override bool Execute(CommandExecuteEventArgs args)
        {
            var selected = TargetAtom(args);
            if (selected != null) {
                var trigger = selected.GetComponentInChildren<UIButtonTrigger>();
                trigger?.button?.onClick?.Invoke();
            }
            return true;
        }
    }
}
