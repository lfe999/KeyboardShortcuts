namespace LFE.KeyboardShortcuts.Commands
{
    public class MessageLogToggle : Command
    {
        public override bool Execute(CommandExecuteEventArgs args)
        {
            var panel = SuperController.singleton.msgLogPanel.Find("Panel");
            var panelIsShowing = panel?.gameObject.activeInHierarchy ?? false;
            if (panelIsShowing) { SuperController.singleton.CloseMessageLogPanel(); }
            else { SuperController.singleton.OpenMessageLogPanel(); }
            return true;
        }
    }
}
