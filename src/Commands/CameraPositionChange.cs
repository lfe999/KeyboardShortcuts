using LFE.KeyboardShortcuts.Extensions;
using System;
using UnityEngine;
using UnityEngine.Animations;

namespace LFE.KeyboardShortcuts.Commands
{
    public class CameraPositionChange : Command
    {
        private Axis _axis;
        private float _unitsPerSecond;
        public CameraPositionChange(Axis axis, float unitPerSecond)
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

            var windowCamera = GetWindowCamera();
            if (windowCamera != null)
            {
                var direction = Vector3.right;
                if(_axis == Axis.X) { direction = Vector3.right; }
                else if (_axis == Axis.Y) { direction = Vector3.up; }
                else if (_axis == Axis.Z) { direction = Vector3.forward; }

                windowCamera.transform.Translate(direction * Time.deltaTime * _unitsPerSecond * Mathf.Abs(args.Data));
            }
            return true;
        }

        private Camera GetWindowCamera() {
            // var windowCamera = SuperController.singleton.navigationCamera;
            // var windowCamera = SuperController.singleton.MonitorCenterCamera;
            // var windowCamera = SuperController.singleton.lookCamera;

            return CameraTarget.centerTarget.targetCamera;

            // Main Camera
            // int uiMask = 1 << LayerMask.NameToLayer("UI")
            //         | 1 << LayerMask.NameToLayer("LoadUI")
            //         | 1 << LayerMask.NameToLayer("ScreenUI")
            //         | 1 << LayerMask.NameToLayer("GUI");

            // var mainCamera = CameraTarget.centerTarget.targetCamera;
            // mainCamera.cullingMask &= ~uiMask;

            // return mainCamera;

            // Window Camera
            // var atoms = SuperController.singleton.GetAtoms();
            // Atom windowAtom = atoms.Find((Atom a) => { return a.type == "WindowCamera"; });
            // if (windowAtom != null)
            // {
            //     CameraControl cameraControl = windowAtom.GetStorableByID("CameraControl") as CameraControl;
            //     if (cameraControl != null)
            //     {
            //         return cameraControl.cameraToControl;
            //     }
            // }
            // return null;
        }
    }
}
