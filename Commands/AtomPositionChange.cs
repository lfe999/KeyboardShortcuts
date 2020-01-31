using System;
using UnityEngine;
using UnityEngine.Animations;

namespace LFE.KeyboardShortcuts.Commands
{
    public class AtomPositionChange : AtomCommandBase
    {
        private Axis _axis;
        private float _unitsPerSecond;
        private FreeControllerV3 _controller;
        public AtomPositionChange(Axis axis, float unitPerSecond) : this(axis, unitPerSecond, (FreeControllerV3)null) { }
        public AtomPositionChange(Axis axis, float unitPerSecond, Atom atom) : this(axis, unitPerSecond, atom.mainController) { }
        public AtomPositionChange(Axis axis, float unitPerSecond, FreeControllerV3 controller) : base(null)
        {
            _axis = axis;
            _unitsPerSecond = unitPerSecond;
            _controller = controller;
        }

        public override bool Execute(CommandExecuteEventArgs args)
        {
            if (args.KeyBinding.KeyChord.HasAxis)
            {
                // make sure the keybinding and value change are going the
                // same "direction" (for the case where the axis is involved)
                if (!MathUtilities.SameSign(args.Data, _unitsPerSecond)) { return false; }
            }

            _controller = _controller ?? TargetAtom(args)?.mainController;
            if (_controller != null)
            {
                var direction = Vector3.right;
                if(_axis == Axis.X) { direction = Vector3.right; }
                else if (_axis == Axis.Y) { direction = Vector3.up; }
                else if (_axis == Axis.Z) { direction = Vector3.forward; }

                _controller.transform.Translate(direction * Time.deltaTime * _unitsPerSecond * Mathf.Abs(args.Data));
            }
            return true;
        }
    }
}
