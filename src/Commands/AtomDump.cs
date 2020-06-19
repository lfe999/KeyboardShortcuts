using System;
using System.Collections;
using LFE.KeyboardShortcuts.Extensions;

namespace LFE.KeyboardShortcuts.Commands
{
    public class AtomDump : AtomCommandBase
    {
        public AtomDump() : this((Func<Atom, bool>)null) { }
        public AtomDump(Atom atom) : this((a) => a.uid.Equals(atom.uid)) { }
        public AtomDump(Func<Atom, bool> predicate) : base(predicate) { }

        // TODO: consider integrating with acidbubbles debug plugin
        public override bool Execute(CommandExecuteEventArgs args)
        {
            var selected = TargetAtom(args);
            if (selected != null) {
                DumpObject(selected);
            }
            return true;
        }

        private void DumpObject(object thing) {
            if(thing is Atom) {
                DumpAtom(thing as Atom);

            }
            else {
                Log($"{thing}");
            }
        }

        private void DumpAtom(Atom atom) {
            Log($"atom.name = \"{atom.name}\"");
            Log($"atom.uid = \"{atom.uid}\"");
            Log($"atom.type = \"{atom.type}\"");

            var paramNames = string.Join(", ", atom.GetAllParamAndActionNames().ToArray());
            Log($"atom.params = [{paramNames}]");

            Log("atom.components = [");
            foreach(var c in atom.GetComponentsInChildren<object>()) {
                Log($"  {c}");
            }
            Log("]");

        }

        private void Log(string message) {
            SuperController.LogMessage(message);
        }
    }
}
