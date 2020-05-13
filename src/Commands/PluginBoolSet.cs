using LFE.KeyboardShortcuts.Extensions;

namespace LFE.KeyboardShortcuts.Commands
{
    public class PluginBoolSet : Command
    {
        private string _atomUid;
        private string _pluginName;
        private string _key;
        private bool _value;
        public PluginBoolSet(JSONStorable plugin, string key, bool value)
        {
            _atomUid = plugin.containingAtom.name;
            _pluginName = plugin.name;
            _key = key;
            _value = value;
        }

        public override bool Execute(CommandExecuteEventArgs args)
        {
            var plugin = SuperController.singleton.GetPluginStorable(_atomUid, _pluginName);
            plugin?.SetBoolParamValue(_key, _value);
            return true;
        }
    }
}
