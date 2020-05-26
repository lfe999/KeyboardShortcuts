namespace LFE.KeyboardShortcuts.Commands
{
    public class PlayEditModeSet : Command
    {
        private SuperController.GameMode _mode;
        public PlayEditModeSet(SuperController.GameMode mode)
        {
            _mode = mode;
        }

        public override bool Execute(CommandExecuteEventArgs args)
        {
            SuperController.singleton.gameMode = _mode;
            return true;
        }
    }
}
