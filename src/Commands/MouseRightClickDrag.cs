using LFE.KeyboardShortcuts.Extensions;
using System;
using UnityEngine;
using UnityEngine.Animations;

namespace LFE.KeyboardShortcuts.Commands
{
    public class MouseRightClickDrag : Command
    {
        private Axis _axis;
        private float _unitsPerSecond;
        private Func<Vector3?> _target;
        public MouseRightClickDrag(Axis axis, float unitPerSecond, Func<Vector3?> target = null)
        {
            Func<Vector3?> defaultTarget = () => {
                // mimic right mouse click and drag
                return
                    SuperController.singleton.MonitorCenterCamera.transform.position
                    + SuperController.singleton.MonitorCenterCamera.transform.forward
                    * SuperController.singleton.focusDistance;
            };

            RunPhase = CommandConst.RUNPHASE_FIXED_UPDATE;
            RepeatSpeed = 0;
            RepeatDelay = 0;
            _axis = axis;
            _unitsPerSecond = unitPerSecond;
            _target = target ?? defaultTarget;
        }

        public override bool Execute(CommandExecuteEventArgs args)
        {
            if (args.KeyBinding.KeyChord.HasAxis)
            {
                // make sure the keybinding and value change are going the
                // same "direction" (for the case where the axis is involved)
                if (!MathUtilities.SameSign(args.Data, _unitsPerSecond)) { return false; }
            }

            // this algorithm from Supercontroller.cs ProcessMouseControl()
            var navigation = SuperController.singleton.navigationRig;
            var target = _target();

            if(navigation != null && target != null) {
                var rotate = 360 * Time.deltaTime * _unitsPerSecond * Mathf.Abs(args.Data);

                if(_axis == Axis.X) { navigation.RotateAround(target.Value, navigation.up, rotate); }
                else if(_axis == Axis.Y) { navigation.RotateAround(target.Value, navigation.right, rotate); }
                else if(_axis == Axis.Z) { navigation.RotateAround(target.Value, navigation.forward, rotate); }
            }

            return true;
        }
    }
}
