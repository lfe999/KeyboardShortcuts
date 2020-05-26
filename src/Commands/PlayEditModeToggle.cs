namespace LFE.KeyboardShortcuts.Commands
{
    public class PlayEditModeToggle : Command
    {
        public override bool Execute(CommandExecuteEventArgs args)
        {
            switch(SuperController.singleton.gameMode) {
                case SuperController.GameMode.Edit:
                    SuperController.singleton.gameMode = SuperController.GameMode.Play;
                    break;
                case SuperController.GameMode.Play:
                    SuperController.singleton.gameMode = SuperController.GameMode.Edit;
                    break;
            }
            return true;
        }
    }
}
