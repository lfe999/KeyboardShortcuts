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
        public AtomPositionSetLerp(Axis axis, float absolutePositionMin, float absolutePositionMax) : this(axis, absolutePositionMin, absolutePositionMax, (Func<Atom, bool>)null) { }
        public AtomPositionSetLerp(Axis axis, float absolutePositionMin, float absolutePositionMax, Atom atom) : this(axis, absolutePositionMin, absolutePositionMax, (a) => a.uid.Equals(atom.uid)) { }
        public AtomPositionSetLerp(Axis axis, float absolutePositionMin, float absolutePositionMax, Func<Atom, bool> predicate) : base(predicate)
        {
            _axis = axis;
            _min = absolutePositionMin;
            _max = absolutePositionMax;
        }

        public override bool Execute(CommandExecuteEventArgs args)
        {
            var selected = TargetAtom(args);
            if (selected != null)
            {
                float proportion = Mathf.Lerp(0, 1, Mathf.Abs(args.Data));
                var transform = selected.freeControllers[0].transform;
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
