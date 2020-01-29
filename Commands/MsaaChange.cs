using UnityEngine;

namespace LFE.KeyboardShortcuts.Commands
{
    public class MsaaChange : Command
    {
        private int _indexDelta;
        public MsaaChange(int indexDelta)
        {
            _indexDelta = indexDelta;
        }

        public override bool Execute(CommandExecuteEventArgs args)
        {
            if (args.KeyBinding.KeyChord.HasAxis)
            {
                // make sure the keybinding and value change are going the
                // same "direction" (for the case where the axis is involved)
                if (!MathUtilities.SameSign(args.Data, _indexDelta)) { return false; }
            }

            var values = UserPreferences.singleton.msaaPopup.popupValues;
            var currentIndex = values.ToList().FindIndex((val) => UserPreferences.singleton.msaaPopup.currentValue.Equals(val));
            var newValue = values[Mathf.Clamp(currentIndex + _indexDelta, 0, values.Length - 1)];
            UserPreferences.singleton.SetMsaaFromString(newValue);
            return true;
        }
    }
}
