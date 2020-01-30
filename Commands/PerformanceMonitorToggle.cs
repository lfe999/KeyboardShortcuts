using LFE.KeyboardShortcuts.Extensions;
using System.Linq;

namespace LFE.KeyboardShortcuts.Commands
{
    public class PerformanceMonitorToggle : Command
    {
        public override bool Execute(CommandExecuteEventArgs args)
        {
            var perfMonitorButton = UserPreferences.singleton.transform
                .Find((p) => p.EndsWith("PerfMon Toggle"))
                .FirstOrDefault()
                ?.GetComponent<UnityEngine.UI.Toggle>();
            if(perfMonitorButton != null)
            {
                perfMonitorButton.isOn = !perfMonitorButton.isOn;
            }
            return true;
        }
    }
}
