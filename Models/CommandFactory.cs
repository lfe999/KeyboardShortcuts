using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Animations;

using LFE.KeyboardShortcuts.Extensions;

namespace LFE.KeyboardShortcuts.Models
{
    public class CommandFactory {

        private Plugin _plugin;
        public CommandFactory(Plugin plugin)
        {
            _plugin = plugin;
        }

        public IEnumerable<Command> BuildCommands()
        {
            // GENERAL commands
            yield return new AnimationSpeedChange(0.05f) { Name = "Animation Speed > Increase", Group = CommandConst.CAT_GENERAL };
            yield return new AnimationSpeedChange(-0.05f) { Name = "Animation Speed > Decrease", Group = CommandConst.CAT_GENERAL };
            yield return new AtomSelectNext() { Name = "Atom > Select Next", Group = CommandConst.CAT_GENERAL };
            yield return new AtomSelectNext((x) => !x.hidden) { Name = "Atom > Select Next Visible", Group = CommandConst.CAT_GENERAL };
            yield return new AtomSelectPrev() { Name = "Atom > Select Prev", Group = CommandConst.CAT_GENERAL };
            yield return new AtomSelectPrev((x) => !x.hidden) { Name = "Atom > Select Prev Visible", Group = CommandConst.CAT_GENERAL };
            yield return new ErrorLogToggle() { Name = "Error Log > Toggle", Group = CommandConst.CAT_GENERAL };
            yield return new MonitorFieldOfViewChange(10.0f) { Name = "Field Of View > Increase", Group = CommandConst.CAT_GENERAL };
            yield return new MonitorFieldOfViewChange(-10.0f) { Name = "Field Of View > Decrease", Group = CommandConst.CAT_GENERAL };
            yield return new FreezeAnimationToggle() { Name = "Freeze Animation > Toggle", Group = CommandConst.CAT_GENERAL };
            yield return new FreezeAnimationSet(true) { Name = "Freeze Animation > On", Group = CommandConst.CAT_GENERAL };
            yield return new FreezeAnimationSet(false) { Name = "Freeze Animation > Off", Group = CommandConst.CAT_GENERAL };
            yield return new MessageLogToggle() { Name = "Message Log > Toggle", Group = CommandConst.CAT_GENERAL };
            yield return new SoftBodyPhysicsToggle() { Name = "Soft Body Physics > Toggle", Group = CommandConst.CAT_GENERAL };
            yield return new TimeScaleChange(1f) { Name = "Time Scale > Increase", Group = CommandConst.CAT_GENERAL };
            yield return new TimeScaleChange(-1f) { Name = "Time Scale > Decrease", Group = CommandConst.CAT_GENERAL };
            yield return new WorldScaleChange(0.00025f) { Name = "World Scale > Increase", Group = CommandConst.CAT_GENERAL };
            yield return new WorldScaleChange(-0.00025f) { Name = "World Scale > Decrease", Group = CommandConst.CAT_GENERAL };


            // SELECTED ATOM commands
            yield return new SelectedAtomDelete() { Name = "Selected > Delete", Group = CommandConst.CAT_SELECTEDATOM };
            yield return new SelectedAtomHiddenToggle() { Name = "Selected > Hide > Toggle", Group = CommandConst.CAT_SELECTEDATOM };

            yield return new SelectedAtomPositionChange(Axis.X, 0.01f) { Name = "Selected > Position > X > Increase Small", Group = CommandConst.CAT_SELECTEDATOM  };
            yield return new SelectedAtomPositionChange(Axis.X, 0.1f) { Name = "Selected > Position > X > Increase Medium", Group = CommandConst.CAT_SELECTEDATOM  };
            yield return new SelectedAtomPositionChange(Axis.X, 1.0f) { Name = "Selected > Position > X > Increase Large", Group = CommandConst.CAT_SELECTEDATOM  };
            yield return new SelectedAtomPositionChange(Axis.X, -0.01f) { Name = "Selected > Position > X > Decrease Small", Group = CommandConst.CAT_SELECTEDATOM  };
            yield return new SelectedAtomPositionChange(Axis.X, -0.1f) { Name = "Selected > Position > X > Decrease Medium", Group = CommandConst.CAT_SELECTEDATOM  };
            yield return new SelectedAtomPositionChange(Axis.X, -1.0f) { Name = "Selected > Position > X > Decrease Large", Group = CommandConst.CAT_SELECTEDATOM  };
            yield return new SelectedAtomPositionChange(Axis.Y, 0.01f) { Name = "Selected > Position > Y Increase Small", Group = CommandConst.CAT_SELECTEDATOM  };
            yield return new SelectedAtomPositionChange(Axis.Y, 0.1f) { Name = "Selected > Position > Y Increase Medium", Group = CommandConst.CAT_SELECTEDATOM  };
            yield return new SelectedAtomPositionChange(Axis.Y, 1.0f) { Name = "Selected > Position > Y Increase Large", Group = CommandConst.CAT_SELECTEDATOM  };
            yield return new SelectedAtomPositionChange(Axis.Y, -0.01f) { Name = "Selected > Position > Y Decrease Small", Group = CommandConst.CAT_SELECTEDATOM  };
            yield return new SelectedAtomPositionChange(Axis.Y, -0.1f) { Name = "Selected > Position > Y Decrease Medium", Group = CommandConst.CAT_SELECTEDATOM  };
            yield return new SelectedAtomPositionChange(Axis.Y, -1.0f) { Name = "Selected > Position > Y Decrease Large", Group = CommandConst.CAT_SELECTEDATOM  };
            yield return new SelectedAtomPositionChange(Axis.Z, 0.01f) { Name = "Selected > Position > Z Increase Small", Group = CommandConst.CAT_SELECTEDATOM  };
            yield return new SelectedAtomPositionChange(Axis.Z, 0.1f) { Name = "Selected > Position > Z Increase Medium", Group = CommandConst.CAT_SELECTEDATOM  };
            yield return new SelectedAtomPositionChange(Axis.Z, 1.0f) { Name = "Selected > Position > Z Increase Large", Group = CommandConst.CAT_SELECTEDATOM  };
            yield return new SelectedAtomPositionChange(Axis.Z, -0.01f) { Name = "Selected > Position > Z Decrease Small", Group = CommandConst.CAT_SELECTEDATOM  };
            yield return new SelectedAtomPositionChange(Axis.Z, -0.1f) { Name = "Selected > Position > Z Decrease Medium", Group = CommandConst.CAT_SELECTEDATOM  };
            yield return new SelectedAtomPositionChange(Axis.Z, -1.0f) { Name = "Selected > Position > Z Decrease Large", Group = CommandConst.CAT_SELECTEDATOM  };

            yield return new SelectedAtomRotationChange(Axis.X, 0.5f) { Name = "Selected > Rotation > X Increase Small", Group = CommandConst.CAT_SELECTEDATOM  };
            yield return new SelectedAtomRotationChange(Axis.X, 5.0f) { Name = "Selected > Rotation > X Increase Medium", Group = CommandConst.CAT_SELECTEDATOM  };
            yield return new SelectedAtomRotationChange(Axis.X, 45.0f) { Name = "Selected > Rotation > X Increase Large", Group = CommandConst.CAT_SELECTEDATOM  };
            yield return new SelectedAtomRotationChange(Axis.X, -0.5f) { Name = "Selected > Rotation > X Decrease Small", Group = CommandConst.CAT_SELECTEDATOM  };
            yield return new SelectedAtomRotationChange(Axis.X, -5.0f) { Name = "Selected > Rotation > X Decrease Medium", Group = CommandConst.CAT_SELECTEDATOM  };
            yield return new SelectedAtomRotationChange(Axis.X, -45.0f) { Name = "Selected > Rotation > X Decrease Large", Group = CommandConst.CAT_SELECTEDATOM  };
            yield return new SelectedAtomRotationChange(Axis.Y, 0.5f) { Name = "Selected > Rotation > Y Increase Small", Group = CommandConst.CAT_SELECTEDATOM  };
            yield return new SelectedAtomRotationChange(Axis.Y, 5.0f) { Name = "Selected > Rotation > Y Increase Medium", Group = CommandConst.CAT_SELECTEDATOM  };
            yield return new SelectedAtomRotationChange(Axis.Y, 45.0f) { Name = "Selected > Rotation > Y Increase Large", Group = CommandConst.CAT_SELECTEDATOM  };
            yield return new SelectedAtomRotationChange(Axis.Y, -0.5f) { Name = "Selected > Rotation > Y Decrease Small", Group = CommandConst.CAT_SELECTEDATOM  };
            yield return new SelectedAtomRotationChange(Axis.Y, -5.0f) { Name = "Selected > Rotation > Y Decrease Medium", Group = CommandConst.CAT_SELECTEDATOM  };
            yield return new SelectedAtomRotationChange(Axis.Y, -45.0f) { Name = "Selected > Rotation > Y Decrease Large", Group = CommandConst.CAT_SELECTEDATOM  };
            yield return new SelectedAtomRotationChange(Axis.Z, 0.5f) { Name = "Selected > Rotation > Z Increase Small", Group = CommandConst.CAT_SELECTEDATOM  };
            yield return new SelectedAtomRotationChange(Axis.Z, 5.0f) { Name = "Selected > Rotation > Z Increase Medium", Group = CommandConst.CAT_SELECTEDATOM  };
            yield return new SelectedAtomRotationChange(Axis.Z, 45.0f) { Name = "Selected > Rotation > Z Increase Large", Group = CommandConst.CAT_SELECTEDATOM  };
            yield return new SelectedAtomRotationChange(Axis.Z, -0.5f) { Name = "Selected > Rotation > Z Decrease Small", Group = CommandConst.CAT_SELECTEDATOM  };
            yield return new SelectedAtomRotationChange(Axis.Z, -5.0f) { Name = "Selected > Rotation > Z Decrease Medium", Group = CommandConst.CAT_SELECTEDATOM  };
            yield return new SelectedAtomRotationChange(Axis.Z, -45.0f) { Name = "Selected > Rotation > Z Decrease Large", Group = CommandConst.CAT_SELECTEDATOM  };

            yield return new SelectedAtomSelectTab("Animation Pattern") { Name = "Selected > ShowUI > Animation Pattern", Group = CommandConst.CAT_SELECTEDATOM };
            yield return new SelectedAtomSelectTab("Animation Trigger") { Name = "Selected > ShowUI > Animation Trigger", Group = CommandConst.CAT_SELECTEDATOM };
            yield return new SelectedAtomSelectTab("Animation") { Name = "Selected > ShowUI > Animation", Group = CommandConst.CAT_SELECTEDATOM };
            yield return new SelectedAtomSelectTab("Appearance Presets") { Name = "Selected > ShowUI > Appearance Presets", Group = CommandConst.CAT_SELECTEDATOM };
            yield return new SelectedAtomSelectTab("Auto Behaviours") { Name = "Selected > ShowUI > Auto Behaviours", Group = CommandConst.CAT_SELECTEDATOM };
            yield return new SelectedAtomSelectTab("Camera") { Name = "Selected > ShowUI > Camera", Group = CommandConst.CAT_SELECTEDATOM };
            yield return new SelectedAtomSelectTab("Clothing Presets") { Name = "Selected > ShowUI > Clothing Presets", Group = CommandConst.CAT_SELECTEDATOM };
            yield return new SelectedAtomSelectTab("Clothing") { Name = "Selected > ShowUI > Clothing", Group = CommandConst.CAT_SELECTEDATOM };
            yield return new SelectedAtomSelectTab("Collision Trigger") { Name = "Selected > ShowUI > Collision Trigger", Group = CommandConst.CAT_SELECTEDATOM };
            yield return new SelectedAtomSelectTab("Collision Trigger") { Name = "Selected > ShowUI > Collision Trigger", Group = CommandConst.CAT_SELECTEDATOM };
            yield return new SelectedAtomSelectTab("Collision Triggers") { Name = "Selected > ShowUI > Collision Triggers", Group = CommandConst.CAT_SELECTEDATOM };
            yield return new SelectedAtomSelectTab("Control") { Name = "Selected > ShowUI > Control", Group = CommandConst.CAT_SELECTEDATOM };
            yield return new SelectedAtomSelectTab("F Breast Physics 1") { Name = "Selected > ShowUI > F Breast Physics 1", Group = CommandConst.CAT_SELECTEDATOM };
            yield return new SelectedAtomSelectTab("F Breast Physics 2") { Name = "Selected > ShowUI > F Breast Physics 2", Group = CommandConst.CAT_SELECTEDATOM };
            yield return new SelectedAtomSelectTab("F Breast Presets") { Name = "Selected > ShowUI > F Breast Presets", Group = CommandConst.CAT_SELECTEDATOM };
            yield return new SelectedAtomSelectTab("F Eyelash Materials") { Name = "Selected > ShowUI > F Eyelash Materials", Group = CommandConst.CAT_SELECTEDATOM };
            yield return new SelectedAtomSelectTab("F Glute Physics") { Name = "Selected > ShowUI > F Glute Physics", Group = CommandConst.CAT_SELECTEDATOM };
            yield return new SelectedAtomSelectTab("F Glute Presets") { Name = "Selected > ShowUI > F Glute Presets", Group = CommandConst.CAT_SELECTEDATOM };
            yield return new SelectedAtomSelectTab("Female Morphs") { Name = "Selected > ShowUI > Female Morphs", Group = CommandConst.CAT_SELECTEDATOM };
            yield return new SelectedAtomSelectTab("General Presets") { Name = "Selected > ShowUI > General Presets", Group = CommandConst.CAT_SELECTEDATOM };
            yield return new SelectedAtomSelectTab("Hair Presets") { Name = "Selected > ShowUI > Hair Presets", Group = CommandConst.CAT_SELECTEDATOM };
            yield return new SelectedAtomSelectTab("Hair") { Name = "Selected > ShowUI > Hair", Group = CommandConst.CAT_SELECTEDATOM };
            yield return new SelectedAtomSelectTab("Hand Control") { Name = "Selected > ShowUI > Hand Control", Group = CommandConst.CAT_SELECTEDATOM };
            yield return new SelectedAtomSelectTab("Head Audio") { Name = "Selected > ShowUI > Head Audio", Group = CommandConst.CAT_SELECTEDATOM };
            yield return new SelectedAtomSelectTab("Iris Materials") { Name = "Selected > ShowUI > Iris Materials", Group = CommandConst.CAT_SELECTEDATOM };
            yield return new SelectedAtomSelectTab("Lacrimals Materials") { Name = "Selected > ShowUI > Lacrimals Materials", Group = CommandConst.CAT_SELECTEDATOM };
            yield return new SelectedAtomSelectTab("Light") { Name = "Selected > ShowUI > Light", Group = CommandConst.CAT_SELECTEDATOM };
            yield return new SelectedAtomSelectTab("M Eyelash Materials") { Name = "Selected > ShowUI > M Eyelash Materials", Group = CommandConst.CAT_SELECTEDATOM };
            yield return new SelectedAtomSelectTab("M Pectoral Physics") { Name = "Selected > ShowUI > M Pectoral Physics", Group = CommandConst.CAT_SELECTEDATOM };
            yield return new SelectedAtomSelectTab("Male Morphs") { Name = "Selected > ShowUI > Male Morphs", Group = CommandConst.CAT_SELECTEDATOM };
            yield return new SelectedAtomSelectTab("Misc Physics") { Name = "Selected > ShowUI > Misc Physics", Group = CommandConst.CAT_SELECTEDATOM };
            yield return new SelectedAtomSelectTab("Morphs Presets") { Name = "Selected > ShowUI > Morphs Presets", Group = CommandConst.CAT_SELECTEDATOM };
            yield return new SelectedAtomSelectTab("Mouth Materials") { Name = "Selected > ShowUI > Mouth Materials", Group = CommandConst.CAT_SELECTEDATOM };
            yield return new SelectedAtomSelectTab("Move") { Name = "Selected > ShowUI > Move", Group = CommandConst.CAT_SELECTEDATOM };
            yield return new SelectedAtomSelectTab("Move") { Name = "Selected > ShowUI > Move", Group = CommandConst.CAT_SELECTEDATOM };
            yield return new SelectedAtomSelectTab("Physics Control") { Name = "Selected > ShowUI > Physics Control", Group = CommandConst.CAT_SELECTEDATOM };
            yield return new SelectedAtomSelectTab("Physics Object") { Name = "Selected > ShowUI > Physics Object", Group = CommandConst.CAT_SELECTEDATOM };
            yield return new SelectedAtomSelectTab("Plugins") { Name = "Selected > ShowUI > Plugins", Group = CommandConst.CAT_SELECTEDATOM };
            yield return new SelectedAtomSelectTab("Pose Presets") { Name = "Selected > ShowUI > Pose Presets", Group = CommandConst.CAT_SELECTEDATOM };
            yield return new SelectedAtomSelectTab("Preset") { Name = "Selected > ShowUI > Preset", Group = CommandConst.CAT_SELECTEDATOM };
            yield return new SelectedAtomSelectTab("Sclera Materials") { Name = "Selected > ShowUI > Sclera Materials", Group = CommandConst.CAT_SELECTEDATOM };
            yield return new SelectedAtomSelectTab("Skin Materials 1") { Name = "Selected > ShowUI > Skin Materials 1", Group = CommandConst.CAT_SELECTEDATOM };
            yield return new SelectedAtomSelectTab("Skin Materials 2") { Name = "Selected > ShowUI > Skin Materials 2", Group = CommandConst.CAT_SELECTEDATOM };
            yield return new SelectedAtomSelectTab("Skin Presets") { Name = "Selected > ShowUI > Skin Presets", Group = CommandConst.CAT_SELECTEDATOM };
            yield return new SelectedAtomSelectTab("Skin Select") { Name = "Selected > ShowUI > Skin Select", Group = CommandConst.CAT_SELECTEDATOM };
            yield return new SelectedAtomSelectTab("Skin Textures") { Name = "Selected > ShowUI > Skin Textures", Group = CommandConst.CAT_SELECTEDATOM };
            yield return new SelectedAtomSelectTab("Teeth Materials") { Name = "Selected > ShowUI > Teeth Materials", Group = CommandConst.CAT_SELECTEDATOM };
            yield return new SelectedAtomSelectTab("Tongue Materials") { Name = "Selected > ShowUI > Tongue Materials", Group = CommandConst.CAT_SELECTEDATOM };

            // PLUGIN BY NAME (assuming only the first plugin by that type in the scene)
            var pluginsByShortName = SuperController.singleton.GetAllPlugins()
                .GroupBy((p) => p.GetShortName())
                .Select((g) => g.First())
                .Where((p) => !p.GetShortName().Equals(_plugin.GetShortName()));
            foreach(var plugin in pluginsByShortName)
            {
                var shortName = plugin.GetShortName();
                var group = $"{shortName}";

                // booleans
                foreach (var param in plugin.GetBoolParamNames().Where((param) => !param.Equals("enabled")))
                {
                    yield return new PluginBoolSet(plugin, param, true) { Name = $"{shortName} > {param} > On", DisplayName = $"{param} > On", Group = group };
                    yield return new PluginBoolSet(plugin, param, false) { Name = $"{shortName} > {param} > Off", DisplayName = $"{param} > Off", Group = group };
                    yield return new PluginBoolToggle(plugin, param) { Name = $"{shortName} > {param} > Toggle", DisplayName = $"{param} > Toggle", Group = group };
                }
                // actions
                foreach (var param in plugin.GetCustomActionNames())
                {
                    yield return new PluginActionCall(plugin, param) { Name = $"{shortName} > {param} > Call", DisplayName = $"{param} > Call", Group = group };
                }
            }
        }

        private readonly Dictionary<string, string> _defaultForAction = new Dictionary<string, string>
        {
            { "Atom > Select Next Visible", "Alt-RightArrow" },
            { "Atom > Select Prev Visible", "Alt-LeftArrow" },
            { "Atom > Select Next", "Control-Alt-RightArrow" },
            { "Atom > Select Prev", "Control-Alt-LeftArrow" },
            { "Error Log > Toggle", "Control-BackQuote" },
            { "Field Of View > Decrease", "Shift-Minus" },
            { "Field Of View > Increase", "Shift-Equals" },
            { "Time Scale > Increase", "Control-UpArrow" },
            { "Time Scale > Decrease", "Control-DownArrow" },
            { "Freeze Animation > Toggle", "Space" },
            { "Message Log > Toggle", "BackQuote" }
        };

        public KeyChord GetDefaultKeyChordByActionName(string name)
        {
            return _defaultForAction.ContainsKey(name) ? new KeyChord(_defaultForAction[name]) : null;
        }
    }
}
