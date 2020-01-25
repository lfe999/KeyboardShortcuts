using LFE.KeyboardShortcuts.Extensions;
using UnityEngine;

namespace LFE.KeyboardShortcuts.Commands
{
    public class WorldScaleChange : Command
    {
        private const float _multiplier = 1.0f;
        private const float _worldScaleMin = 0.01f;
        private const float _worldScaleMax = 10.0f;

        private float _amount = 0.0f;

        public WorldScaleChange(float amount)
        {
            _amount = amount;
        }

        public override bool Execute(CommandExecuteEventArgs args)
        {
            if (args.KeyBinding.KeyChord.HasAxis)
            {
                // make sure the keybinding and value change are going the
                // same "direction" (for the case where the axis is involved)
                if (!MathUtilities.SameSign(args.Data, _amount)) { return false; }
            }

            SuperController sc = SuperController.singleton;
            var multiplier = _multiplier * Mathf.Abs(args.Data);

            if (InputWrapper.GetKey(KeyCode.LeftShift) || InputWrapper.GetKey(KeyCode.RightShift))
            {
                multiplier *= 5.0f;
            }

            var scale = sc.worldScale + (_amount * multiplier);
            SuperController.singleton.worldScale = Mathf.Clamp(scale, _worldScaleMin, _worldScaleMax);

            //// Modify player height with scale
            Vector3 dir = Vector3.down;
            dir *= multiplier * 0.0011f;
            sc.navigationRig.position += dir;
            return true;
        }
    }
}
