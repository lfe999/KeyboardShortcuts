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
                if(_displayName == null)
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
        public string Group { get; set; } = "[General]";

        public abstract void Execute();
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

        public override void Execute()
        {
            SuperController sc = SuperController.singleton;
            var multiplier = _multiplier;

            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                multiplier *= 5.0f;
            }

            var scale = sc.motionAnimationMaster.playbackSpeed + (_amount * multiplier);
            sc.motionAnimationMaster.playbackSpeed = Mathf.Clamp(scale, _min, _max);
        }
    }

    public class SoftBodyPhysicsToggle : Command
    {
        public override void Execute()
        {
            UserPreferences.singleton.softPhysics = !UserPreferences.singleton.softPhysics;
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

        public override void Execute()
        {
            SuperController sc = SuperController.singleton;

            var multiplier = _multiplier;

            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                multiplier *= 5.0f;
            }

            var scale = sc.worldScale + (_amount * multiplier);
            SuperController.singleton.worldScale = Mathf.Clamp(scale, _worldScaleMin, _worldScaleMax);

            //// Modify player height with scale
            Vector3 dir = Vector3.down;
            dir *= multiplier * 0.0011f;
            sc.navigationRig.position += dir;
        }
    }

    public class FreezeAnimationSet : Command
    {
        private bool _value;
        public FreezeAnimationSet(bool value)
        {
            _value = value;
        }

        public override void Execute()
        {
            SuperController.singleton.SetFreezeAnimation(_value);
        }
    }

    public class FreezeAnimationToggle : Command
    {
        public override void Execute()
        {
            SuperController.singleton.SetFreezeAnimation(!SuperController.singleton.freezeAnimation);
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

        public override void Execute()
        {
            SuperController.singleton.monitorCameraFOV = Mathf.Clamp(SuperController.singleton.monitorCameraFOV + _value, _min, _max);
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

        public override void Execute()
        {
            var multiplier = _multiplier;
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                multiplier *= 5.0f;
            }
            var scale = TimeControl.singleton.currentScale + (_value * multiplier);
            TimeControl.singleton.currentScale = Mathf.Clamp(scale, _min, _max);
        }

    }

    public class MessageLogToggle : Command
    {
        public override void Execute()
        {
            var panel = SuperController.singleton.msgLogPanel.Find("Panel");
            var panelIsShowing = panel?.gameObject.activeInHierarchy ?? false;
            if (panelIsShowing) { SuperController.singleton.CloseMessageLogPanel(); }
            else { SuperController.singleton.OpenMessageLogPanel(); }
        }
    }

    public class ErrorLogToggle : Command
    {
        public override void Execute()
        {
            var panel = SuperController.singleton.errorLogPanel.Find("Panel");
            var panelIsShowing = panel?.gameObject.activeInHierarchy ?? false;
            if (panelIsShowing) { SuperController.singleton.CloseErrorLogPanel(); }
            else { SuperController.singleton.OpenErrorLogPanel(); }
        }
    }

    public class AtomSelect : Command
    {
        protected Func<Atom, bool> _predicate;
        public AtomSelect(Func<Atom, bool> predicate) {
            _predicate = predicate;
        }

        public virtual Atom TargetAtom()
        {
            return SelectableAtoms().Where(_predicate).First() ?? SuperController.singleton.GetSelectedAtom();
        }

        public override void Execute()
        {
            SuperController.singleton.SelectController(TargetAtom()?.mainController);
        }

        protected IEnumerable<Atom> SelectableAtoms()
        {
            return SuperController.singleton.GetAtoms().Where((a) => a.mainController != null).OrderBy((a) => a.uid);
        }
    }

    public class AtomSelectNext : AtomSelect
    {
        public AtomSelectNext() : base((x) => true) { }
        public AtomSelectNext(Func<Atom, bool> predicate) : base(predicate) { }

        public override Atom TargetAtom()
        {
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

        public override Atom TargetAtom()
        {
            var current = SuperController.singleton.GetSelectedAtom();
            return SelectableAtoms()
                .Reverse()
                .LoopOnceStartingWhen((a) => a.uid.Equals(current?.uid))
                .First(_predicate) ?? current;
        }
    }

    public class SelectedAtomDelete : Command
    {
        public override void Execute()
        {
            var selected = SuperController.singleton.GetSelectedAtom();
            if (selected != null) { SuperController.singleton.RemoveAtom(selected); }
        }
    }

    public class SelectedAtomHiddenToggle : Command
    {
        public override void Execute()
        {
            var selected = SuperController.singleton.GetSelectedAtom();
            if (selected != null) { selected.hidden = !selected.hidden; }
        }
    }

    public class SelectedAtomRotationChange : Command
    {
        private Axis _axis;
        private float _value;
        public SelectedAtomRotationChange(Axis axis, float value)
        {
            _axis = axis;
            _value = value;
        }

        public override void Execute()
        {
            var selected = SuperController.singleton.GetSelectedAtom();
            if (selected != null)
            {
                if(_axis == Axis.X) { selected.transform.Rotate(_value, 0, 0); }
                else if(_axis == Axis.Y) { selected.transform.Rotate(0, _value, 0); }
                else if(_axis == Axis.Z) { selected.transform.Rotate(0, 0, _value); }
            }
        }
    }

    public class SelectedAtomPositionChange : Command
    {
        private Axis _axis;
        private float _value;
        public SelectedAtomPositionChange(Axis axis, float value)
        {
            _axis = axis;
            _value = value;
        }

        public override void Execute()
        {
            var selected = SuperController.singleton.GetSelectedAtom();
            if (selected != null)
            {
                var position = selected.transform.position;
                if(_axis == Axis.X) { position.x += _value; }
                else if(_axis == Axis.Y) { position.y += _value; }
                else if(_axis == Axis.Z) { position.z += _value; }
                selected.transform.position = position;
            }
        }
    }

    public class SelectedAtomSelectTab : Command
    {
        private string _tabName;
        public SelectedAtomSelectTab(string tabName)
        {
            _tabName = tabName;
        }

        public override void Execute()
        {
            var selected = SuperController.singleton.GetSelectedAtom();
            if(selected == null) { return; }

            // make sure the atom UI is visible (it can be hidden by the main menu)
            var mainTabBar = SuperController.singleton.mainMenuUI.parent;
            var showSelectedUIButton = mainTabBar.Find((name) => name.EndsWith("/ButtonSelectedOptions"))
                .FirstOrDefault()
                ?.GetComponent<UnityEngine.UI.Button>();
            showSelectedUIButton?.onClick?.Invoke();

            // set the active tab
            SuperController.singleton.SelectController(selected?.mainController);
            selected?.GetTabSelector()?.SetActiveTab(_tabName);
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

        public override void Execute()
        {
            _plugin?.SetBoolParamValue(_key, _value);
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
        public override void Execute()
        {
            if(_plugin != null) { _plugin.SetBoolParamValue(_key, !_plugin.GetBoolParamValue(_key)); }
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
        public override void Execute()
        {
            _plugin?.CallAction(_key);
        }
    }

    public static class CommandConst
    {

        public const string CAT_GENERAL = "[General]";
        public const string CAT_SELECTEDATOM = "[Selected Atom]";
    }
}
