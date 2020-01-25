using LFE.KeyboardShortcuts.Extensions;
using System;
using System.Linq;

namespace LFE.KeyboardShortcuts.Commands
{
    public class AtomSelectNext : AtomSelect
    {
        public AtomSelectNext() : base((x) => true) { }
        public AtomSelectNext(Func<Atom, bool> predicate) : base(predicate) { }

        public override Atom TargetAtom(CommandExecuteEventArgs args)
        {
            if (args.KeyBinding.KeyChord.HasAxis)
            {
                // make sure the keybinding and value change are going the
                // same "direction" (for the case where the axis is involved)
                if (!MathUtilities.SameSign(args.Data, 1)) { return null; }
            }

            var current = SuperController.singleton.GetSelectedAtom();
            return SelectableAtoms()
                .LoopOnceStartingWhen((a) => a.uid.Equals(current?.uid))
                .First(_predicate) ?? current;
        }
    }
}
