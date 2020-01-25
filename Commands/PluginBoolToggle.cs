namespace LFE.KeyboardShortcuts.Commands
{
    public class PluginBoolToggle : Command
    {
        private JSONStorable _plugin;
        private string _key;
        public PluginBoolToggle(JSONStorable plugin, string key)
        {
            _plugin = plugin;
            _key = key;
        }
        public override bool Execute(CommandExecuteEventArgs args)
        {
            if(_plugin != null) { _plugin.SetBoolParamValue(_key, !_plugin.GetBoolParamValue(_key)); }
            return true;
        }
    }
}
