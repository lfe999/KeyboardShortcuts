using LFE.KeyboardShortcuts.Models;
using System;

namespace LFE.KeyboardShortcuts.Commands
{
    public class CommandExecuteEventArgs : EventArgs
    {
        public KeyBinding KeyBinding { get; set; }
        public float Data { get; set; } = 0f;
        public bool IsRepeat { get; set; } = false;
    }
}
