using LFE.KeyboardShortcuts.Extensions;
using System;
using UnityEngine;
using UnityEngine.Animations;

namespace LFE.KeyboardShortcuts.Commands
{
    public class MouseWheelScroll : Command
    {
        private float _unitsPerSecond;
        private Func<Transform> _target;
        public MouseWheelScroll(float unitPerSecond, Func<Transform> target = null)
        {
            Func<Transform> defaultTarget = () => {
                // mimic right mouse click and drag
                return SuperController.singleton.MonitorCenterCamera.transform;
            };

            RunPhase = CommandConst.RUNPHASE_FIXED_UPDATE;
            RepeatSpeed = 0;
            RepeatDelay = 0;
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
                var amount = _unitsPerSecond * Mathf.Abs(args.Data) * Time.deltaTime;

                // from SuperController ProcessMouseControl
                // float num3 = 0.1f;
                // if (amount < -0.5f)
                // {
                //     num3 = 0f - amount;
                // }

                Vector3 forward = target.forward;
                Vector3 val8 = amount * forward * SuperController.singleton.focusDistance;
                Vector3 val9 = navigation.position + val8;
                SuperController.singleton.focusDistance *= 1f - amount;
                Vector3 up3 = navigation.up;
                float num4 = Vector3.Dot(val8, up3);
                val9 += up3 * (0f - num4);

                navigation.position = val9;
                SuperController.singleton.playerHeightAdjust += num4;
            }

            return true;
        }
    }
}
