using LFE.KeyboardShortcuts.Extensions;
using UnityEngine;

namespace LFE.KeyboardShortcuts.Commands
{
    public class AnimationSpeedChange : Command
    {
        private const float _multiplier = 1.0f;
        private const float _min = -3.0f;
        private const float _max = 5.0f;

        private float _amount;

        public AnimationSpeedChange(float amount)
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

            var scale = sc.motionAnimationMaster.playbackSpeed + (_amount * multiplier);
            sc.motionAnimationMaster.playbackSpeed = Mathf.Clamp(scale, _min, _max);
            return true;
        }
    }
}
