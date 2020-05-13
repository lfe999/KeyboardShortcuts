using LFE.KeyboardShortcuts.Extensions;
using UnityEngine;

namespace LFE.KeyboardShortcuts.Commands
{
    public class PluginStringChooserChange : Command
    {
        private string _atomUid;
        private string _pluginName;
        private string _key;
        private int _incrementBy;
        public PluginStringChooserChange(JSONStorable plugin, string key, int incrementBy)
        {
            _atomUid = plugin.containingAtom.uid;
            _pluginName = plugin.name;
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

            var plugin = SuperController.singleton.GetPluginStorable(_atomUid, _pluginName);

            var choices = plugin.GetStringChooserJSONParamChoices(_key);
            var selectedIndex = choices.FindIndex((c) => c.Equals(plugin.GetStringChooserParamValue(_key)));
            var newValue = choices[Mathf.Clamp(selectedIndex + _incrementBy, 0, choices.Count - 1)];
            plugin?.SetStringChooserParamValue(_key, newValue);
            return true;
        }
    }
}
