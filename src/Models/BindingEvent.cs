using LFE.KeyboardShortcuts.Commands;

namespace LFE.KeyboardShortcuts.Models
{
    public class BindingEvent {

        public const string ON_BINDING_UP = "OnBindingUp";
        public const string ON_BINDING_DOWN = "OnBindingDown";
        public const string ON_BINDING_REPEAT = "OnBindingRepeat";

        public string EventName { get; set; }
        public KeyBinding Binding { get; set; }
        public Command Command { get; set; }
        public CommandExecuteEventArgs Args { get; set; }

        public bool Execute() {
            switch(EventName) {
                case ON_BINDING_UP:
                    Command.Execute(Args);
                    return true;
                case ON_BINDING_REPEAT:
                    if(Binding.Enabled) {
                        return Command.Execute(Args);
                    }
                    return true;
                case ON_BINDING_DOWN:
                    if(Binding.Enabled) {
                        return Command.Execute(Args);
                    }
                    return false;
            }
            return false;
        }
    }
}
