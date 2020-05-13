using LFE.KeyboardShortcuts.Extensions;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace LFE.KeyboardShortcuts.Commands
{
    public class AtomSelectTab : AtomCommandBase
    {
        private string _tabName;
        private UnityEngine.UI.Button _showSelectedButton;
        public AtomSelectTab(string tabName) : this(tabName, (Func<Atom, bool>)null) { }
        public AtomSelectTab(string tabName, Atom atom) : this(tabName, (a) => a.uid.Equals(atom.uid)) { }
        public AtomSelectTab(string tabName, Func<Atom, bool> predicate) : base(predicate)
        {
            _tabName = tabName;
            var mainTabBar = SuperController.singleton.mainMenuUI.parent;
            _showSelectedButton = mainTabBar.Find((name) => name.EndsWith("/ButtonSelectedOptions"))
                .FirstOrDefault()
                ?.GetComponent<UnityEngine.UI.Button>();
        }

        public override bool Execute(CommandExecuteEventArgs args)
        {
            var target = TargetAtom(args);
            // there is some polling that is done with a timeout
            // so do this work async without blocking any run loop
            SuperController.singleton.StartCoroutine(DoWorkCoroutine(target));
            return true;
        }

        private IEnumerator DoWorkCoroutine(Atom target)
        {
            if (target == null) {
                SuperController.LogMessage($"nothing selected", false);
                yield return false;
            }

            // main controller has to be selected for the ui tab stuff to work
            if(!target.mainController.selected)
            {
                SuperController.singleton.SelectController(target.mainController);
                // there is a race condition where the selected controller doesn't
                // happen fast enough before getting the tab selector.. so poll for 
                // no more than 0.5 absolute seconds
                var waitUntil = Time.fixedUnscaledTime + 0.5f;
                while(!target.mainController.selected && Time.fixedUnscaledTime < waitUntil)
                {
                    yield return new WaitForSecondsRealtime(0.02f);
                }
            }

            var ui = target.GetTabSelector();
            if (ui == null)
            {
                yield return false;
            }

            if (!ui.HasTabName(_tabName))
            {
                SuperController.LogMessage($"{target}: no tab name {_tabName}", false);
                yield return false;
            }

            // set the active tab
            if (target.mainController != null)
            {
                SuperController.singleton.SelectController(target.mainController);
            }

            // we might be showing the main menu (for example user prefs) even if person is selected
            if(_showSelectedButton != null)
            {
                _showSelectedButton?.onClick?.Invoke();
            }

            ui.SetActiveTab(_tabName);
            yield return true;
        }
    }
}
