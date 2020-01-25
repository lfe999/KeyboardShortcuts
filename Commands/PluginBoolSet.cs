namespace LFE.KeyboardShortcuts.Commands
{
    public class PluginBoolSet : Command
    {
        private JSONStorable _plugin;
        private string _key;
        private bool _value;
        public PluginBoolSet(JSONStorable plugin, string key, bool value)
        {
            _plugin = plugin;
            _key = key;
            _value = value;
        }

        public override bool Execute(CommandExecuteEventArgs args)
        {
            _plugin?.SetBoolParamValue(_key, _value);
            return true;
        }
    }
}
