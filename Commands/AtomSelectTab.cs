using LFE.KeyboardShortcuts.Extensions;
using System;
using System.Linq;

namespace LFE.KeyboardShortcuts.Commands
{
    public class AtomSelectTab : AtomCommandBase
    {
        private string _tabName;
        public AtomSelectTab(string tabName) : this(tabName, (Func<Atom, bool>)null) { }
        public AtomSelectTab(string tabName, Atom atom) : this(tabName, (a) => a.uid.Equals(atom.uid)) { }
        public AtomSelectTab(string tabName, Func<Atom, bool> predicate) : base(predicate)
        {
            _tabName = tabName;
        }

        public override bool Execute(CommandExecuteEventArgs args)
        {
            var target = TargetAtom(args);
            if (target == null) {
                SuperController.LogMessage($"nothing selected", false);
                return false;
            }

            // main controller has to be selected for the ui tab stuff to work
            var currentSelection = SuperController.singleton.GetSelectedAtom();
            if(!target.uid.Equals(currentSelection?.uid))
            {
                // different atom is selected
                SuperController.singleton.SelectController(TargetAtom(args)?.mainController);
            }

            var ui = target.GetTabSelector();
            if (ui == null)
            {
                return false;
            }

            if (!ui.HasTabName(_tabName))
            {
                SuperController.LogMessage($"{target}: no tab name {_tabName}", false);
                return false;
            }

            // make sure the atom UI is visible (it can be hidden by the main menu)
            var mainTabBar = SuperController.singleton.mainMenuUI.parent;
            var showSelectedUIButton = mainTabBar.Find((name) => name.EndsWith("/ButtonSelectedOptions"))
                .FirstOrDefault()
                ?.GetComponent<UnityEngine.UI.Button>();
            showSelectedUIButton?.onClick?.Invoke();

            // set the active tab
            if (target.mainController != null)
            {
                SuperController.singleton.SelectController(target.mainController);
            }

            ui.SetActiveTab(_tabName);
            return true;
        }
    }
}
