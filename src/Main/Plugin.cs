/***********************************************************************************
KeyboardShortcuts by LFE#9677

Allows defining custom keyboard bindings to trigger actions

KNOWN BUGS
VaM built in shortcuts (like "T" or "E") will always fire even if you set
  your own in this plugin. Try and avoid overlapping with VaM shortcuts.

FEATURE REQUESTS / TODO:

- Ability to share shortcuts with others and let them import them

***********************************************************************************/
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Linq;
using LFE.KeyboardShortcuts.Models;
using LFE.KeyboardShortcuts.Extensions;
using System.Collections.Generic;
using LFE.KeyboardShortcuts.Commands;

namespace LFE.KeyboardShortcuts.Main
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

        private HashSet<KeyBinding> _activeBindings = new HashSet<KeyBinding>();

        private float _chordRepeatTimestamp;

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
                    if(noLongerPressed.KeyChord.HasAxis)
                    {
                        model.EnqueueAction(
                            new BindingEvent {
                                EventName = BindingEvent.ON_BINDING_REPEAT,
                                Command = noLongerPressed.Command,
                                Binding = noLongerPressed,
                                Args = new CommandExecuteEventArgs
                                {
                                    KeyBinding = noLongerPressed,
                                    Data = 0,
                                    IsRepeat = true
                                }
                            }
                        );
                    }
                }
                _activeBindings.Clear();

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
                _activeBindings.Clear();
                return;
            }

            // see if anything used to be pressed but is not anymore
            foreach(var noLongerPressed in _activeBindings.Where((b) => !matches.Contains(b)))
            {
                // axis commands really benefit from a final "reset to 0" command run -- so do that
                if(noLongerPressed.KeyChord.HasAxis)
                {
                    model.EnqueueAction(
                        new BindingEvent {
                            EventName = BindingEvent.ON_BINDING_UP,
                            Command = noLongerPressed.Command,
                            Binding = noLongerPressed,
                            Args = new CommandExecuteEventArgs
                            {
                                KeyBinding = noLongerPressed,
                                Data = 0,
                                IsRepeat = true
                            }
                        }
                    );
                }
            }
            // remove only the inactive bindings
            _activeBindings.RemoveWhere((b) => !matches.Contains(b));

            foreach (var binding in matches)
            {
                var chord = binding.KeyChord;
                var action = binding.Action;

                // did another binding get triggered that is a superset of us?
                // (or are we a subset of an action that was just triggered)
                if(_activeBindings.Any((b) => chord.IsProperSubsetOf(b.KeyChord)))
                {
                    // .. then consider us already tiggered
                    continue;
                }

                if (chord.IsBeingRepeated())
                {
                    // Check repeat if still holding down last key
                    if (_activeBindings.Any((b) => b.Equals(binding)))
                    {
                        // Still holding down the key... is it time for a repeat?
                        if (Time.unscaledTime >= _chordRepeatTimestamp)
                        {
                            _chordRepeatTimestamp = Time.unscaledTime + binding.Command.RepeatSpeed;
                            model.EnqueueAction(
                                new BindingEvent {
                                    EventName = BindingEvent.ON_BINDING_REPEAT,
                                    Command = binding.Command,
                                    Binding = binding,
                                    Args = new CommandExecuteEventArgs
                                    {
                                        KeyBinding = binding,
                                        Data = binding.KeyChord.GetPressedValue(),
                                        IsRepeat = true
                                    }
                                }
                            );
                        }
                    }
                    else
                    {
                        _activeBindings.Add(binding);
                        _chordRepeatTimestamp = Time.unscaledTime + binding.Command.RepeatDelay;
                    }
                }
                else
                {
                    // Handle the keypress and set the repeat delay timer
                    model.EnqueueAction(
                        new BindingEvent {
                            EventName = BindingEvent.ON_BINDING_DOWN,
                            Command = binding.Command,
                            Binding = binding,
                            Args = new CommandExecuteEventArgs
                            {
                                KeyBinding = binding,
                                Data = binding.KeyChord.GetPressedValue(),
                                IsRepeat = false
                            }
                        }
                    );

                    _chordRepeatTimestamp = Time.unscaledTime + binding.Command.RepeatDelay;
                    _activeBindings.Add(binding);
                }
            }

            // run any queue commands for this phase
            foreach(var e in model.DequeueActionForUpdate()) {
                var result = e.Execute();
            }
        }

        public void FixedUpdate() {
            // run any queue commands for this phase
            foreach(var e in model.DequeueActionForFixedUpdate()) {
                var result = e.Execute();
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
