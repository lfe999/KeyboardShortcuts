using LFE.KeyboardShortcuts.Extensions;

namespace LFE.KeyboardShortcuts.Commands
{
    public class PluginBoolToggle : Command
    {
        private string _atomUid;
        private string _pluginName;
        private string _key;
        public PluginBoolToggle(JSONStorable plugin, string key)
        {
            _atomUid = plugin.containingAtom.uid;
            _pluginName = plugin.name;
            _key = key;
        }
        public override bool Execute(CommandExecuteEventArgs args)
        {
            var plugin = SuperController.singleton.GetPluginStorable(_atomUid, _pluginName);
            if(plugin != null)
            {
                plugin.SetBoolParamValue(_key, !plugin.GetBoolParamValue(_key));
            }
            return true;
        }
    }
}
