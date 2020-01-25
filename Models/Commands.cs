using LFE.KeyboardShortcuts.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Animations;

namespace LFE.KeyboardShortcuts.Models
{
    public abstract class Command
    {
        public string Name { get; set; }

        private string _displayName;
        public string DisplayName
        {
            get {
                if (_displayName == null)
                {
                    _displayName = Name;
                    if (_displayName.IndexOf("Selected > ") == 0)
                    {
                        _displayName = _displayName.Substring("Selected > ".Length);
                    }
                }
                return _displayName;
            }
            set { _displayName = value; }
        }
        public string Group { get; set; } = CommandConst.CAT_GENERAL;

        public abstract bool Execute(CommandExecuteEventArgs args);
    }

    public class CommandExecuteEventArgs : EventArgs
    {
        public KeyBinding KeyBinding { get; set; }
        public float Data { get; set; } = 0f;
        public bool IsRepeat { get; set; } = false;
    }

    public class AnimationSpeedChange : Command
    {
        private const float _multiplier = 1.0f;
        private const float _min = -3.0f;
        private const float _max = 5.0f;

        private float _amount;

        public AnimationSpeedChange(float amount)
        {
            _amount = amount;
        }

        public override bool Execute(CommandExecuteEventArgs args)
        {
            if (args.KeyBinding.KeyChord.HasAxis)
            {
                // make sure the keybinding and value change are going the
                // same "direction" (for the case where the axis is involved)
                if (!MathUtilities.SameSign(args.Data, _amount)) { return false; }
            }

            SuperController sc = SuperController.singleton;
            var multiplier = _multiplier * Mathf.Abs(args.Data);

            if (InputWrapper.GetKey(KeyCode.LeftShift) || InputWrapper.GetKey(KeyCode.RightShift))
            {
                multiplier *= 5.0f;
            }

            var scale = sc.motionAnimationMaster.playbackSpeed + (_amount * multiplier);
            sc.motionAnimationMaster.playbackSpeed = Mathf.Clamp(scale, _min, _max);
            return true;
        }
    }

    public class SoftBodyPhysicsToggle : Command
    {
        public override bool Execute(CommandExecuteEventArgs args)
        {
            UserPreferences.singleton.softPhysics = !UserPreferences.singleton.softPhysics;
            return true;
        }
    }

    public class WorldScaleChange : Command
    {
        private const float _multiplier = 1.0f;
        private const float _worldScaleMin = 0.01f;
        private const float _worldScaleMax = 10.0f;

        private float _amount = 0.0f;

        public WorldScaleChange(float amount)
        {
            _amount = amount;
        }

        public override bool Execute(CommandExecuteEventArgs args)
        {
            if (args.KeyBinding.KeyChord.HasAxis)
            {
                // make sure the keybinding and value change are going the
                // same "direction" (for the case where the axis is involved)
                if (!MathUtilities.SameSign(args.Data, _amount)) { return false; }
            }

            SuperController sc = SuperController.singleton;
            var multiplier = _multiplier * Mathf.Abs(args.Data);

            if (InputWrapper.GetKey(KeyCode.LeftShift) || InputWrapper.GetKey(KeyCode.RightShift))
            {
                multiplier *= 5.0f;
            }

            var scale = sc.worldScale + (_amount * multiplier);
            SuperController.singleton.worldScale = Mathf.Clamp(scale, _worldScaleMin, _worldScaleMax);

            //// Modify player height with scale
            Vector3 dir = Vector3.down;
            dir *= multiplier * 0.0011f;
            sc.navigationRig.position += dir;
            return true;
        }
    }

    public class FreezeAnimationSet : Command
    {
        private bool _value;
        public FreezeAnimationSet(bool value)
        {
            _value = value;
        }

        public override bool Execute(CommandExecuteEventArgs args)
        {
            SuperController.singleton.SetFreezeAnimation(_value);
            return true;
        }
    }

    public class FreezeAnimationToggle : Command
    {
        public override bool Execute(CommandExecuteEventArgs args)
        {
            SuperController.singleton.SetFreezeAnimation(!SuperController.singleton.freezeAnimation);
            return true;
        }
    }

    public class MonitorFieldOfViewChange : Command
    {
        private const float _min = 20f;
        private const float _max = 100f;

        private float _value;
        public MonitorFieldOfViewChange(float value)
        {
            _value = value;
        }

        public override bool Execute(CommandExecuteEventArgs args)
        {
            if (args.KeyBinding.KeyChord.HasAxis)
            {
                // make sure the keybinding and value change are going the
                // same "direction" (for the case where the axis is involved)
                if (!MathUtilities.SameSign(args.Data, _value)) { return false; }
            }

            SuperController.singleton.monitorCameraFOV = Mathf.Clamp(SuperController.singleton.monitorCameraFOV + _value, _min, _max);
            return true;
        }
    }

    public class TimeScaleChange : Command
    {
        private const float _multiplier = 1.0f;
        private const float _min = 0.01f;
        private const float _max = 1.0f;

        private float _value;
        public TimeScaleChange(float value)
        {
            _value = value;
        }

        public override bool Execute(CommandExecuteEventArgs args)
        {
            if (args.KeyBinding.KeyChord.HasAxis)
            {
                // make sure the keybinding and value change are going the
                // same "direction" (for the case where the axis is involved)
                if (!MathUtilities.SameSign(args.Data, _value)) { return false; }
            }

            var multiplier = _multiplier;
            if (InputWrapper.GetKey(KeyCode.LeftShift) || InputWrapper.GetKey(KeyCode.RightShift))
            {
                multiplier *= 5.0f;
            }
            var scale = TimeControl.singleton.currentScale + (_value * multiplier);
            TimeControl.singleton.currentScale = Mathf.Clamp(scale, _min, _max);
            return true;
        }

    }

    public class MessageLogToggle : Command
    {
        public override bool Execute(CommandExecuteEventArgs args)
        {
            var panel = SuperController.singleton.msgLogPanel.Find("Panel");
            var panelIsShowing = panel?.gameObject.activeInHierarchy ?? false;
            if (panelIsShowing) { SuperController.singleton.CloseMessageLogPanel(); }
            else { SuperController.singleton.OpenMessageLogPanel(); }
            return true;
        }
    }

    public class ErrorLogToggle : Command
    {
        public override bool Execute(CommandExecuteEventArgs args)
        {
            var panel = SuperController.singleton.errorLogPanel.Find("Panel");
            var panelIsShowing = panel?.gameObject.activeInHierarchy ?? false;
            if (panelIsShowing) { SuperController.singleton.CloseErrorLogPanel(); }
            else { SuperController.singleton.OpenErrorLogPanel(); }
            return true;
        }
    }

    public class AtomSelect : Command
    {
        protected Func<Atom, bool> _predicate;
        public AtomSelect(Func<Atom, bool> predicate) {
            _predicate = predicate;
        }

        public virtual Atom TargetAtom(CommandExecuteEventArgs args)
        {
            return SelectableAtoms().Where(_predicate).First() ?? SuperController.singleton.GetSelectedAtom();
        }

        public override bool Execute(CommandExecuteEventArgs args)
        {
            SuperController.singleton.SelectController(TargetAtom(args)?.mainController);
            return true;
        }

        protected IEnumerable<Atom> SelectableAtoms()
        {
            return SuperController.singleton.GetSelectableAtoms().OrderBy((a) => a.uid);
        }
    }

    public class AtomSelectNext : AtomSelect
    {
        public AtomSelectNext() : base((x) => true) { }
        public AtomSelectNext(Func<Atom, bool> predicate) : base(predicate) { }

        public override Atom TargetAtom(CommandExecuteEventArgs args)
        {
            if (args.KeyBinding.KeyChord.HasAxis)
            {
                // make sure the keybinding and value change are going the
                // same "direction" (for the case where the axis is involved)
                if (!MathUtilities.SameSign(args.Data, 1)) { return null; }
            }

            var current = SuperController.singleton.GetSelectedAtom();
            return SelectableAtoms()
                .LoopOnceStartingWhen((a) => a.uid.Equals(current?.uid))
                .First(_predicate) ?? current;
        }
    }

    public class AtomSelectPrev : AtomSelect
    {
        public AtomSelectPrev() : base((x) => true) { }
        public AtomSelectPrev(Func<Atom, bool> predicate) : base(predicate) { }

        public override Atom TargetAtom(CommandExecuteEventArgs args)
        {
            if (args.KeyBinding.KeyChord.HasAxis)
            {
                // make sure the keybinding and value change are going the
                // same "direction" (for the case where the axis is involved)
                if (!MathUtilities.SameSign(args.Data, -1)) { return null; }
            }

            var current = SuperController.singleton.GetSelectedAtom();
            return SelectableAtoms()
                .Reverse()
                .LoopOnceStartingWhen((a) => a.uid.Equals(current?.uid))
                .First(_predicate) ?? current;
        }
    }

    public abstract class AtomCommandBase : Command {
        protected Atom Atom; // null means get selected
        protected AtomCommandBase(Atom atom = null)
        {
            Atom = atom;
        }

        public Atom GetAtomTarget()
        {
            if(Atom == null) { return SuperController.singleton.GetSelectedAtom(); }
            return Atom;
        }
    }

    public class AtomDelete : AtomCommandBase
    {
        public AtomDelete(Atom atom = null) : base(atom) { }
        public override bool Execute(CommandExecuteEventArgs args)
        {
            var selected = GetAtomTarget();
            if (selected != null) { SuperController.singleton.RemoveAtom(selected); }
            return true;
        }
    }


    public class AtomHiddenToggle : AtomCommandBase
    {
        public AtomHiddenToggle(Atom atom = null) : base(atom) { }

        public override bool Execute(CommandExecuteEventArgs args)
        {
            var target = GetAtomTarget();
            if (target != null) { target.hidden = !target.hidden; }
            return true;
        }
    }

    public class AtomRotationChange : AtomCommandBase
    {
        private Axis _axis;
        private float _rotationsPerSecond;
        public AtomRotationChange(Axis axis, float rotationsPerSecond, Atom atom = null) : base(atom)
        {
            _axis = axis;
            _rotationsPerSecond = rotationsPerSecond;
        }

        public override bool Execute(CommandExecuteEventArgs args)
        {
            if (args.KeyBinding.KeyChord.HasAxis)
            {
                // make sure the keybinding and value change are going the
                // same "direction" (for the case where the axis is involved)
                if (!MathUtilities.SameSign(args.Data, _rotationsPerSecond)) { return false; }
            }

            var selected = GetAtomTarget();
            if (selected != null)
            {
                var rotate = 360 * Time.deltaTime * _rotationsPerSecond * Mathf.Abs(args.Data);
                var target = selected.freeControllers[0].transform;

                if(_axis == Axis.X) { target.Rotate(rotate, 0, 0); }
                else if(_axis == Axis.Y) { target.Rotate(0, rotate, 0); }
                else if(_axis == Axis.Z) { target.Rotate(0, 0, rotate); }
            }
            return true;
        }
    }

    public class AtomPositionSetLerp : AtomCommandBase
    {
        private Axis _axis;
        private float _min;
        private float _max;
        public AtomPositionSetLerp(Axis axis, float absolutePositionMin, float absolutePositionMax, Atom atom = null) : base(atom)
        {
            _axis = axis;
            _min = absolutePositionMin;
            _max = absolutePositionMax;
        }

        public override bool Execute(CommandExecuteEventArgs args)
        {
            var selected = GetAtomTarget();
            if (selected != null)
            {
                float proportion = Mathf.Lerp(0, 1, Mathf.Abs(args.Data));
                var transform = selected.freeControllers[0].transform;
                var newPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                var newValue = Mathf.Lerp(_min, _max, proportion);
                if (_axis == Axis.X) { newPosition.x = newValue; }
                else if (_axis == Axis.Y) { newPosition.y = newValue; }
                else if (_axis == Axis.Z) { newPosition.z = newValue; }

                transform.position = newPosition;
            }
            return true;
        }
    }

    public class AtomPositionChange : AtomCommandBase
    {
        private Axis _axis;
        private float _unitsPerSecond;
        public AtomPositionChange(Axis axis, float unitPerSecond, Atom atom = null) : base(atom)
        {
            _axis = axis;
            _unitsPerSecond = unitPerSecond;
        }

        public override bool Execute(CommandExecuteEventArgs args)
        {
            if (args.KeyBinding.KeyChord.HasAxis)
            {
                // make sure the keybinding and value change are going the
                // same "direction" (for the case where the axis is involved)
                if (!MathUtilities.SameSign(args.Data, _unitsPerSecond)) { return false; }
            }

            var selected = GetAtomTarget();
            if (selected != null)
            {
                var direction = Vector3.right;
                if(_axis == Axis.X) { direction = Vector3.right; }
                else if (_axis == Axis.Y) { direction = Vector3.up; }
                else if (_axis == Axis.Z) { direction = Vector3.forward; }

                selected.freeControllers[0].transform.Translate(direction * Time.deltaTime * _unitsPerSecond * Mathf.Abs(args.Data));
            }
            return true;
        }
    }

    public class AtomSelectTab : AtomCommandBase
    {
        private string _tabName;
        public AtomSelectTab(string tabName, Atom atom = null) : base(atom)
        {
            _tabName = tabName;
        }

        public override bool Execute(CommandExecuteEventArgs args)
        {
            var selected = GetAtomTarget();
            if (selected == null) {
                return false;
            }

            var ui = selected.GetTabSelector();
            if (ui == null)
            {
                return false;
            }

            if (!ui.HasTabName(_tabName))
            {
                return false;
            }

            // make sure the atom UI is visible (it can be hidden by the main menu)
            var mainTabBar = SuperController.singleton.mainMenuUI.parent;
            var showSelectedUIButton = mainTabBar.Find((name) => name.EndsWith("/ButtonSelectedOptions"))
                .FirstOrDefault()
                ?.GetComponent<UnityEngine.UI.Button>();
            showSelectedUIButton?.onClick?.Invoke();

            // set the active tab
            if (selected.mainController != null)
            {
                SuperController.singleton.SelectController(selected.mainController);
            }

            ui.SetActiveTab(_tabName);
            return true;
        }
    }

    public class PluginBoolSet : Command
    {
        private JSONStorable _plugin;
        private string _key;
        private bool _value;
        public PluginBoolSet(JSONStorable plugin, string key, bool value)
        {
            _plugin = plugin;
            _key = key;
            _value = value;
        }

        public override bool Execute(CommandExecuteEventArgs args)
        {
            _plugin?.SetBoolParamValue(_key, _value);
            return true;
        }
    }

    public class PluginBoolToggle : Command
    {
        private JSONStorable _plugin;
        private string _key;
        public PluginBoolToggle(JSONStorable plugin, string key)
        {
            _plugin = plugin;
            _key = key;
        }
        public override bool Execute(CommandExecuteEventArgs args)
        {
            if(_plugin != null) { _plugin.SetBoolParamValue(_key, !_plugin.GetBoolParamValue(_key)); }
            return true;
        }
    }

    public class PluginActionCall : Command
    {
        private JSONStorable _plugin;
        private string _key;
        public PluginActionCall(JSONStorable plugin, string key)
        {
            _plugin = plugin;
            _key = key;
        }
        public override bool Execute(CommandExecuteEventArgs args)
        {
            _plugin?.CallAction(_key);
            return true;
        }
    }

    public static class CommandConst
    {

        public const string CAT_GENERAL = "[General]";
        public const string CAT_SELECTEDATOM = "[Selected Atom]";
    }
}
