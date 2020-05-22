namespace LFE.KeyboardShortcuts.Commands
{
    public abstract class Command
    {
        public string Name { get; set; }

        private string _displayName;
        public string DisplayName
        {
            get {
                if (_displayName == null)
                {
                    _displayName = Name;
                    if (_displayName.IndexOf("Selected > ") == 0)
                    {
                        _displayName = _displayName.Substring("Selected > ".Length);
                    }
                }
                return _displayName;
            }
            set { _displayName = value; }
        }
        public string Group { get; set; } = CommandConst.CAT_GENERAL;
        public string SubGroup { get; set; } = CommandConst.SUBCAT_DEFAULT;
        public string RunPhase { get; set; } = CommandConst.RUNPHASE_UPDATE;
        public float RepeatDelay { get; set; } = 0.5f;
        public float RepeatSpeed { get; set; } = 0.1f;

        public abstract bool Execute(CommandExecuteEventArgs args);
    }
}
