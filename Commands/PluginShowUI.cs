using LFE.KeyboardShortcuts.Extensions;
using System.Linq;
using UnityEngine;

namespace LFE.KeyboardShortcuts.Commands
{
    public class PluginShowUI : Command
    {
        private string _atomUid;
        private string _pluginName;
        public PluginShowUI(JSONStorable plugin)
        {
            _atomUid = plugin.containingAtom.uid;
            _pluginName = plugin.name;
        }
        public override bool Execute(CommandExecuteEventArgs args)
        {
            var plugin = SuperController.singleton.GetPluginStorable(_atomUid, _pluginName);
            if(plugin == null) { return false; }
            // make sure the atom plugin tab is showing
            if(new AtomSelectTab("Plugins", plugin.containingAtom).Execute(args))
            {
                var atom = plugin.containingAtom;
                foreach (var scriptController in atom.GetComponentsInChildren<MVRScriptControllerUI>())
                {
                    if(scriptController.label.text.Equals(plugin.name))
                    {
                        scriptController.openUIButton?.onClick?.Invoke();
                        return true;
                    }
                }
            }
            return true;
        }
    }
}
