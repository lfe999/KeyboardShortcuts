namespace LFE.KeyboardShortcuts.Commands
{
    public class ErrorLogToggle : Command
    {
        public override bool Execute(CommandExecuteEventArgs args)
        {
            var panel = SuperController.singleton.errorLogPanel.Find("Panel");
            var panelIsShowing = panel?.gameObject.activeInHierarchy ?? false;
            if (panelIsShowing) { SuperController.singleton.CloseErrorLogPanel(); }
            else { SuperController.singleton.OpenErrorLogPanel(); }
            return true;
        }
    }
}
