using LFE.KeyboardShortcuts.Extensions;
using System.Linq;
using UnityEngine;

namespace LFE.KeyboardShortcuts.Commands
{
    public class PluginShowUI : Command
    {
        private JSONStorable _plugin;
        public PluginShowUI(JSONStorable plugin)
        {
            _plugin = plugin;
        }
        public override bool Execute(CommandExecuteEventArgs args)
        {
            // make sure the atom plugin tab is showing
            if(new AtomSelectTab("Plugins", _plugin.containingAtom).Execute(args))
            {
                var atom = _plugin.containingAtom;
                foreach (var scriptController in atom.GetComponentsInChildren<MVRScriptControllerUI>())
                {
                    if(scriptController.label.text.Equals(_plugin.name))
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
