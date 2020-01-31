using System;
using UnityEngine;
using UnityEngine.Animations;

namespace LFE.KeyboardShortcuts.Commands
{
    public class AtomPositionSetLerp : AtomCommandBase
    {
        private Axis _axis;
        private float _min;
        private float _max;
        private FreeControllerV3 _controller;
        public AtomPositionSetLerp(Axis axis, float absolutePositionMin, float absolutePositionMax) : this(axis, absolutePositionMin, absolutePositionMax, (FreeControllerV3)null) { }
        public AtomPositionSetLerp(Axis axis, float absolutePositionMin, float absolutePositionMax, Atom atom) : this(axis, absolutePositionMin, absolutePositionMax, atom.mainController) { }
        public AtomPositionSetLerp(Axis axis, float absolutePositionMin, float absolutePositionMax, FreeControllerV3 controller) : base(null)
        {
            _axis = axis;
            _min = absolutePositionMin;
            _max = absolutePositionMax;
            _controller = controller;
        }

        public override bool Execute(CommandExecuteEventArgs args)
        {
            _controller = _controller ?? TargetAtom(args)?.mainController;
            if (_controller != null)
            {
                float proportion = Mathf.Lerp(0, 1, Mathf.Abs(args.Data));
                var transform = _controller.transform;
                var newPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                var newValue = Mathf.Lerp(_min, _max, proportion);
                if (_axis == Axis.X) { newPosition.x = newValue; }
                else if (_axis == Axis.Y) { newPosition.y = newValue; }
                else if (_axis == Axis.Z) { newPosition.z = newValue; }

                transform.position = newPosition;
            }
            return true;
        }
    }
}
