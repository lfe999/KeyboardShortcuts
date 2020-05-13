using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LFE.KeyboardShortcuts.Extensions
{
    public static class InputWrapper
    {
        // Note: The list of axis names here did not work http://wiki.unity3d.com/index.php?title=Xbox360Controller
        // Instead, I just looked at the binary data in VaM to find hints on the supported axis names
        // using the linux command "strings"
        // strings VaM_Data/ globalgamemanagers | egrep - i "axis|trigger|joystick|dpad|mouse"
        public static readonly IList<string> AxisNames = new List<string> { "Mouse X", "Mouse Y", "Mouse ScrollWheel", "LeftStickX", "LeftStickY", "RightStickX", "RightStickY", "Triggers", "DPadX", "DPadY", "Axis6", "Axis7", "Axis8" }.AsReadOnly();

        public static IList<KeyCode> KeyCodes = Enum.GetValues(typeof(KeyCode)).Cast<KeyCode>().ToList().AsReadOnly();

        public static float GetAxis(string axisName)
        {
            return Input.GetAxis(axisName);
        }
        public static bool AnyAxis()
        {
            return AxisNames.Any((n) => GetAxis(n) != 0);
        }

        public static bool GetKey(KeyCode key)
        {
            return Input.GetKey(key);
        }

        public static bool GetKeyDown(KeyCode key)
        {
            return Input.GetKeyDown(key);
        }

        public static bool AnyKey()
        {
            return Input.anyKey;
        }

        public static bool AnyKeyOrAxis()
        {
            return AnyKey() || AnyAxis();
        }
    }
}
