using UnityEngine;

namespace LFE.KeyboardShortcuts.Commands
{
    public class MonitorFieldOfViewChange : Command
    {
        private const float _min = 20f;
        private const float _max = 100f;

        private float _value;
        public MonitorFieldOfViewChange(float value)
        {
            RunPhase = CommandConst.RUNPHASE_FIXED_UPDATE;
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

            SuperController.singleton.monitorCameraFOV = Mathf.Clamp(SuperController.singleton.monitorCameraFOV + _value, _min, _max);
            return true;
        }
    }
}
