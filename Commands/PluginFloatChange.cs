using UnityEngine;

namespace LFE.KeyboardShortcuts.Commands
{
    public class PluginFloatChange : Command
    {
        private JSONStorable _plugin;
        private string _key;
        private float _amount;
        public PluginFloatChange(JSONStorable plugin, string key, float amount)
        {
            _plugin = plugin;
            _key = key;
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

            var floatParam = _plugin?.GetFloatJSONParam(_key) ?? null;
            if(floatParam == null)
            {
                return false;
            }

            var newValue = Mathf.Clamp(floatParam.val + _amount, floatParam.min, floatParam.max);
            _plugin?.SetFloatParamValue(_key, newValue);
            return true;
        }
    }
}
