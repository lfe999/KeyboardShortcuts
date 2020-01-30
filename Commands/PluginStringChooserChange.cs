using UnityEngine;

namespace LFE.KeyboardShortcuts.Commands
{
    public class PluginStringChooserChange : Command
    {
        private JSONStorable _plugin;
        private string _key;
        private int _incrementBy;
        public PluginStringChooserChange(JSONStorable plugin, string key, int incrementBy)
        {
            _plugin = plugin;
            _key = key;
            _incrementBy = incrementBy;
        }

        public override bool Execute(CommandExecuteEventArgs args)
        {
            if (args.KeyBinding.KeyChord.HasAxis)
            {
                // make sure the keybinding and value change are going the
                // same "direction" (for the case where the axis is involved)
                if (!MathUtilities.SameSign(args.Data, _incrementBy)) { return false; }
            }

            var choices = _plugin.GetStringChooserJSONParamChoices(_key);
            var selectedIndex = choices.FindIndex((c) => c.Equals(_plugin.GetStringChooserParamValue(_key)));
            var newValue = choices[Mathf.Clamp(selectedIndex + _incrementBy, 0, choices.Count - 1)];
            _plugin?.SetStringChooserParamValue(_key, newValue);
            return true;
        }
    }
}
