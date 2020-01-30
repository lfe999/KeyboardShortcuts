using System;
using UnityEngine;
using UnityEngine.Animations;

namespace LFE.KeyboardShortcuts.Commands
{
    public class AtomPositionChange : AtomCommandBase
    {
        private Axis _axis;
        private float _unitsPerSecond;
        public AtomPositionChange(Axis axis, float unitPerSecond) : this(axis, unitPerSecond, (Func<Atom, bool>)null) { }
        public AtomPositionChange(Axis axis, float unitPerSecond, Atom atom) : this(axis, unitPerSecond, (a) => a.uid.Equals(atom.uid)) { }
        public AtomPositionChange(Axis axis, float unitPerSecond, Func<Atom, bool> predicate) : base(predicate)
        {
            _axis = axis;
            _unitsPerSecond = unitPerSecond;
        }

        public override bool Execute(CommandExecuteEventArgs args)
        {
            if (args.KeyBinding.KeyChord.HasAxis)
            {
                // make sure the keybinding and value change are going the
                // same "direction" (for the case where the axis is involved)
                if (!MathUtilities.SameSign(args.Data, _unitsPerSecond)) { return false; }
            }

            var selected = TargetAtom(args);
            if (selected != null)
            {
                var direction = Vector3.right;
                if(_axis == Axis.X) { direction = Vector3.right; }
                else if (_axis == Axis.Y) { direction = Vector3.up; }
                else if (_axis == Axis.Z) { direction = Vector3.forward; }

                selected.freeControllers[0].transform.Translate(direction * Time.deltaTime * _unitsPerSecond * Mathf.Abs(args.Data));
            }
            return true;
        }
    }
}
