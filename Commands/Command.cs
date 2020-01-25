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

        public abstract bool Execute(CommandExecuteEventArgs args);
    }
}
