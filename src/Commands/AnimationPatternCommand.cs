
using System;

namespace LFE.KeyboardShortcuts.Commands
{
    public class AnimationPatternCommand : AtomCommandBase
    {
        public const int NOTHING = 0;
        public const int PLAY = 1;
        public const int PAUSE = 2;
        public const int UNPAUSE = 3;
        public const int TOGGLE_PAUSE = 4;
        public const int RESET_AND_PLAY = 5;
        public const int RESET = 6;


        private int _action;

        public AnimationPatternCommand() : this((Func<Atom, bool>)null, NOTHING) { }
        public AnimationPatternCommand(Atom atom, int action) : this((a) => a.uid.Equals(atom.uid), action) { }
        public AnimationPatternCommand(Func<Atom, bool> predicate, int action) : base(predicate) {
            _action = action;
        }

        public override bool Execute(CommandExecuteEventArgs args)
        {
            var selected = TargetAtom(args);
            if (selected != null) {
                var pattern = selected.GetComponentInChildren<AnimationPattern>();
                if(pattern != null) {
                    switch(_action) {
                        case PLAY:
                            pattern.Play();
                            break;
                        case RESET_AND_PLAY:
                            pattern.ResetAndPlay();
                            break;
                        case PAUSE:
                            pattern.Pause();
                            break;
                        case UNPAUSE:
                            pattern.UnPause();
                            break;
                        case TOGGLE_PAUSE:
                            pattern.TogglePause();
                            break;
                        case RESET:
                            pattern.ResetAnimation();
                            break;
                    }
                }
            }
            return true;
        }
    }
}
