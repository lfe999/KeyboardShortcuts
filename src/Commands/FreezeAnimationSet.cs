namespace LFE.KeyboardShortcuts.Commands
{
    public class FreezeAnimationSet : Command
    {
        private bool _value;
        public FreezeAnimationSet(bool value)
        {
            _value = value;
        }

        public override bool Execute(CommandExecuteEventArgs args)
        {
            SuperController.singleton.SetFreezeAnimation(_value);
            return true;
        }
    }
}
