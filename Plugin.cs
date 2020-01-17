/***********************************************************************************
KeyboardShortcuts v0.3 by LFE#9677

Allows defining custom keyboard bindings to trigger actions

Thanks to ChrisTopherTa for the original idea

CHANGELOG

Version 0.3 2019-01-16
    Add more UI Tab actions available
    Reorganize code so I don't have so much scrolling to do

Version 0.2 2019-01-15
    Example shortcuts to select a tab in the UI for an atom

Version 0.1 2019-01-15
    Initial release

***********************************************************************************/
using UnityEngine;
using System;
using System.Linq;
using LFE.KeyboardShortcuts.Models;

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
            model.InitUI();
        }

        private KeyBinding _lastBindingPressed;
        private float _chordRepeatTimestamp;
        const float REPEAT_HOLD_DELAY = 0.5f; // How long do you have to hold a key before it begins repeating
        const float REPEAT_RATE_DELAY = 0.100f; // What is the delay between key repeats

        protected void Update()
        {
            if (model.IsRecording)
            {
                model.RecordUpdate(); 
            }

            // If no keys are pressed then just exit out
            if (!Input.anyKey)
            {
                if(model.IsRecording)
                {
                    model.RecordFinish();
                }
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
                _lastBindingPressed = null;
                return;
            }

            foreach (var binding in matches)
            {
                var chord = binding.KeyChord;
                var action = binding.Action;

                if (chord.IsBeingRepeated())
                {
                    // Check repeat if still holding down last key
                    if (_lastBindingPressed == binding)
                    {
                        // Still holding down the key... is it time for a repeat?
                        if (Time.unscaledTime >= _chordRepeatTimestamp)
                        {
                            _chordRepeatTimestamp = Time.unscaledTime + REPEAT_RATE_DELAY;
                            if (binding.Enabled)
                            {
                                action();
                            }
                        }
                    }
                    else
                    {
                        // Not still holding down the key? Clear last key
                        _lastBindingPressed = binding;
                        _chordRepeatTimestamp = Time.unscaledTime + REPEAT_HOLD_DELAY;
                    }
                }
                else
                {
                    // Handle the keypress and set the repeat delay timer
                    if (binding.Enabled)
                    {
                        action();
                    }
                    _lastBindingPressed = binding;
                    _chordRepeatTimestamp = Time.unscaledTime + REPEAT_HOLD_DELAY;
                    break;
                }
            }
        }
    }
}