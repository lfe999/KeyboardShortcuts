using LFE.KeyboardShortcuts.Extensions;
using System;
using UnityEngine;
using UnityEngine.Animations;

namespace LFE.KeyboardShortcuts.Commands
{
    public class CameraRotationChange : Command
    {
        private Axis _axis;
        private float _unitsPerSecond;
        private Camera _windowCamera;
        public CameraRotationChange(Axis axis, float unitPerSecond)
        {
            RunPhase = CommandConst.RUNPHASE_FIXED_UPDATE;
            RepeatSpeed = 0;
            RepeatDelay = 0;
            _axis = axis;
            _unitsPerSecond = unitPerSecond;
            _windowCamera = GetWindowCamera();
        }

        public override bool Execute(CommandExecuteEventArgs args)
        {
            if (args.KeyBinding.KeyChord.HasAxis)
            {
                // make sure the keybinding and value change are going the
                // same "direction" (for the case where the axis is involved)
                if (!MathUtilities.SameSign(args.Data, _unitsPerSecond)) { return false; }
            }

            if (_windowCamera != null)
            {
                var rotate = 360 * Time.deltaTime * _unitsPerSecond * Mathf.Abs(args.Data);
                var target = _windowCamera.transform;

                if(_axis == Axis.X) { target.Rotate(rotate, 0, 0); }
                else if(_axis == Axis.Y) { target.Rotate(0, rotate, 0); }
                else if(_axis == Axis.Z) { target.Rotate(0, 0, rotate); }
            }
            return true;
        }

        private Camera GetWindowCamera() {
            return CameraTarget.centerTarget.targetCamera;
        }
    }
}
