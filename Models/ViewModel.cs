using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LFE.KeyboardShortcuts.Models
{
    public class ViewModel
    {
        private Plugin _plugin;
        private ActionController _actionController;
        private Dictionary<string, KeyBinding> _bindings;
        private KeyRecorder _keyRecorder;

        public ViewModel(Plugin plugin)
        {
            _plugin = plugin;

            // build all of the action names
            _actionController = new ActionController(_plugin.containingAtom);
            _bindings = new Dictionary<string, KeyBinding>();
            foreach (var actionName in _actionController.List())
            {
                var action = _actionController.GetActionByName(actionName);

                // export this action to the plugin so it shows up in GetActions elsewhere
                _plugin.RegisterAction(new JSONStorableAction(actionName, () => action()));

                var defaultKeyChord = _actionController.GetDefaultKeyChordByActionName(actionName);
                if (defaultKeyChord != null)
                {
                    _bindings[actionName] = KeyBinding.Build(_plugin, actionName, defaultKeyChord);
                }
                else
                {
                    _bindings[actionName] = null;
                }
            }
        }

        public IEnumerable<string> ActionNames => _bindings.Keys;
        public IEnumerable<KeyBinding> KeyBindings => _bindings.Values.Where((b) => b != null);

        public void ClearKeyBinding(string name)
        {
            if(_bindings.ContainsKey(name))
            {
                _bindings[name] = null;
            }
        }

        public KeyBinding GetKeyBinding(string name)
        {
            return _bindings.ContainsKey(name) ? _bindings[name] : null;
        }

        public void SetKeyBinding(string name, KeyBinding binding)
        {
            if (!_bindings.ContainsKey(name)) { throw new ArgumentException("Invalid action name", nameof(name)); }
            _bindings[name] = binding;
        }

        public bool IsRecording => _keyRecorder != null;
        public void RecordUpdate()
        {
            _keyRecorder?.Record();
        }
        public void RecordFinish()
        {
            if (_keyRecorder != null && _keyRecorder.InProgress)
            {
                _keyRecorder.OnFinish();
                _keyRecorder = null;
            }
        }

        public void InitUI()
        {
            // setup the UI for all available action
            foreach (var item in _bindings.OrderBy((kvp) => kvp.Key))
            {
                var actionName = item.Key;
                var binding = item.Value;
                var enabled = binding?.Enabled ?? false;

                // checkbox on the left
                var checkboxStorable = new JSONStorableBool(actionName, enabled, (JSONStorableBool newStorable) => {
                    var b = GetKeyBinding(actionName);
                    if (b != null)
                    {
                        b.Enabled = newStorable.val;
                        SetKeyBinding(actionName, b);
                    }
                });

                var checkboxUi = _plugin.CreateToggle(checkboxStorable);
                checkboxUi.labelText.resizeTextMaxSize = checkboxUi.labelText.fontSize;
                checkboxUi.labelText.resizeTextForBestFit = true;
                checkboxUi.backgroundColor = Color.clear;

                // assigned (optional) textbox on the right
                var shortcutText = binding == null ? "" : binding.KeyChord.ToString();
                var shortcutStorable = _plugin.CreateButton(shortcutText, rightSide: true);
                shortcutStorable.height = checkboxUi.height;
                shortcutStorable.button.onClick.AddListener(() =>
                {
                    if (IsRecording)
                    {
                        SuperController.LogError("Already recording", false);
                        return;
                    }

                    var origButtonText = shortcutStorable.buttonText.text;

                    shortcutStorable.buttonText.text = "recording...";
                    _keyRecorder = new KeyRecorder(actionName, (recordedChord) =>
                    {
                        var recordedChordText = recordedChord.ToString();
                        var keyBinding = GetKeyBinding(actionName);
                        if (keyBinding != null)
                        {
                            var keyChord = keyBinding.KeyChord;
                            var keyChordInUse = KeyBindings
                                .Where((x) => !x.Name.Equals(actionName)) // don't look at "us"
                                .Any((x) => x.KeyChord.Equals(keyChord)); // but see if the chord is used anywhere else

                            if (keyChordInUse)
                            {
                                shortcutStorable.buttonText.text = origButtonText;
                                SuperController.LogError("This key is already assigned to another action");
                                return;
                            }

                            ClearKeyBinding(actionName);

                            // if user recorded "esc" then exit out assuming clearning the 
                            // binding was what was wanted
                            if (recordedChordText == "Escape")
                            {
                                shortcutStorable.buttonText.text = "";
                                checkboxStorable.SetVal(false);
                                return;
                            }
                        }

                        SetKeyBinding(actionName, KeyBinding.Build(_plugin, actionName, recordedChord));
                        shortcutStorable.buttonText.text = recordedChordText;
                    });

                    checkboxStorable.SetVal(true);
                });
            }
        }
    }
}
