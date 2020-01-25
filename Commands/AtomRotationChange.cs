using UnityEngine;
using UnityEngine.Animations;

namespace LFE.KeyboardShortcuts.Commands
{
    public class AtomRotationChange : AtomCommandBase
    {
        private Axis _axis;
        private float _rotationsPerSecond;
        public AtomRotationChange(Axis axis, float rotationsPerSecond, Atom atom = null) : base(atom)
        {
            _axis = axis;
            _rotationsPerSecond = rotationsPerSecond;
        }

        public override bool Execute(CommandExecuteEventArgs args)
        {
            if (args.KeyBinding.KeyChord.HasAxis)
            {
                // make sure the keybinding and value change are going the
                // same "direction" (for the case where the axis is involved)
                if (!MathUtilities.SameSign(args.Data, _rotationsPerSecond)) { return false; }
            }

            var selected = GetAtomTarget();
            if (selected != null)
            {
                var rotate = 360 * Time.deltaTime * _rotationsPerSecond * Mathf.Abs(args.Data);
                var target = selected.freeControllers[0].transform;

                if(_axis == Axis.X) { target.Rotate(rotate, 0, 0); }
                else if(_axis == Axis.Y) { target.Rotate(0, rotate, 0); }
                else if(_axis == Axis.Z) { target.Rotate(0, 0, rotate); }
            }
            return true;
        }
    }
}
