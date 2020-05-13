namespace LFE.KeyboardShortcuts.Commands
{
    public class SoftBodyPhysicsToggle : Command
    {
        public override bool Execute(CommandExecuteEventArgs args)
        {
            UserPreferences.singleton.softPhysics = !UserPreferences.singleton.softPhysics;
            return true;
        }
    }
}
