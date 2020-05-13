namespace LFE.KeyboardShortcuts.Commands
{
    public class HardReset : Command
    {
        public override bool Execute(CommandExecuteEventArgs args)
        {
            SuperController.singleton.HardReset();
            return true;
        }
    }
}
