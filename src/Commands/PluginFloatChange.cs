using LFE.KeyboardShortcuts.Extensions;
using UnityEngine;

namespace LFE.KeyboardShortcuts.Commands
{
    public class PluginFloatChange : Command
    {
        private string _atomUid;
        private string _pluginName;
        private string _key;
        private float _amount;
        public PluginFloatChange(JSONStorable plugin, string key, float amount)
        {
            _atomUid = plugin.containingAtom.uid;
            _pluginName = plugin.name;
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

            var plugin = SuperController.singleton.GetPluginStorable(_atomUid, _pluginName);

            var floatParam = plugin?.GetFloatJSONParam(_key) ?? null;
            if(floatParam == null)
            {
                return false;
            }

            var newValue = Mathf.Clamp(floatParam.val + _amount, floatParam.min, floatParam.max);
            plugin?.SetFloatParamValue(_key, newValue);
            return true;
        }
    }
}
