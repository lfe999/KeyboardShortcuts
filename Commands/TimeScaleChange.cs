using LFE.KeyboardShortcuts.Extensions;
using UnityEngine;

namespace LFE.KeyboardShortcuts.Commands
{
    public class TimeScaleChange : Command
    {
        private const float _multiplier = 1.0f;
        private const float _min = 0.01f;
        private const float _max = 1.0f;

        private float _value;
        public TimeScaleChange(float value)
        {
            _value = value;
        }

        public override bool Execute(CommandExecuteEventArgs args)
        {
            if (args.KeyBinding.KeyChord.HasAxis)
            {
                // make sure the keybinding and value change are going the
                // same "direction" (for the case where the axis is involved)
                if (!MathUtilities.SameSign(args.Data, _value)) { return false; }
            }

            var multiplier = _multiplier;
            if (InputWrapper.GetKey(KeyCode.LeftShift) || InputWrapper.GetKey(KeyCode.RightShift))
            {
                multiplier *= 5.0f;
            }
            var scale = TimeControl.singleton.currentScale + (_value * multiplier);
            TimeControl.singleton.currentScale = Mathf.Clamp(scale, _min, _max);
            return true;
        }

    }
}
