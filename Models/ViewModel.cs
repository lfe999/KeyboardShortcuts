using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LFE.KeyboardShortcuts.Models
{
    public class ViewModel
    {
        private Plugin _plugin;
        private CommandFactory _actionController;
        private Dictionary<string, KeyBinding> _bindings;
        private KeyRecorder _keyRecorder;

        private SuperController.OnAtomUIDsChanged _onAtomUIDsChanged;

        public ViewModel(Plugin plugin)
        {
            _plugin = plugin;
            _onAtomUIDsChanged = (uidList) =>
            {
                Initialize();
            };

            SuperController.singleton.onAtomUIDsChangedHandlers += _onAtomUIDsChanged;
            Initialize();
        }

        public void Destroy()
        {
            if(_onAtomUIDsChanged != null)
            {
                SuperController.singleton.onAtomUIDsChangedHandlers -= _onAtomUIDsChanged;
            }
        }

        private string _actionCategory = CommandConst.CAT_GENERAL;
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
        public IEnumerable<string> UnusedKeyBindings => _bindings.Where((kvp) => kvp.Value == null).Select((kvp) => kvp.Key);

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

        private Dictionary<string, string> _lastAtomPluginInfo;
        private float _lastAtomPluginInfoTimer = 0.0f;
        public void CheckPluginsHaveChanged()
        {
            _lastAtomPluginInfoTimer += Time.deltaTime;
            // only poll for plugin changes once every 3 seconds for performance
            if(_lastAtomPluginInfoTimer < 3.0f) {
                return;
            }
            else
            {
                _lastAtomPluginInfoTimer = 0.0f;
            }

            var atomPluginInfo = new Dictionary<string, string>();
            foreach (var atom in SuperController.singleton.GetAtoms())
            {
                MVRPluginManager manager = atom.GetComponentInChildren<MVRPluginManager>();
                if (manager != null)
                {
                    var atomUid = atom.uid;
                    var pluginInfo = manager.GetJSON(true, true).ToString();
                    atomPluginInfo[atomUid] = pluginInfo;
                }
            }

            if(_lastAtomPluginInfo != null)
            {
                // calculate which atom names have plugin changes
                var changedAtomUids = atomPluginInfo.Except(_lastAtomPluginInfo)
                    .Concat(_lastAtomPluginInfo.Except(atomPluginInfo))
                    .Select((kvp) => kvp.Key)
                    .Distinct();
                if(changedAtomUids.Count() > 0)
                {
                    Initialize();
                }
            }

            _lastAtomPluginInfo = atomPluginInfo;
        }

        public void Initialize()
        {
            //SuperController.LogMessage("Initializing", false);
            _actionController = new CommandFactory(_plugin);
            ClearUI();
            InitBindings();
            InitUI();
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

        private Dictionary<string, Command> _commandsByName = new Dictionary<string, Command>();
        private void InitBindings()
        {
            _bindings = new Dictionary<string, KeyBinding>();
            var settings = _plugin.LoadBindingSettings();

            // build all of the commands/actions
            _commandsByName = new Dictionary<string, Command>();
            var commands = _actionController.BuildCommands();
            foreach(var command in commands)
            {
                var commandName = command.Name;

                _commandsByName[commandName] = command;
                // Consider exporting this action to the plugin so it shows up in GetActions elsewhere
                //_plugin.RegisterAction(new JSONStorableAction(commandName, () => command.Execute()));

                // fill in bindings from the saved settings
                if(settings != null)
                {
                    if(settings[commandName] != null)
                    {
                        try
                        {
                            var chord = settings[commandName]["chord"].Value;
                            var enabled = settings[commandName]["enabled"].AsBool;

                            var binding = KeyBinding.Build(_plugin, commandName, new KeyChord(chord), command);
                            binding.Enabled = enabled;
                            _bindings[commandName] = binding;
                        }
                        catch
                        {
                            SuperController.LogError($"Failed loading action {commandName} from save file");
                            _bindings[commandName] = null;
                        }
                    }
                    else
                    {
                        _bindings[commandName] = null;
                    }
                }
                // ... or use defaults if there isn't anything there yet
                else
                {
                    var defaultKeyChord = _actionController.GetDefaultKeyChordByActionName(commandName);
                    if (defaultKeyChord != null)
                    {
                        _bindings[commandName] = KeyBinding.Build(_plugin, commandName, defaultKeyChord, command);
                    }
                    else
                    {
                        _bindings[commandName] = null;
                    }
                }
            }
        }

        private List<Action> _uiCleanup = new List<Action>();

        private void ClearUI()
        {
            // if there are _uiCleanup actions that need to be done before
            // rendering again, then
            foreach(var uiCleanup in _uiCleanup)
            {
                uiCleanup();
            }
            _uiCleanup = new List<Action>();
        }

        private void InitUI()
        {
            // add the action filter
            var groupNames = _commandsByName.Values.Select((c) => c.Group).Distinct();
            var actionFilterStorable = new JSONStorableStringChooser("category", groupNames.OrderBy((x) => x).ToList(), ActionCategory, "Actions For");
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
            foreach (var item in _bindings)
            {
                var actionName = item.Key;
                Command command;
                if(!_commandsByName.TryGetValue(actionName, out command))
                {
                    continue; 
                }

                var binding = item.Value;
                var enabled = binding?.Enabled ?? false;

                if(!command.Group.Equals(ActionCategory))
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
                checkboxUi.labelText.text = command.DisplayName;
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
                    _keyRecorder = new KeyRecorder((recordedChord) =>
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

                        var keyChordInUse = KeyBindings
                            .Where((x) => !x.Name.Equals(actionName)) // don't look at "us"
                            .Count((x) => x.KeyChord.Equals(recordedChord)) + 1; // but see if the chord is used anywhere else
                        var usageMax = recordedChord.HasAxis ? 2 : 1;

                        if (keyChordInUse > usageMax)
                        {
                            shortcutButtonUi.buttonText.text = origButtonText;
                            SuperController.LogError("This key is already assigned to another action");
                            return;
                        }

                        SetKeyBinding(actionName, KeyBinding.Build(_plugin, actionName, recordedChord, command));
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
