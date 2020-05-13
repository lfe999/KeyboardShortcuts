namespace LFE.KeyboardShortcuts.Commands
{
    public class SceneSave : Command
    {
        public override bool Execute(CommandExecuteEventArgs args)
        {
            SuperController.singleton.SaveSceneDialog();
            return true;
        }
    }
}
