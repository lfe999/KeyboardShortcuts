using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Animations;

using LFE.KeyboardShortcuts.Extensions;

namespace LFE.KeyboardShortcuts.Models
{
    public class ActionController {
        struct Range
        {
            public Range(float aStep, float aMin, float aMax)
            {
                step = aStep;
                min = aMin;
                max = aMax;
            }
            public float step;
            public float min;
            public float max;
        };

        public const string ACTION_CATEGORY_GENERAL = "[General]";
        public const string ACTION_CATEGORY_SELECTEDATOM = "[Selected Atom]";


        public const string ACTION_WORLDSCALE_INCREASE = "WorldScale > Increase";
        public const string ACTION_WORLDSCALE_DECREASE = "WorldScale > Decrease";
        public const string ACTION_FREEZEANIMATION_TOGGLE = "FreezeAnimation > Toggle";
        public const string ACTION_FREEZEANIMATION_ENABLE = "FreezeAnimation > On";
        public const string ACTION_FREEZEANIMATION_DISABLE = "FreezeAnimation > Off";
        public const string ACTION_FOV_INCREASE = "FieldOfView > Increase";
        public const string ACTION_FOV_DECREASE = "FieldOfView > Decrease";
        public const string ACTION_TIMESCALE_INCREASE = "TimeScale > Increase";
        public const string ACTION_TIMESCALE_DECREASE = "TimeScale > Decrease";
        public const string ACTION_ANIMATIONSPEED_INCREASE = "AnimationSpeed > Increase";
        public const string ACTION_ANIMATIONSPEED_DECREASE = "AnimationSpeed > Decrease";
        public const string ACTION_ERRORLOG_TOGGLE = "ErrorLog > Toggle";
        public const string ACTION_MESSAGELOG_TOGGLE = "MessageLog > Toggle";
        public const string ACTION_ATOMS_SELECTNEXTVISIBLE = "Atoms > Select Next Visible";
        public const string ACTION_ATOMS_SELECTPREVVISIBLE = "Atoms > Select Prev Visible";
        public const string ACTION_ATOMS_SELECTNEXT = "Atoms > Select Next";
        public const string ACTION_ATOMS_SELECTPREV = "Atoms > Select Prev";
        public const string ACTION_PREFERENCE_SOFTPHYSICS_TOGGLE = "Soft Body Physics > Toggle";

        public List<string> GeneralActionNames = new List<string> {
            ACTION_ATOMS_SELECTNEXTVISIBLE,
            ACTION_ATOMS_SELECTPREVVISIBLE,
            ACTION_ATOMS_SELECTNEXT,
            ACTION_ATOMS_SELECTPREV,
            ACTION_ERRORLOG_TOGGLE,
            ACTION_FOV_INCREASE,
            ACTION_FOV_DECREASE,
            ACTION_TIMESCALE_INCREASE,
            ACTION_TIMESCALE_DECREASE,
            ACTION_ANIMATIONSPEED_INCREASE,
            ACTION_ANIMATIONSPEED_DECREASE,
            ACTION_FREEZEANIMATION_TOGGLE,
            ACTION_FREEZEANIMATION_ENABLE,
            ACTION_FREEZEANIMATION_DISABLE,
            ACTION_MESSAGELOG_TOGGLE,
            ACTION_PREFERENCE_SOFTPHYSICS_TOGGLE,
            ACTION_WORLDSCALE_INCREASE,
            ACTION_WORLDSCALE_DECREASE,
        };

        public const string ACTION_SELECTED_HIDE_TOGGLE = "Selected > Hide > Toggle";
        public const string ACTION_SELECTED_DELETE = "Selected > Delete";
        public const string ACTION_SELECTED_X_POSITION_INCREASE_SMALL = "Selected > X > Position Increase Small";
        public const string ACTION_SELECTED_X_POSITION_DECREASE_SMALL = "Selected > X > Position Decrease Small";
        public const string ACTION_SELECTED_X_ROTATION_INCREASE_SMALL = "Selected > X > Rotation Increase Small";
        public const string ACTION_SELECTED_X_ROTATION_DECREASE_SMALL = "Selected > X > Rotation Decrease Small";
        public const string ACTION_SELECTED_Y_POSITION_INCREASE_SMALL = "Selected > Y > Position Increase Small";
        public const string ACTION_SELECTED_Y_POSITION_DECREASE_SMALL = "Selected > Y > Position Decrease Small";
        public const string ACTION_SELECTED_Y_ROTATION_INCREASE_SMALL = "Selected > Y > Rotation Increase Small";
        public const string ACTION_SELECTED_Y_ROTATION_DECREASE_SMALL = "Selected > Y > Rotation Decrease Small";
        public const string ACTION_SELECTED_Z_POSITION_INCREASE_SMALL = "Selected > Z > Position Increase Small";
        public const string ACTION_SELECTED_Z_POSITION_DECREASE_SMALL = "Selected > Z > Position Decrease Small";
        public const string ACTION_SELECTED_Z_ROTATION_INCREASE_SMALL = "Selected > Z > Rotation Increase Small";
        public const string ACTION_SELECTED_Z_ROTATION_DECREASE_SMALL = "Selected > Z > Rotation Decrease Small";
        public const string ACTION_SELECTED_X_POSITION_INCREASE_MEDIUM = "Selected > X > Position Increase Medium";
        public const string ACTION_SELECTED_X_POSITION_DECREASE_MEDIUM = "Selected > X > Position Decrease Medium";
        public const string ACTION_SELECTED_X_ROTATION_INCREASE_MEDIUM = "Selected > X > Rotation Increase Medium";
        public const string ACTION_SELECTED_X_ROTATION_DECREASE_MEDIUM = "Selected > X > Rotation Decrease Medium";
        public const string ACTION_SELECTED_Y_POSITION_INCREASE_MEDIUM = "Selected > Y > Position Increase Medium";
        public const string ACTION_SELECTED_Y_POSITION_DECREASE_MEDIUM = "Selected > Y > Position Decrease Medium";
        public const string ACTION_SELECTED_Y_ROTATION_INCREASE_MEDIUM = "Selected > Y > Rotation Increase Medium";
        public const string ACTION_SELECTED_Y_ROTATION_DECREASE_MEDIUM = "Selected > Y > Rotation Decrease Medium";
        public const string ACTION_SELECTED_Z_POSITION_INCREASE_MEDIUM = "Selected > Z > Position Increase Medium";
        public const string ACTION_SELECTED_Z_POSITION_DECREASE_MEDIUM = "Selected > Z > Position Decrease Medium";
        public const string ACTION_SELECTED_Z_ROTATION_INCREASE_MEDIUM = "Selected > Z > Rotation Increase Medium";
        public const string ACTION_SELECTED_Z_ROTATION_DECREASE_MEDIUM = "Selected > Z > Rotation Decrease Medium";
        public const string ACTION_SELECTED_X_POSITION_INCREASE_LARGE = "Selected > X > Position Increase Large";
        public const string ACTION_SELECTED_X_POSITION_DECREASE_LARGE = "Selected > X > Position Decrease Large";
        public const string ACTION_SELECTED_X_ROTATION_INCREASE_LARGE = "Selected > X > Rotation Increase Large";
        public const string ACTION_SELECTED_X_ROTATION_DECREASE_LARGE = "Selected > X > Rotation Decrease Large";
        public const string ACTION_SELECTED_Y_POSITION_INCREASE_LARGE = "Selected > Y > Position Increase Large";
        public const string ACTION_SELECTED_Y_POSITION_DECREASE_LARGE = "Selected > Y > Position Decrease Large";
        public const string ACTION_SELECTED_Y_ROTATION_INCREASE_LARGE = "Selected > Y > Rotation Increase Large";
        public const string ACTION_SELECTED_Y_ROTATION_DECREASE_LARGE = "Selected > Y > Rotation Decrease Large";
        public const string ACTION_SELECTED_Z_POSITION_INCREASE_LARGE = "Selected > Z > Position Increase Large";
        public const string ACTION_SELECTED_Z_POSITION_DECREASE_LARGE = "Selected > Z > Position Decrease Large";
        public const string ACTION_SELECTED_Z_ROTATION_INCREASE_LARGE = "Selected > Z > Rotation Increase Large";
        public const string ACTION_SELECTED_Z_ROTATION_DECREASE_LARGE = "Selected > Z > Rotation Decrease Large";

        public List<string> SelectedAtomActionNames = new List<string>
        {
            ACTION_SELECTED_HIDE_TOGGLE,
            ACTION_SELECTED_DELETE,
            ACTION_SELECTED_X_POSITION_INCREASE_SMALL,
            ACTION_SELECTED_X_POSITION_DECREASE_SMALL,
            ACTION_SELECTED_X_ROTATION_INCREASE_SMALL,
            ACTION_SELECTED_X_ROTATION_DECREASE_SMALL,
            ACTION_SELECTED_X_POSITION_INCREASE_MEDIUM,
            ACTION_SELECTED_X_POSITION_DECREASE_MEDIUM,
            ACTION_SELECTED_X_ROTATION_INCREASE_MEDIUM,
            ACTION_SELECTED_X_ROTATION_DECREASE_MEDIUM,
            ACTION_SELECTED_X_POSITION_INCREASE_LARGE,
            ACTION_SELECTED_X_POSITION_DECREASE_LARGE,
            ACTION_SELECTED_X_ROTATION_INCREASE_LARGE,
            ACTION_SELECTED_X_ROTATION_DECREASE_LARGE,
            ACTION_SELECTED_Y_POSITION_INCREASE_SMALL,
            ACTION_SELECTED_Y_POSITION_DECREASE_SMALL,
            ACTION_SELECTED_Y_ROTATION_INCREASE_SMALL,
            ACTION_SELECTED_Y_ROTATION_DECREASE_SMALL,
            ACTION_SELECTED_Y_POSITION_INCREASE_MEDIUM,
            ACTION_SELECTED_Y_POSITION_DECREASE_MEDIUM,
            ACTION_SELECTED_Y_ROTATION_INCREASE_MEDIUM,
            ACTION_SELECTED_Y_ROTATION_DECREASE_MEDIUM,
            ACTION_SELECTED_Y_POSITION_INCREASE_LARGE,
            ACTION_SELECTED_Y_POSITION_DECREASE_LARGE,
            ACTION_SELECTED_Y_ROTATION_INCREASE_LARGE,
            ACTION_SELECTED_Y_ROTATION_DECREASE_LARGE,
            ACTION_SELECTED_Z_POSITION_INCREASE_SMALL,
            ACTION_SELECTED_Z_POSITION_DECREASE_SMALL,
            ACTION_SELECTED_Z_ROTATION_INCREASE_SMALL,
            ACTION_SELECTED_Z_ROTATION_DECREASE_SMALL,
            ACTION_SELECTED_Z_POSITION_INCREASE_MEDIUM,
            ACTION_SELECTED_Z_POSITION_DECREASE_MEDIUM,
            ACTION_SELECTED_Z_ROTATION_INCREASE_MEDIUM,
            ACTION_SELECTED_Z_ROTATION_DECREASE_MEDIUM,
            ACTION_SELECTED_Z_POSITION_INCREASE_LARGE,
            ACTION_SELECTED_Z_POSITION_DECREASE_LARGE,
            ACTION_SELECTED_Z_ROTATION_INCREASE_LARGE,
            ACTION_SELECTED_Z_ROTATION_DECREASE_LARGE
        };

        private readonly Dictionary<string, string> _defaultForAction = new Dictionary<string, string>
        {
            { ACTION_ATOMS_SELECTNEXTVISIBLE, "Alt-RightArrow" },
            { ACTION_ATOMS_SELECTPREVVISIBLE, "Alt-LeftArrow" },
            { ACTION_ATOMS_SELECTNEXT, "Control-Alt-RightArrow" },
            { ACTION_ATOMS_SELECTPREV, "Control-Alt-LeftArrow" },
            { ACTION_ERRORLOG_TOGGLE, "Control-BackQuote" },
            { ACTION_FOV_DECREASE, "Shift-Minus" },
            { ACTION_FOV_INCREASE, "Shift-Equals" },
            { ACTION_TIMESCALE_DECREASE, "Control-DownArrow" },
            { ACTION_TIMESCALE_INCREASE, "Control-UpArrow" },
            { ACTION_FREEZEANIMATION_TOGGLE, "Space" },
            { ACTION_MESSAGELOG_TOGGLE, "BackQuote" }
        };

        // **** MODIFY THESE TO TWEAK PARAMETERS ****
        // Ranges for each modifiable variable. Order is 'step size', 'minimum value', 'maximum value'
        Range _worldScale = new Range(0.00025f, 0.01f, 10.0f);
        Range _timeScale = new Range(0.01f, 0.01f, 1.0f);
        Range _animationSpeed = new Range(0.05f, -3.0f, 5.0f);

        const float SHIFT_MULTIPLIER = 5.0f; // Multiplier to apply to step size if holding down shift

        const float WORLD_SCALE_HEIGHT_STEP_DEFAULT = .0011f;
        float _ws_height_mult = WORLD_SCALE_HEIGHT_STEP_DEFAULT;

        private bool _messageLogShowing = false;
        private bool _errorLogShowing = false;

        private Dictionary<string, string> _actionCategory = new Dictionary<string, string>();

        public string GetActionCategory(string action)
        {
            return _actionCategory.ContainsKey(action) ? _actionCategory[action] : ACTION_CATEGORY_GENERAL;
        }

        public List<string> GetActionCategories()
        {
            return _actionCategory.Values.Distinct().ToList();
        }

        public List<string> GetActionNames()
        {
            var names = new List<string>();

            // GENERAL actions
            GeneralActionNames.ForEach((actionName) =>
            {
                names.Add(actionName);
                _actionCategory[actionName] = ACTION_CATEGORY_GENERAL;
            });

            // SELECTED ATOM actions
            SelectedAtomActionNames.ForEach((actionName) =>
            {
                names.Add(actionName);
                _actionCategory[actionName] = ACTION_CATEGORY_SELECTEDATOM;
            });

            // SPECIFIC ATOM actions
            foreach(var atom in SuperController.singleton.GetAtoms())
            {
                foreach(var uiName in atom.GetUITabNames())
                {
                    var a = $"Atom > {atom.uid} > ShowUI > {uiName}";
                    _actionCategory[a] = atom.uid;
                    names.Add(a);
                }
            }

            return names.ToList();
        }

        public List<Atom> GetSelectableAtoms(bool includeHidden = false)
        {
            if (includeHidden)
            {
                return SuperController.singleton.GetAtoms()
                    .Where((a) => a.mainController != null)
                    .OrderBy((a) => a.uid)
                    .ToList();
            }
            else
            {
                return SuperController.singleton.GetAtoms()
                    .Where((a) => a.mainController != null)
                    .Where((a) => !a.hidden)
                    .OrderBy((a) => a.uid)
                    .ToList();
            }
        }

        public KeyChord GetDefaultKeyChordByActionName(string name)
        {
            return _defaultForAction.ContainsKey(name) ? new KeyChord(_defaultForAction[name]) : null;
        }

        public Action GetActionByName(string actionName)
        {
            Action act = null;
            switch (actionName)
            {
                case ACTION_WORLDSCALE_INCREASE:
                    act = () => ChangeWorldScale(1.0f);
                    break;
                case ACTION_WORLDSCALE_DECREASE:
                    act = () => ChangeWorldScale(-1.0f);
                    break;
                case ACTION_FREEZEANIMATION_TOGGLE:
                    act = () => TogglePause();
                    break;
                case ACTION_FREEZEANIMATION_ENABLE:
                    act = () => SuperController.singleton.SetFreezeAnimation(true);
                    break;
                case ACTION_FREEZEANIMATION_DISABLE:
                    act = () => SuperController.singleton.SetFreezeAnimation(false);
                    break;
                case ACTION_FOV_DECREASE:
                    act = () => ChangeMonitorFOV(-10.0f);
                    break;
                case ACTION_FOV_INCREASE:
                    act = () => ChangeMonitorFOV(10.0f);
                    break;
                case ACTION_TIMESCALE_DECREASE:
                    act = () => ChangeTimeScale(-10.0f);
                    break;
                case ACTION_TIMESCALE_INCREASE:
                    act = () => ChangeTimeScale(10.0f);
                    break;
                case ACTION_ANIMATIONSPEED_DECREASE:
                    act = () => ChangeAnimationSpeed(-10.0f);
                    break;
                case ACTION_ANIMATIONSPEED_INCREASE:
                    act = () => ChangeAnimationSpeed(10.0f);
                    break;                    
                case ACTION_MESSAGELOG_TOGGLE:
                    act = () => ToggleMessageLogs();
                    break;
                case ACTION_ERRORLOG_TOGGLE:
                    act = () => ToggleErrorLogs();
                    break;
                case ACTION_ATOMS_SELECTNEXTVISIBLE:
                    act = () => SelectNextAtom();
                    break;
                case ACTION_ATOMS_SELECTPREVVISIBLE:
                    act = () => SelectPrevAtom();
                    break;
                case ACTION_ATOMS_SELECTNEXT:
                    act = () => SelectNextAtom(includeHidden: true);
                    break;
                case ACTION_ATOMS_SELECTPREV:
                    act = () => SelectPrevAtom(includeHidden: true);
                    break;
                case ACTION_PREFERENCE_SOFTPHYSICS_TOGGLE:
                    act = () => ToggleSoftBodyPhysics();
                    break;
                case ACTION_SELECTED_HIDE_TOGGLE:
                    act = () => ToggleAtomHidden(SuperController.singleton.GetSelectedAtom());
                    break;
                case ACTION_SELECTED_DELETE:
                    act = () => DeleteAtom(SuperController.singleton.GetSelectedAtom());
                    break;
                case ACTION_SELECTED_X_POSITION_INCREASE_SMALL:
                    act = () => ChangeAtomPosition(SuperController.singleton.GetSelectedAtom(), Axis.X, 0.01f);
                    break;
                case ACTION_SELECTED_X_POSITION_DECREASE_SMALL:
                    act = () => ChangeAtomPosition(SuperController.singleton.GetSelectedAtom(), Axis.X, -0.01f);
                    break;
                case ACTION_SELECTED_X_ROTATION_INCREASE_SMALL:
                    act = () => ChangeAtomRotation(SuperController.singleton.GetSelectedAtom(), Axis.X, 0.5f);
                    break;
                case ACTION_SELECTED_X_ROTATION_DECREASE_SMALL:
                    act = () => ChangeAtomRotation(SuperController.singleton.GetSelectedAtom(), Axis.X, -0.5f);
                    break;
                case ACTION_SELECTED_Y_POSITION_INCREASE_SMALL:
                    act = () => ChangeAtomPosition(SuperController.singleton.GetSelectedAtom(), Axis.Y, 0.01f);
                    break;
                case ACTION_SELECTED_Y_POSITION_DECREASE_SMALL:
                    act = () => ChangeAtomPosition(SuperController.singleton.GetSelectedAtom(), Axis.Y, -0.01f);
                    break;
                case ACTION_SELECTED_Y_ROTATION_INCREASE_SMALL:
                    act = () => ChangeAtomRotation(SuperController.singleton.GetSelectedAtom(), Axis.Y, 0.5f);
                    break;
                case ACTION_SELECTED_Y_ROTATION_DECREASE_SMALL:
                    act = () => ChangeAtomRotation(SuperController.singleton.GetSelectedAtom(), Axis.Y, -0.5f);
                    break;
                case ACTION_SELECTED_Z_POSITION_INCREASE_SMALL:
                    act = () => ChangeAtomPosition(SuperController.singleton.GetSelectedAtom(), Axis.Z, 0.01f);
                    break;
                case ACTION_SELECTED_Z_POSITION_DECREASE_SMALL:
                    act = () => ChangeAtomPosition(SuperController.singleton.GetSelectedAtom(), Axis.Z, -0.01f);
                    break;
                case ACTION_SELECTED_Z_ROTATION_INCREASE_SMALL:
                    act = () => ChangeAtomRotation(SuperController.singleton.GetSelectedAtom(), Axis.Z, 0.5f);
                    break;
                case ACTION_SELECTED_Z_ROTATION_DECREASE_SMALL:
                    act = () => ChangeAtomRotation(SuperController.singleton.GetSelectedAtom(), Axis.Z, -0.5f);
                    break;
                case ACTION_SELECTED_X_POSITION_INCREASE_MEDIUM:
                    act = () => ChangeAtomPosition(SuperController.singleton.GetSelectedAtom(), Axis.X, 0.1f);
                    break;
                case ACTION_SELECTED_X_POSITION_DECREASE_MEDIUM:
                    act = () => ChangeAtomPosition(SuperController.singleton.GetSelectedAtom(), Axis.X, -0.1f);
                    break;
                case ACTION_SELECTED_X_ROTATION_INCREASE_MEDIUM:
                    act = () => ChangeAtomRotation(SuperController.singleton.GetSelectedAtom(), Axis.X, 5.0f);
                    break;
                case ACTION_SELECTED_X_ROTATION_DECREASE_MEDIUM:
                    act = () => ChangeAtomRotation(SuperController.singleton.GetSelectedAtom(), Axis.X, -5.0f);
                    break;
                case ACTION_SELECTED_Y_POSITION_INCREASE_MEDIUM:
                    act = () => ChangeAtomPosition(SuperController.singleton.GetSelectedAtom(), Axis.Y, 0.1f);
                    break;
                case ACTION_SELECTED_Y_POSITION_DECREASE_MEDIUM:
                    act = () => ChangeAtomPosition(SuperController.singleton.GetSelectedAtom(), Axis.Y, -0.1f);
                    break;
                case ACTION_SELECTED_Y_ROTATION_INCREASE_MEDIUM:
                    act = () => ChangeAtomRotation(SuperController.singleton.GetSelectedAtom(), Axis.Y, 5.0f);
                    break;
                case ACTION_SELECTED_Y_ROTATION_DECREASE_MEDIUM:
                    act = () => ChangeAtomRotation(SuperController.singleton.GetSelectedAtom(), Axis.Y, -5.0f);
                    break;
                case ACTION_SELECTED_Z_POSITION_INCREASE_MEDIUM:
                    act = () => ChangeAtomPosition(SuperController.singleton.GetSelectedAtom(), Axis.Z, 0.1f);
                    break;
                case ACTION_SELECTED_Z_POSITION_DECREASE_MEDIUM:
                    act = () => ChangeAtomPosition(SuperController.singleton.GetSelectedAtom(), Axis.Z, -0.1f);
                    break;
                case ACTION_SELECTED_Z_ROTATION_INCREASE_MEDIUM:
                    act = () => ChangeAtomRotation(SuperController.singleton.GetSelectedAtom(), Axis.Z, 5.0f);
                    break;
                case ACTION_SELECTED_Z_ROTATION_DECREASE_MEDIUM:
                    act = () => ChangeAtomRotation(SuperController.singleton.GetSelectedAtom(), Axis.Z, -5.0f);
                    break;
                case ACTION_SELECTED_X_POSITION_INCREASE_LARGE:
                    act = () => ChangeAtomPosition(SuperController.singleton.GetSelectedAtom(), Axis.X, 1.0f);
                    break;
                case ACTION_SELECTED_X_POSITION_DECREASE_LARGE:
                    act = () => ChangeAtomPosition(SuperController.singleton.GetSelectedAtom(), Axis.X, -1.0f);
                    break;
                case ACTION_SELECTED_X_ROTATION_INCREASE_LARGE:
                    act = () => ChangeAtomRotation(SuperController.singleton.GetSelectedAtom(), Axis.X, 45.0f);
                    break;
                case ACTION_SELECTED_X_ROTATION_DECREASE_LARGE:
                    act = () => ChangeAtomRotation(SuperController.singleton.GetSelectedAtom(), Axis.X, -45.0f);
                    break;
                case ACTION_SELECTED_Y_POSITION_INCREASE_LARGE:
                    act = () => ChangeAtomPosition(SuperController.singleton.GetSelectedAtom(), Axis.Y, 1.0f);
                    break;
                case ACTION_SELECTED_Y_POSITION_DECREASE_LARGE:
                    act = () => ChangeAtomPosition(SuperController.singleton.GetSelectedAtom(), Axis.Y, -1.0f);
                    break;
                case ACTION_SELECTED_Y_ROTATION_INCREASE_LARGE:
                    act = () => ChangeAtomRotation(SuperController.singleton.GetSelectedAtom(), Axis.Y, 45.0f);
                    break;
                case ACTION_SELECTED_Y_ROTATION_DECREASE_LARGE:
                    act = () => ChangeAtomRotation(SuperController.singleton.GetSelectedAtom(), Axis.Y, -45.0f);
                    break;
                case ACTION_SELECTED_Z_POSITION_INCREASE_LARGE:
                    act = () => ChangeAtomPosition(SuperController.singleton.GetSelectedAtom(), Axis.Z, 1.0f);
                    break;
                case ACTION_SELECTED_Z_POSITION_DECREASE_LARGE:
                    act = () => ChangeAtomPosition(SuperController.singleton.GetSelectedAtom(), Axis.Z, -1.0f);
                    break;
                case ACTION_SELECTED_Z_ROTATION_INCREASE_LARGE:
                    act = () => ChangeAtomRotation(SuperController.singleton.GetSelectedAtom(), Axis.Z, 45.0f);
                    break;
                case ACTION_SELECTED_Z_ROTATION_DECREASE_LARGE:
                    act = () => ChangeAtomRotation(SuperController.singleton.GetSelectedAtom(), Axis.Z, -45.0f);
                    break;
            }

            // now match actions that have variables hidden away in the action name
            var match = Regex.Match(actionName, @"^Atom > (.*?) > ShowUI > (.*?)$");
            if(match.Success)
            {
                act = () =>
                {
                    SelectAtomTab(match.Groups[1].Value, match.Groups[2].Value);
                };
            }

            if (act == null)
            {
                act = () => {
                    SuperController.LogError($"Don't know what to do for {actionName} yet");
                    return;
                };
            }
            return act;
        }

        private void SelectAtom(Atom a)
        {
            SuperController.singleton.SelectController(a?.mainController);
        }

        private void SelectAtomTab(string atomUid, string tabName)
        {
            var atom = SuperController.singleton.GetAtomByUid(atomUid);
            SelectAtomTab(atom, tabName);
        }

        private void ShowSelectedUI()
        {
            var mainTabBar = SuperController.singleton.mainMenuUI.parent;
            var showSelectedUIButton = mainTabBar
                .Find((name) => name.EndsWith("/ButtonSelectedOptions"))
                .FirstOrDefault()
                ?.GetComponent<UnityEngine.UI.Button>();

            showSelectedUIButton?.onClick?.Invoke();

        }

        private void SelectAtomTab(Atom atom, string tabName)
        {
            // make sure the selected atom has it's UI showing - if
            // it doesn't then selecting the tab won't work
            ShowSelectedUI();

            SelectAtom(atom);
            var ui = atom.GetTabSelector();
            if(ui != null)
            {
                ui.SetActiveTab(tabName);
            }
            else
            {
                SuperController.LogError($"{atom.uid} does not have a tab named {tabName}");
            }
        }

        private void SelectNextAtom(bool includeHidden = false)
        {
            var atomList = GetSelectableAtoms(includeHidden: includeHidden);
            var lastIndex = atomList.Count - 1;
            var nextAtomIndex = 0;
            if (atomList.Count == 0) { return; }
            var currentAtom = SuperController.singleton.GetSelectedAtom();
            if (currentAtom == null) { nextAtomIndex = 0; }
            else
            {
                nextAtomIndex = atomList.FindIndex((a) => a.uid == currentAtom?.uid) + 1;
                if (nextAtomIndex > lastIndex) { nextAtomIndex = 0; }
            }
            SelectAtom(atomList[nextAtomIndex]);
        }

        private void SelectPrevAtom(bool includeHidden = false)
        {
            var atomList = GetSelectableAtoms(includeHidden: includeHidden);
            var lastIndex = atomList.Count - 1;
            var prevAtomIndex = lastIndex;
            if (atomList.Count == 0) { return; }
            var currentAtom = SuperController.singleton.GetSelectedAtom();
            if (currentAtom == null) { prevAtomIndex = 0; }
            else
            {
                prevAtomIndex = atomList.FindIndex((a) => a.uid == currentAtom?.uid) - 1;
                if (prevAtomIndex < 0) { prevAtomIndex = lastIndex; }
            }
            SelectAtom(atomList[prevAtomIndex]);
        }

        private void ToggleAtomHidden(Atom a)
        {
            if(a != null) { a.hidden = !a.hidden; }
        }

        private void DeleteAtom(Atom a)
        {
            if(a != null) { SuperController.singleton.RemoveAtom(a); }
        }

        private void ChangeAtomPosition(Atom a, Axis axis, float delta)
        {
            if (a != null)
            {
                var position = a.transform.position;
                switch (axis)
                {
                    case Axis.X:
                        position.x += delta;
                        break;
                    case Axis.Y:
                        position.y += delta;
                        break;
                    case Axis.Z:
                        position.z += delta;
                        break;
                }
                a.transform.position = position;
            }
        }

        private void ChangeAtomRotation(Atom a, Axis axis, float delta)
        {
            if(a != null)
            {
                switch(axis)
                {
                    case Axis.X:
                        a.transform.Rotate(delta, 0, 0);
                        break;
                    case Axis.Y:
                        a.transform.Rotate(0, delta, 0);
                        break;
                    case Axis.Z:
                        a.transform.Rotate(0, 0, delta);
                        break;
                }
            }
        }

        private void ToggleSoftBodyPhysics()
        {
            UserPreferences.singleton.softPhysics = !UserPreferences.singleton.softPhysics;
        }

        private void ToggleErrorLogs()
        {
            if (_errorLogShowing) { SuperController.singleton.CloseErrorLogPanel(); }
            else { SuperController.singleton.OpenErrorLogPanel(); }
            _errorLogShowing = !_errorLogShowing;
        }

        private void ToggleMessageLogs()
        {
            if (_messageLogShowing) { SuperController.singleton.CloseMessageLogPanel(); }
            else { SuperController.singleton.OpenMessageLogPanel(); }
            _messageLogShowing = !_messageLogShowing;
        }

        private void ChangeWorldScale(float aStepMult)
        {
            SuperController sc = SuperController.singleton;
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                aStepMult *= SHIFT_MULTIPLIER;
            }
            SetWorldScale(sc.worldScale + _worldScale.step * aStepMult);


            // Modify player height with scale
            Vector3 dir = Vector3.down;
            dir *= aStepMult * _ws_height_mult;
            sc.navigationRig.position += dir;
        }


        private void SetWorldScale(float aScale)
        {
            if (aScale > _worldScale.max)
            {
                aScale = _worldScale.max;
            }
            else if (aScale < _worldScale.min)
            {
                aScale = _worldScale.min;
            }
            SuperController.singleton.worldScale = aScale;
        }


        private void ChangeTimeScale(float aStepMult)
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                aStepMult *= SHIFT_MULTIPLIER;
            }
            SetTimeScale(TimeControl.singleton.currentScale + _timeScale.step * aStepMult);
        }


        private void SetTimeScale(float aSpeed)
        {
            if (aSpeed > _timeScale.max)
            {
                aSpeed = _timeScale.max;
            }
            else if (aSpeed < _timeScale.min)
            {
                aSpeed = _timeScale.min;
            }
            TimeControl.singleton.currentScale = aSpeed;
        }


        private void ChangeAnimationSpeed(float aStepMult)
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                aStepMult *= SHIFT_MULTIPLIER;
            }
            SetAnimationSpeed(SuperController.singleton.motionAnimationMaster.playbackSpeed + _animationSpeed.step * aStepMult);
        }


        private void SetAnimationSpeed(float aSpeed)
        {
            if (aSpeed > _animationSpeed.max)
            {
                aSpeed = _animationSpeed.max;
            }
            else if (aSpeed < _animationSpeed.min)
            {
                aSpeed = _animationSpeed.min;
            }
            SuperController.singleton.motionAnimationMaster.playbackSpeed = aSpeed;
        }

        private void ChangeMonitorFOV(float delta)
        {
            SetMonitorFOV(SuperController.singleton.monitorCameraFOV + delta);
        }

        private void SetMonitorFOV(float fov)
        {
            var min = 20f;
            var max = 100f;
            if (fov > max)
            {
                fov = max;
            }
            else if (fov < min)
            {
                fov = min;
            }
            SuperController.singleton.monitorCameraFOV = fov;
        }

        private void TogglePause()
        {
            SuperController.singleton.SetFreezeAnimation(!SuperController.singleton.freezeAnimation);
        }
    }
}
