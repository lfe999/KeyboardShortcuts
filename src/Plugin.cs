/***********************************************************************************
KeyboardShortcuts v0.14 by LFE#9677

Allows defining custom keyboard bindings to trigger actions

KNOWN BUGS
VaM built in shortcuts (like "T" or "E") will always fire even if you set
  your own in this plugin. Try and avoid overlapping with VaM shortcuts.

FEATURE REQUESTS / TODO:

- Ability to share shortcuts with others and let them import them
- Action for toggling screenshot mode
- Feature: Ability for shortcut on current selected controller (babul)

***********************************************************************************/
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Linq;
using LFE.KeyboardShortcuts.Models;
using LFE.KeyboardShortcuts.Extensions;
using System.Collections.Generic;
using LFE.KeyboardShortcuts.Commands;

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

            // get all bindings that match the keys being pressed ordered by the most specific ones first
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

                // did another binding get triggered that is a superset of us?
                // (or are we a subset of an action that was just triggered)
                if(_activeBindings.Any((kvp) => chord.IsProperSubsetOf(kvp.Key.KeyChord)))
                {
                    // .. then consider us already tiggered
                    continue;
                }

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
                    _activeBindings.Add(new KeyValuePair<KeyBinding, Func<CommandExecuteEventArgs, bool>>(binding, actionResult ? action : null));
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
                var settings = SuperController.singleton.LoadJSON($"Saves\\lfe_keyboardshortcuts.json");
                if(settings == null) {
                    return null;
                }
                return settings.AsObject;
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
                SuperController.singleton.SaveJSON(json, $"Saves\\lfe_keyboardshortcuts.json");
            }
            catch(Exception e)
            {
                SuperController.LogError(e.ToString(), false);
            }
        }
    }
}
