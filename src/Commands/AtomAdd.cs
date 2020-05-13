using System;
using System.Collections;

namespace LFE.KeyboardShortcuts.Commands
{
    public class AtomAdd : Command
    {
        private string _type;
        public AtomAdd(string type)
        {
            _type = type;
        }

        public override bool Execute(CommandExecuteEventArgs args)
        {
            SuperController.singleton.StartCoroutine(SuperController.singleton.AddAtomByType(_type));
            return true;
        }
    }
}
