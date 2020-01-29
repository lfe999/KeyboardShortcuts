namespace LFE.KeyboardShortcuts.Commands
{
    public class SceneNew : Command
    {
        public override bool Execute(CommandExecuteEventArgs args)
        {
            SuperController.singleton.NewScene();
            return true;
        }
    }
}
