using LFE.KeyboardShortcuts.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using LFE.KeyboardShortcuts.Utils;
using LFE.KeyboardShortcuts.Main;

namespace LFE.KeyboardShortcuts.Models
{
    public class ViewModel
    {
        public Plugin Plugin { get; private set; }
        private CommandFactory _actionController;
        private Dictionary<string, KeyBinding> _bindings;
        public KeyRecorder keyRecorder;

        private SuperController.OnAtomUIDsChanged _onAtomUIDsChanged;

        public ViewModel(Plugin plugin)
        {
            Plugin = plugin;

            _onAtomUIDsChanged = (uidList) =>
            {
                if(!SuperController.singleton.isLoading) {
                    Initialize();
                }
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
            _actionFilterStorable = null;
            _actionFilterUi = null;
        }

        public string ActionCategory { get; private set; } = CommandConst.CAT_GENERAL;

        public string ActionSubCategory { get; private set; } = CommandConst.SUBCAT_DEFAULT;

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
            if (!_bindings.ContainsKey(name)) { throw new ArgumentException($"Invalid action name {name}", nameof(name)); }
            _bindings[name] = binding;
        }

        private Dictionary<string, string> _lastAtomPluginInfo;
        private float _lastAtomPluginInfoTimer = 0.0f;
        public void CheckPluginsHaveChanged()
        {
            _lastAtomPluginInfoTimer += Time.deltaTime;

            if(SuperController.singleton.isLoading) {
                // Don't loop through scene information looking for changes if it is in the process of loading.
                return;
            }

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

        private Queue<BindingEvent> _actionQueueUpdatePhase = new Queue<BindingEvent>();
        private Queue<BindingEvent> _actionQueueFixedUpdatePhase = new Queue<BindingEvent>();
        public void EnqueueAction(BindingEvent e) {
            switch(e.Command.RunPhase) {
                case CommandConst.RUNPHASE_FIXED_UPDATE:
                    _actionQueueFixedUpdatePhase.Enqueue(e);
                    break;
                case CommandConst.RUNPHASE_UPDATE:
                default:
                    _actionQueueUpdatePhase.Enqueue(e);
                    break;
            }
        }

        public IEnumerable<BindingEvent> DequeueActionForUpdate() {
            for(var i = 0; i < _actionQueueUpdatePhase.Count; i++) {
                yield return _actionQueueUpdatePhase.Dequeue();
            }
        }

        public IEnumerable<BindingEvent> DequeueActionForFixedUpdate() {
            for(var i = 0; i < _actionQueueFixedUpdatePhase.Count; i++) {
                yield return _actionQueueFixedUpdatePhase.Dequeue();
            }
        }

        public void InitializeUI()
        {
            ClearUI();
            InitUI();
        }

        public void Initialize()
        {
            using(var timing = TimingLogger.Track("Initialize()"))
            {
                _actionController = new CommandFactory(Plugin);
                InitBindings();
                InitializeUI();
            }
        }

        public bool IsRecording => keyRecorder != null;
        public void RecordUpdate()
        {
            keyRecorder?.Record();
        }
        public void RecordFinish()
        {
            if (keyRecorder != null && keyRecorder.InProgress)
            {
                try
                {
                    keyRecorder.OnFinish();
                }
                catch(Exception e)
                {
                    SuperController.LogError(e.ToString());
                }
                keyRecorder = null;
            }
        }

        private Dictionary<string, Command> _commandsByName = new Dictionary<string, Command>();
        private void InitBindings()
        {
            _bindings = new Dictionary<string, KeyBinding>();
            var settings = Plugin.LoadBindingSettings();

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

                            var binding = KeyBinding.Build(Plugin, commandName, new KeyChord(chord), command);
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
                        _bindings[commandName] = KeyBinding.Build(Plugin, commandName, defaultKeyChord, command);
                    }
                    else
                    {
                        _bindings[commandName] = null;
                    }
                }
            }
        }

        private void ClearUI()
        {
            foreach(var item in _uiBindings)
            {
                item.Destroy();
            }
            _uiBindings = new List<UIBindingRow>();
        }

        private IEnumerable<string> GetGroupNames()
        {
            return _commandsByName.Values
                .Select((c) => c.Group)
                .Distinct();
        }

        private IEnumerable<string> GetSubGroupNames(string category)
        {
            return _commandsByName.Values
                .Where((c) => c.Group.Equals(category))
                .Select((c) => c.SubGroup)
                .Distinct();
        }

        private JSONStorableStringChooser _actionFilterStorable;
        private UIDynamicPopup _actionFilterUi;
        private JSONStorableStringChooser _actionSubCatFilterStorable;
        private UIDynamicPopup _actionSubCatFilterUi;

        private List<UIBindingRow> _uiBindings = new List<UIBindingRow>();

        private void InitUIHeader()
        {
            using(TimingLogger.Track("InitUIHeader()")) {
                // add the action filter

                using (TimingLogger.Track("... setup category dropdown"))
                {
                    var groupNames = GetGroupNames().OrderBy((x) => x).ToList();
                    if (_actionFilterStorable == null)
                    {
                        _actionFilterStorable = new JSONStorableStringChooser("category", groupNames, ActionCategory, "");
                        _actionFilterUi = Plugin.CreateScrollablePopup(_actionFilterStorable);
                        _actionFilterUi.labelWidth = 0f;
                        _actionFilterUi.popup.onValueChangeHandlers += (newValue) =>
                        {
                            var currentSubCategory = ActionSubCategory;
                            ActionCategory = newValue;
                            var subCategories = GetSubGroupNames(newValue).ToList();
                            if(subCategories.Count == 0)
                            {
                                ActionSubCategory = "";
                            }
                            else if(!subCategories.Any((c) => c.Equals(currentSubCategory)))
                            {
                                ActionSubCategory = subCategories[0];
                            }
                            InitializeUI();

                            // TODO: figure out how to actually get this UI element to
                            // show up on top of the UI elements just below it instead
                            // of behind
                            _actionFilterUi.popup.Toggle();
                            _actionFilterUi.popup.Toggle();
                        };
                    }
                    else
                    {
                        _actionFilterStorable.choices = groupNames;
                    }
                }

                using (TimingLogger.Track("... setup subcategory dropdown"))
                {
                    // add the subcategory filter
                    var subCategories = GetSubGroupNames(ActionCategory).OrderBy((x) => x).ToList();
                    if (_actionSubCatFilterStorable == null)
                    {
                        _actionSubCatFilterStorable = new JSONStorableStringChooser("subcategory", subCategories, ActionSubCategory, "");
                        _actionSubCatFilterUi = Plugin.CreateScrollablePopup(_actionSubCatFilterStorable, rightSide: true);
                        _actionSubCatFilterUi.height = _actionFilterUi.height;
                        _actionSubCatFilterUi.labelWidth = 0f;
                        _actionSubCatFilterUi.popup.onValueChangeHandlers += (newValue) =>
                        {
                            ActionSubCategory = newValue;
                            InitializeUI();
                            // TODO: figure out how to actually get this UI element to
                            // show up on top of the UI elements just below it instead
                            // of behind
                            _actionSubCatFilterUi.popup.Toggle();
                            _actionSubCatFilterUi.popup.Toggle();
                        };
                    }
                    else
                    {
                        _actionSubCatFilterStorable.choices = subCategories;
                        _actionSubCatFilterUi.popup.currentValueNoCallback = ActionSubCategory;
                    }
                }
            }
        }

        public List<KeyValuePair<string, KeyBinding>> GetShownBindings()
        {
            using (TimingLogger.Track("GetShowBindings()"))
            {

                var shown = new List<KeyValuePair<string, KeyBinding>>();
                foreach (var item in _bindings)
                {
                    var actionName = item.Key;
                    Command command = _commandsByName.ContainsKey(actionName) ? _commandsByName[actionName] : null;
                    if (command == null)
                    {
                        continue;
                    }

                    if(!ActionCategory.Equals(command.Group))
                    {
                        continue;
                    }

                    if(!ActionSubCategory.Equals(command.SubGroup))
                    {
                        continue;
                    }

                    shown.Add(item);
                }

                return shown;
            }
        }

        private void InitUIBindings()
        {
            var shownBindings = GetShownBindings();
            using (TimingLogger.Track($"InitUIBindings() [count: {shownBindings.Count}]"))
            {
                // show ui elements based on filters
                foreach (var item in shownBindings)
                {
                    var actionName = item.Key;
                    var command = _commandsByName[actionName];
                    _uiBindings.Add(new UIBindingRow(this, command));
                }
            }
        }

        private void InitUI()
        {
            using(TimingLogger.Track("InitUI()"))
            {
                InitUIHeader();
                InitUIBindings();
            }
        }
    }

    internal class UIBindingRow
    {

        public string ActionName { get; private set; }

        private ViewModel _model;
        private Command _command;
        private JSONStorableBool _checkboxStorable;
        private UIDynamicToggle _checkboxUi;
        private UIDynamicButton _shortcutButtonUi;

        private void OnCheckboxHandler(JSONStorableBool value)
        {
            var b = _model.GetKeyBinding(ActionName);
            if (b != null)
            {
                b.Enabled = value.val;
                _model.SetKeyBinding(ActionName, b);
                _model.Plugin.SaveBindingSettings();
            }
        }

        private void OnShortcutHandler()
        {
            if (_model.IsRecording)
            {
                SuperController.LogError("Already recording", false);
                return;
            }

            if(_shortcutButtonUi == null)
            {
                SuperController.LogError("button ui has dissappeared");
                return;
            }

            var origButtonText = _shortcutButtonUi.buttonText.text;

            _shortcutButtonUi.buttonText.text = "recording (press ESC to clear)";
            _model.keyRecorder = new KeyRecorder((recordedChord) =>
            {
                if(_shortcutButtonUi == null)
                {
                    SuperController.LogError("button ui has dissappeared");
                    return;
                }
                var recordedChordText = recordedChord.ToString();

                // if user recorded "esc" then exit out assuming clearning the
                // binding was what was wanted
                if (recordedChordText == "Escape")
                {
                    _model.ClearKeyBinding(ActionName);
                    _shortcutButtonUi.buttonText.text = "";
                    _checkboxStorable.SetVal(false);
                    _model.Plugin.SaveBindingSettings();
                    return;
                }

                var keyChordInUse = _model.KeyBindings
                    .Where((x) => !x.Name.Equals(ActionName)) // don't look at "us"
                    .Count((x) => x.KeyChord.Equals(recordedChord)) + 1; // but see if the chord is used anywhere else
                var usageMax = recordedChord.HasAxis ? 2 : 1;

                if (keyChordInUse > usageMax)
                {
                    _shortcutButtonUi.buttonText.text = origButtonText;
                    SuperController.LogError("This key is already assigned to another action");
                    return;
                }

                _model.SetKeyBinding(ActionName, KeyBinding.Build(_model.Plugin, ActionName, recordedChord, _command));
                _shortcutButtonUi.buttonText.text = recordedChordText;
                _model.Plugin.SaveBindingSettings();
            });


            _checkboxStorable.SetVal(true);
        }


        public UIBindingRow(ViewModel model, Command command)
        {
            using(var tracker = TimingLogger.Track("UIBindingRow.ctor()"))
            {
                _model = model;
                _command = command;
                ActionName = command.Name;

                var binding = _model.GetKeyBinding(ActionName);

                tracker.Log("... after binding");

                // checkbox on the left
                _checkboxStorable = new JSONStorableBool(ActionName, binding == null ? false : binding.Enabled, OnCheckboxHandler);
                _checkboxUi = _model.Plugin.CreateToggle(_checkboxStorable);
                _checkboxUi.labelText.text = command.DisplayName;
                _checkboxUi.labelText.resizeTextMaxSize = _checkboxUi.labelText.fontSize;
                _checkboxUi.labelText.resizeTextForBestFit = true;
                _checkboxUi.backgroundColor = Color.clear;

                tracker.Log("... after checkbox create");

                var shortcutText = binding == null ? "" : binding.KeyChord.ToString();
                _shortcutButtonUi = _model.Plugin.CreateButton(shortcutText, rightSide: true);
                _shortcutButtonUi.height = _checkboxUi.height;
                _shortcutButtonUi.button.onClick.AddListener(OnShortcutHandler);

                tracker.Log("... after button create");
            }
        }

        public void Destroy()
        {
            if(_checkboxUi != null)
            {
                _model.Plugin.RemoveToggle(_checkboxStorable);
                _model.Plugin.RemoveToggle(_checkboxUi);
            }
            if(_shortcutButtonUi != null)
            {
                _model.Plugin.RemoveButton(_shortcutButtonUi);
            }
        }
    }
}
