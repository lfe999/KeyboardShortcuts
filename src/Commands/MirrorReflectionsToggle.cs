namespace LFE.KeyboardShortcuts.Commands
{
    public class MirrorReflectionsToggle : Command
    {
        public override bool Execute(CommandExecuteEventArgs args)
        {
            UserPreferences.singleton.mirrorReflections = !UserPreferences.singleton.mirrorReflections;
            return true;
        }
    }
}
