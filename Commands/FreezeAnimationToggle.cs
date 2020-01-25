namespace LFE.KeyboardShortcuts.Commands
{
    public class FreezeAnimationToggle : Command
    {
        public override bool Execute(CommandExecuteEventArgs args)
        {
            SuperController.singleton.SetFreezeAnimation(!SuperController.singleton.freezeAnimation);
            return true;
        }
    }
}
