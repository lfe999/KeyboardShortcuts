/***********************************************************************************
KeyboardShortcuts v0.7 by LFE#9677

Allows defining custom keyboard bindings to trigger actions

Thanks to ChrisTopherTa for the original idea

CHANGELOG

Version 0.7 2019-01-24
    New: Gamepad Axis support
    New: Position actions for absolute position control (from 0 - 1)
    New: Position and rotation actions now respect timescale
    New: Action names are shorter in the UI
    Fix: Keybinding in config that are not in scene don't get deleted from config file
    Refactor: all command internals refactored - so might have made more bugs

Version 0.6 2019-01-20
    New: Atom rotation and positional change actions
    New: Atom hide and delete actions
    New: Plugin registered boolean actions
    New: Plugin registered action actions
    Fix: Watch scene for changes and update actions that are listed

Version 0.5 2019-01-18
    Fix: Stop listening to shortcuts if user is typing somewhere
    Fix: Saving/Loading settings now works properly if added as Session plugin
    Fix: Lots of other "added as session plugin" related breaking is fixed

Version 0.4 2019-01-17
    New: Save settings
    New: Ability to filter shortcuts by category
    New: Toggle softbody physics action
    New: All UI panels for Person now listed as showable (thanks itsgus)
    New: Timescale and Animation speed actions (thanks itsgus)
    Fix: Hitting ESC in an empty binding field properly exits recording
    Fix: Actions show now for atom that plugin is attached to
    Fix: Shortcut that goes to tab now works if curently on main menu

    Add more UI Tab actions available
    Reorganize code so I don't have so much scrolling to do

Version 0.3 2019-01-16
    Add more UI Tab actions available
    Reorganize code so I don't have so much scrolling to do

Version 0.2 2019-01-15
    Example shortcuts to select a tab in the UI for an atom

Version 0.1 2019-01-15
    Initial release

***********************************************************************************/
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Linq;
using LFE.KeyboardShortcuts.Models;
using LFE.KeyboardShortcuts.Extensions;
using System.Collections.Generic;

namespace LFE.KeyboardShortcuts
{
    public class Plugin : MVRScript
    {

        private ViewModel model;

        public override void Init()
        {
            try
            {
                model = new ViewModel(this);
            }
            catch (Exception e)
            {
                SuperController.LogError("Exception caught: " + e);
            }
        }

        public void Start()
        {
            SaveBindingSettings();
        }

        private List<KeyValuePair<KeyBinding, Func<CommandExecuteEventArgs, bool>>> _activeBindings = new List<KeyValuePair<KeyBinding, Func<CommandExecuteEventArgs, bool>>>();

        private float _chordRepeatTimestamp;
        const float REPEAT_HOLD_DELAY = 0.5f; // How long do you have to hold a key before it begins repeating
        const float REPEAT_RATE_DELAY = 0.100f; // What is the delay between key repeats

        protected void Update()
        {
            model.CheckPluginsHaveChanged();

            if (model.IsRecording)
            {
                model.RecordUpdate();
            }

            // If no keys are pressed then just exit out
            if (!InputWrapper.AnyKeyOrAxis())
            {
                if(model.IsRecording)
                {
                    model.RecordFinish();
                }

                foreach(var noLongerPressed in _activeBindings)
                {
                    // axis commands really benefit from a final "reset to 0" command run -- so do that
                    if(noLongerPressed.Key.KeyChord.HasAxis && noLongerPressed.Value != null)
                    {
                        noLongerPressed.Value.Invoke(new CommandExecuteEventArgs
                        {
                            KeyBinding = noLongerPressed.Key,
                            Data = 0,
                            IsRepeat = true
                        });
                    }
                }
                _activeBindings = new List<KeyValuePair<KeyBinding, Func<CommandExecuteEventArgs, bool>>>();

                return;
            }

            if (model.IsRecording)
            {
                return;
            }

            // If we are typing somewhere else, don't listen for keybindings
            var focusedObject = EventSystem.current.currentSelectedGameObject;
            if(focusedObject != null && focusedObject.GetComponent<UnityEngine.UI.InputField>() != null)
            {
                return;
            }

            // get all bindings that match the keys being pressed ordered by the morst specific ones first
            var matches = model.KeyBindings
                .Where((b) => b.KeyChord.IsBeingPressed())
                .OrderByDescending((b) => b.KeyChord.Length)
                .ToList();

            // nothing being pressed
            if (matches.Count == 0)
            {
                _activeBindings = new List<KeyValuePair<KeyBinding, Func<CommandExecuteEventArgs, bool>>>();
                return;
            }

            // see if anything used to be pressed but is not anymore
            foreach(var noLongerPressed in _activeBindings.Where((kvp) => !matches.Contains(kvp.Key)))
            {
                // axis commands really benefit from a final "reset to 0" command run -- so do that
                if(noLongerPressed.Key.KeyChord.HasAxis && noLongerPressed.Value != null)
                {
                    noLongerPressed.Value.Invoke(new CommandExecuteEventArgs
                    {
                        KeyBinding = noLongerPressed.Key,
                        Data = 0,
                        IsRepeat = true
                    });
                }
            }
            // remove only the inactive bindings
            _activeBindings.RemoveAll((kvp) => !matches.Contains(kvp.Key));

            foreach (var binding in matches)
            {
                var chord = binding.KeyChord;
                var action = binding.Action;

                if (chord.IsBeingRepeated())
                {
                    // Check repeat if still holding down last key
                    if (_activeBindings.Any((kvp) => kvp.Key.Equals(binding)))
                    {
                        // Still holding down the key... is it time for a repeat?
                        if (Time.unscaledTime >= _chordRepeatTimestamp)
                        {
                            _chordRepeatTimestamp = Time.unscaledTime + REPEAT_RATE_DELAY;
                            bool actionResult = true;
                            if (binding.Enabled)
                            {
                                actionResult = action(new CommandExecuteEventArgs {
                                    KeyBinding = binding,
                                    Data = binding.KeyChord.GetPressedValue(),
                                    IsRepeat = true
                                });
                            }
                            if(actionResult)
                            {
                                // replace the active info just in case
                                _activeBindings.Where((kvp) => kvp.Key.Equals(binding)).ToList().ForEach((kvp) => new KeyValuePair<KeyBinding, Func<CommandExecuteEventArgs, bool>>(binding, action));
                            }
                        }
                    }
                    else
                    {
                        _activeBindings.Add(new KeyValuePair<KeyBinding, Func<CommandExecuteEventArgs, bool>>(binding, null));
                        _chordRepeatTimestamp = Time.unscaledTime + REPEAT_HOLD_DELAY;
                    }
                }
                else
                {
                    // Handle the keypress and set the repeat delay timer
                    bool actionResult = false;
                    if (binding.Enabled)
                    {
                        actionResult = action(new CommandExecuteEventArgs {
                            KeyBinding = binding,
                            Data = binding.KeyChord.GetPressedValue(),
                            IsRepeat = false
                        });
                    }

                    _chordRepeatTimestamp = Time.unscaledTime + REPEAT_HOLD_DELAY;

                    if(actionResult)
                    {
                        _activeBindings.Add(new KeyValuePair<KeyBinding, Func<CommandExecuteEventArgs, bool>>(binding, action));
                    }
                    else
                    {
                        _activeBindings.Add(new KeyValuePair<KeyBinding, Func<CommandExecuteEventArgs, bool>>(binding, null));
                    }
                }
            }
        }

        void OnDestroy()
        {
            model?.Destroy();
        }

        public SimpleJSON.JSONClass LoadBindingSettings()
        {
            try
            {
                return SuperController.singleton.LoadJSON($"{GetPluginPath()}/settings.json").AsObject;
            }
            catch(Exception e)
            {
                // note: can't load "System.IO" without error so we will ignore a file
                // not found error by it's text (sadface)
                if(e.Message.Contains("Could not find file"))
                {
                    // log nothing
                }
                else
                {
                    SuperController.LogError(e.ToString(), false);
                }
                return null;
            }
        }

        public void SaveBindingSettings()
        {
            // merge
            var json = LoadBindingSettings() ?? new SimpleJSON.JSONClass();

            // set any bindings that might have been updated during this session
            foreach(var binding in model.KeyBindings)
            {
                var value = new SimpleJSON.JSONClass();
                value["enabled"] = binding.Enabled.ToString();
                value["chord"] = binding.KeyChord.ToString();
                json[binding.Name] = value;
            }
            // delete any bindings that this session knows about that were cleared this session
            foreach(var bindingName in model.UnusedKeyBindings)
            {
                json.Remove(bindingName);
            }
            try
            {
                SuperController.singleton.SaveJSON(json, $"{GetPluginPath()}/settings.json");
            }
            catch(Exception e)
            {
                SuperController.LogError(e.ToString(), false);
            }
        }

        /// <summary>
        /// Absolute path to the root of this plugin
        /// </summary>
        /// <returns></returns>
        public string GetPluginPath()
        {
            SuperController.singleton.currentSaveDir = SuperController.singleton.currentLoadDir;
            string pluginId = this.storeId.Split('_')[0];
            MVRPluginManager manager = containingAtom.GetComponentInChildren<MVRPluginManager>();
            string pathToScriptFile = manager.GetJSON(true, true)["plugins"][pluginId].Value;
            string pathToScriptFolder = pathToScriptFile.Substring(0, pathToScriptFile.LastIndexOfAny(new char[] { '/', '\\' }));
            return pathToScriptFolder;
        }
    }
}