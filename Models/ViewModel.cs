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

            var settings = _plugin.LoadBindingSettings();

            // build all of the action names
            _actionController = new ActionController();
            _bindings = new Dictionary<string, KeyBinding>();
            foreach (var actionName in _actionController.GetActionNames())
            {
                var action = _actionController.GetActionByName(actionName);

                // export this action to the plugin so it shows up in GetActions elsewhere
                _plugin.RegisterAction(new JSONStorableAction(actionName, () => action()));

                // fill in bindings from the saved settings
                if(settings != null)
                {
                    if(settings[actionName] != null)
                    {
                        try
                        {
                            var chord = settings[actionName]["chord"].Value;
                            var enabled = settings[actionName]["enabled"].AsBool;

                            var binding = KeyBinding.Build(_plugin, actionName, new KeyChord(chord));
                            binding.Enabled = enabled;
                            _bindings[actionName] = binding;
                        }
                        catch
                        {
                            SuperController.LogError($"Failed loading action {actionName} from save file");
                            _bindings[actionName] = null;
                        }
                    }
                    else
                    {
                        _bindings[actionName] = null;
                    }
                }
                // ... or use defaults if there isn't anything there yet
                else
                {
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
        }

        private string _actionCategory = "General";
        public string ActionCategory {
            get { return _actionCategory; }
            set {
                _actionCategory = value;
                ClearUI();
                InitUI();
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

        private List<Action> _uiCleanup = new List<Action>();

        public void ClearUI()
        {
            // if there are _uiCleanup actions that need to be done before
            // rendering again, then
            foreach(var uiCleanup in _uiCleanup)
            {
                uiCleanup();
            }
            _uiCleanup = new List<Action>();
        }

        public void InitUI()
        {
            // add the action filter
            var actionFilterStorable = new JSONStorableStringChooser("category", _actionController.GetActionCategories().OrderBy((x) => x).ToList(), ActionCategory, "Actions For");
            actionFilterStorable.setCallbackFunction = (category) =>
            {
                ActionCategory = category;
            };
            var actionFilterUi = _plugin.CreateScrollablePopup(actionFilterStorable);
            _uiCleanup.Add(() => _plugin.RemovePopup(actionFilterUi));

            // add an empty area to the right for spacing
            var spacingUi = _plugin.CreateSpacer(rightSide: true);
            spacingUi.height = actionFilterUi.height;
            _uiCleanup.Add(() => _plugin.RemoveSpacer(spacingUi));

            // setup the UI for all available actions
            foreach (var item in _bindings.OrderBy((kvp) => kvp.Key))
            {
                var actionName = item.Key;
                var actionGroup = _actionController.GetActionCategory(actionName);
                var binding = item.Value;
                var enabled = binding?.Enabled ?? false;

                if(!actionGroup.Equals(ActionCategory))
                {
                    continue;
                }

                // checkbox on the left
                var checkboxStorable = new JSONStorableBool(actionName, enabled, (JSONStorableBool newStorable) => {
                    var b = GetKeyBinding(actionName);
                    if (b != null)
                    {
                        b.Enabled = newStorable.val;
                        SetKeyBinding(actionName, b);
                        _plugin.SaveBindingSettings();
                    }
                });

                var checkboxUi = _plugin.CreateToggle(checkboxStorable);
                checkboxUi.labelText.resizeTextMaxSize = checkboxUi.labelText.fontSize;
                checkboxUi.labelText.resizeTextForBestFit = true;
                checkboxUi.backgroundColor = Color.clear;

                _uiCleanup.Add(() => _plugin.RemoveToggle(checkboxUi));

                // assigned (optional) textbox on the right
                var shortcutText = binding == null ? "" : binding.KeyChord.ToString();
                var shortcutButtonUi = _plugin.CreateButton(shortcutText, rightSide: true);
                shortcutButtonUi.height = checkboxUi.height;
                shortcutButtonUi.button.onClick.AddListener(() =>
                {
                    if (IsRecording)
                    {
                        SuperController.LogError("Already recording", false);
                        return;
                    }

                    var origButtonText = shortcutButtonUi.buttonText.text;

                    shortcutButtonUi.buttonText.text = "recording...";
                    _keyRecorder = new KeyRecorder(actionName, (recordedChord) =>
                    {
                        var recordedChordText = recordedChord.ToString();

                        // if user recorded "esc" then exit out assuming clearning the 
                        // binding was what was wanted
                        if (recordedChordText == "Escape")
                        {
                            ClearKeyBinding(actionName);
                            shortcutButtonUi.buttonText.text = "";
                            checkboxStorable.SetVal(false);
                            _plugin.SaveBindingSettings();
                            return;
                        }

                        var keyBinding = GetKeyBinding(actionName);
                        if (keyBinding != null)
                        {
                            var keyChord = keyBinding.KeyChord;
                            var keyChordInUse = KeyBindings
                                .Where((x) => !x.Name.Equals(actionName)) // don't look at "us"
                                .Any((x) => x.KeyChord.Equals(keyChord)); // but see if the chord is used anywhere else

                            if (keyChordInUse)
                            {
                                shortcutButtonUi.buttonText.text = origButtonText;
                                SuperController.LogError("This key is already assigned to another action");
                                return;
                            }

                            ClearKeyBinding(actionName);

                        }

                        SetKeyBinding(actionName, KeyBinding.Build(_plugin, actionName, recordedChord));
                        shortcutButtonUi.buttonText.text = recordedChordText;
                        _plugin.SaveBindingSettings();
                    });

                    checkboxStorable.SetVal(true);
                });

                _uiCleanup.Add(() => _plugin.RemoveButton(shortcutButtonUi));
            }
        }
    }
}
