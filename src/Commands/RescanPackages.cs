namespace LFE.KeyboardShortcuts.Commands
{
    public class RescanPackages : Command
    {
        public override bool Execute(CommandExecuteEventArgs args)
        {
            SuperController.singleton.RescanPackages();
            return true;
        }
    }
}
