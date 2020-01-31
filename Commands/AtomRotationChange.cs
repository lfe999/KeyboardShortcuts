﻿using System;
using UnityEngine;
using UnityEngine.Animations;

namespace LFE.KeyboardShortcuts.Commands
{
    public class AtomRotationChange : AtomCommandBase
    {
        private Axis _axis;
        private float _rotationsPerSecond;
        private FreeControllerV3 _controller;
        public AtomRotationChange(Axis axis, float rotationsPerSecond) : this(axis, rotationsPerSecond, (FreeControllerV3)null) { }
        public AtomRotationChange(Axis axis, float rotationsPerSecond, Atom atom) : this(axis, rotationsPerSecond, atom.mainController) { }
        public AtomRotationChange(Axis axis, float rotationsPerSecond, FreeControllerV3 controller) : base(null)
        {
            _axis = axis;
            _rotationsPerSecond = rotationsPerSecond;
            _controller = controller;
        }

        public override bool Execute(CommandExecuteEventArgs args)
        {
            if (args.KeyBinding.KeyChord.HasAxis)
            {
                // make sure the keybinding and value change are going the
                // same "direction" (for the case where the axis is involved)
                if (!MathUtilities.SameSign(args.Data, _rotationsPerSecond)) { return false; }
            }

            _controller = _controller ?? TargetAtom(args)?.mainController;
            if (_controller != null)
            {
                var rotate = 360 * Time.deltaTime * _rotationsPerSecond * Mathf.Abs(args.Data);
                var target = _controller.transform;

                if(_axis == Axis.X) { target.Rotate(rotate, 0, 0); }
                else if(_axis == Axis.Y) { target.Rotate(0, rotate, 0); }
                else if(_axis == Axis.Z) { target.Rotate(0, 0, rotate); }
            }
            return true;
        }
    }
}
