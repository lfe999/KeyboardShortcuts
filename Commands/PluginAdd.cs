using LFE.KeyboardShortcuts.Extensions;
using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

namespace LFE.KeyboardShortcuts.Commands
{
    public class PluginAdd : Command
    {
        private Atom _atom;
        private bool _showFilePrompt;
        private bool _openPluginUi;
        public PluginAdd(Atom atom, bool showFilePrompt = false, bool openPluginUi = false)
        {
            _atom = atom;
            _showFilePrompt = showFilePrompt || openPluginUi;
            _openPluginUi = openPluginUi;
        }

        public override bool Execute(CommandExecuteEventArgs args)
        {
            // select the atom plugin tab first
            new AtomSelectTab("Plugins", _atom).Execute(args);

            // click the "add" button
            var ui = _atom.GetComponentInChildren<MVRPluginManagerUI>();
            if(ui != null)
            {
                ui?.addPluginButton?.onClick?.Invoke();
                if(_showFilePrompt)
                {
                    // find the newest empty plugin ui section that was added
                    var lastAddedPluginUi = _atom.GetComponentsInChildren<MVRPluginUI>()
                        .Where((x) => x.urlText.text.Equals(string.Empty))
                        .LastOrDefault();
                    if(lastAddedPluginUi != null)
                    {
                        // prompt the user to select a file
                        lastAddedPluginUi.fileBrowseButton?.onClick?.Invoke();
                        // wait up to 120 seconds for user to have made a choice and open
                        _atom.StartCoroutine(Waiter(120, lastAddedPluginUi, (found) =>
                        {
                            if(found != null && _openPluginUi)
                            {
                                found.openUIButton?.onClick?.Invoke();
                            }
                            return;
                        }));
                    }
                }
            }
            return true;
        }

        private IEnumerator Waiter(int maxSeconds, MVRPluginUI watchPluginUi, Action<MVRScriptControllerUI> onComplete)
        {
            var timer = Stopwatch.StartNew();
            while(timer.Elapsed.TotalSeconds < maxSeconds)
            {
                yield return new WaitForSeconds(0.25f);
                // do we have any script controls yet?
                var scriptControllers = watchPluginUi.GetComponentsInChildren<MVRScriptControllerUI>().FirstOrDefault();
                if(scriptControllers != null)
                {
                    try { onComplete.Invoke(scriptControllers); } catch(Exception e) { SuperController.LogError(e.ToString()); }
                    yield break;
                }
            }
            timer.Stop();
            try { onComplete.Invoke(null); } catch(Exception e) { SuperController.LogError(e.ToString()); }
            yield break;
        }
    }
}
