namespace LFE.KeyboardShortcuts.Commands
{
    public class PluginActionCall : Command
    {
        private JSONStorable _plugin;
        private string _key;
        public PluginActionCall(JSONStorable plugin, string key)
        {
            _plugin = plugin;
            _key = key;
        }
        public override bool Execute(CommandExecuteEventArgs args)
        {
            _plugin?.CallAction(_key);
            return true;
        }
    }
}
