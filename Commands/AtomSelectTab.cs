using LFE.KeyboardShortcuts.Extensions;
using System.Linq;

namespace LFE.KeyboardShortcuts.Commands
{
    public class AtomSelectTab : AtomCommandBase
    {
        private string _tabName;
        public AtomSelectTab(string tabName, Atom atom = null) : base(atom)
        {
            _tabName = tabName;
        }

        public override bool Execute(CommandExecuteEventArgs args)
        {
            var selected = GetAtomTarget();
            if (selected == null) {
                return false;
            }

            var ui = selected.GetTabSelector();
            if (ui == null)
            {
                return false;
            }

            if (!ui.HasTabName(_tabName))
            {
                return false;
            }

            // make sure the atom UI is visible (it can be hidden by the main menu)
            var mainTabBar = SuperController.singleton.mainMenuUI.parent;
            var showSelectedUIButton = mainTabBar.Find((name) => name.EndsWith("/ButtonSelectedOptions"))
                .FirstOrDefault()
                ?.GetComponent<UnityEngine.UI.Button>();
            showSelectedUIButton?.onClick?.Invoke();

            // set the active tab
            if (selected.mainController != null)
            {
                SuperController.singleton.SelectController(selected.mainController);
            }

            ui.SetActiveTab(_tabName);
            return true;
        }
    }
}
