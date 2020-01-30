namespace LFE.KeyboardShortcuts.Commands
{
    public class ScreenShotModeOn : Command
    {
        public override bool Execute(CommandExecuteEventArgs args)
        {
            SuperController.singleton.SelectModeScreenshot();
            return true;
        }
    }
}
