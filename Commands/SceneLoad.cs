namespace LFE.KeyboardShortcuts.Commands
{
    public class SceneLoad : Command
    {
        public override bool Execute(CommandExecuteEventArgs args)
        {
            SuperController.singleton.LoadSceneDialog();
            return true;
        }
    }
}
