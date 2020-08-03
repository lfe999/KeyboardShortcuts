using System;
using System.Collections.Generic;
using System.Linq;
using LFE.KeyboardShortcuts.Commands;
using UnityEngine;
using UnityEngine.Animations;

using LFE.KeyboardShortcuts.Extensions;
using LFE.KeyboardShortcuts.Main;

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
            yield return new CameraPositionChange(Axis.X, 0.75f) { Name = "Camera > Move > Right", Group = CommandConst.CAT_GENERAL };
            yield return new CameraPositionChange(Axis.X, -0.75f) { Name = "Camera > Move > Left", Group = CommandConst.CAT_GENERAL };
            yield return new CameraPositionChange(Axis.Z, 0.75f) { Name = "Camera > Move > Forward", Group = CommandConst.CAT_GENERAL };
            yield return new CameraPositionChange(Axis.Z, -0.75f) { Name = "Camera > Move > Backward", Group = CommandConst.CAT_GENERAL };
            yield return new CameraPositionChange(Axis.Y, 0.50f) { Name = "Camera > Move > Up", Group = CommandConst.CAT_GENERAL };
            yield return new CameraPositionChange(Axis.Y, -0.50f) { Name = "Camera > Move > Down", Group = CommandConst.CAT_GENERAL };

            yield return new CameraRotationChange(Axis.X, 0.25f) { Name = "Camera > Look > Up", Group = CommandConst.CAT_GENERAL };
            yield return new CameraRotationChange(Axis.X, -0.25f) { Name = "Camera > Look > Down", Group = CommandConst.CAT_GENERAL };
            yield return new CameraRotationChange(Axis.Y, 0.25f) { Name = "Camera > Look > Left", Group = CommandConst.CAT_GENERAL };
            yield return new CameraRotationChange(Axis.Y, -0.25f) { Name = "Camera > Look > Right", Group = CommandConst.CAT_GENERAL };
            yield return new CameraRotationChange(Axis.Z, 0.25f) { Name = "Camera > Look > Tilt Left", Group = CommandConst.CAT_GENERAL };
            yield return new CameraRotationChange(Axis.Z, -0.25f) { Name = "Camera > Look > Tilt Right", Group = CommandConst.CAT_GENERAL };

            yield return new CameraRotationAroundChange(Axis.X, 0.25f) { Name = "Camera > Rotate Around > X Increase", Group = CommandConst.CAT_GENERAL };
            yield return new CameraRotationAroundChange(Axis.X, -0.25f) { Name = "Camera > Rotate Around > X Decrease", Group = CommandConst.CAT_GENERAL };
            yield return new CameraRotationAroundChange(Axis.Y, 0.25f) { Name = "Camera > Rotate Around > Y Increase", Group = CommandConst.CAT_GENERAL };
            yield return new CameraRotationAroundChange(Axis.Y, -0.25f) { Name = "Camera > Rotate Around > Y Decrease", Group = CommandConst.CAT_GENERAL };

            yield return new CameraRotationAroundChange(Axis.X, 0.10f) { Name = "Camera > Rotate Around Slower > X Increase", Group = CommandConst.CAT_GENERAL };
            yield return new CameraRotationAroundChange(Axis.X, -0.10f) { Name = "Camera > Rotate Around Slower > X Decrease", Group = CommandConst.CAT_GENERAL };
            yield return new CameraRotationAroundChange(Axis.Y, 0.10f) { Name = "Camera > Rotate Around Slower > Y Increase", Group = CommandConst.CAT_GENERAL };
            yield return new CameraRotationAroundChange(Axis.Y, -0.10f) { Name = "Camera > Rotate Around Slower > Y Decrease", Group = CommandConst.CAT_GENERAL };

            yield return new PlayEditModeSet(SuperController.GameMode.Play) { Name = "Play/Edit > Set To Play", Group = CommandConst.CAT_GENERAL };
            yield return new PlayEditModeSet(SuperController.GameMode.Edit) { Name = "Play/Edit > Set To Edit", Group = CommandConst.CAT_GENERAL };
            yield return new PlayEditModeToggle() { Name = "Play/Edit > Toggle", Group = CommandConst.CAT_GENERAL };
            yield return new AnimationSpeedChange(0.05f) { Name = "Animation Speed > Increase", Group = CommandConst.CAT_GENERAL };
            yield return new AnimationSpeedChange(-0.05f) { Name = "Animation Speed > Decrease", Group = CommandConst.CAT_GENERAL };
            yield return new AtomSelectNext() { Name = "Atom > Select Next", Group = CommandConst.CAT_GENERAL };
            yield return new AtomSelectNext((x) => !x.hidden) { Name = "Atom > Select Next Visible", Group = CommandConst.CAT_GENERAL };
            yield return new AtomSelectPrev() { Name = "Atom > Select Prev", Group = CommandConst.CAT_GENERAL };
            yield return new AtomSelectPrev((x) => !x.hidden) { Name = "Atom > Select Prev Visible", Group = CommandConst.CAT_GENERAL };
            yield return new ErrorLogToggle() { Name = "Error Log > Toggle", Group = CommandConst.CAT_GENERAL };
            yield return new MonitorFieldOfViewChange(1.0f) { Name = "Field Of View > Increase Small", Group = CommandConst.CAT_GENERAL };
            yield return new MonitorFieldOfViewChange(-1.0f) { Name = "Field Of View > Decrease Small", Group = CommandConst.CAT_GENERAL };
            yield return new MonitorFieldOfViewChange(10.0f) { Name = "Field Of View > Increase", Group = CommandConst.CAT_GENERAL };
            yield return new MonitorFieldOfViewChange(-10.0f) { Name = "Field Of View > Decrease", Group = CommandConst.CAT_GENERAL };
            yield return new FreezeAnimationToggle() { Name = "Freeze Animation > Toggle", Group = CommandConst.CAT_GENERAL };
            yield return new FreezeAnimationSet(true) { Name = "Freeze Animation > On", Group = CommandConst.CAT_GENERAL };
            yield return new FreezeAnimationSet(false) { Name = "Freeze Animation > Off", Group = CommandConst.CAT_GENERAL };
            yield return new MessageLogToggle() { Name = "Message Log > Toggle", Group = CommandConst.CAT_GENERAL };
            yield return new MirrorReflectionsToggle() { Name = "Mirror Reflections > Toggle", Group = CommandConst.CAT_GENERAL };
            yield return new MsaaChange(1) { Name = "MSAA Level > Increase", Group = CommandConst.CAT_GENERAL };
            yield return new MsaaChange(-1) { Name = "MSAA Level > Decrease", Group = CommandConst.CAT_GENERAL };
            yield return new PerformanceMonitorToggle() { Name = "Performance Monitor > Toggle", Group = CommandConst.CAT_GENERAL };
            yield return new PixelLightCountChange(1) { Name = "Pixel Light Count > Increase", Group = CommandConst.CAT_GENERAL };
            yield return new PixelLightCountChange(-1) { Name = "Pixel Light Count > Decrease", Group = CommandConst.CAT_GENERAL };
            yield return new RescanPackages() { Name = "Rescan Add-on Packages", Group = CommandConst.CAT_GENERAL };
            yield return new HardReset() { Name = "Hard Reset", Group = CommandConst.CAT_GENERAL };
            yield return new SceneNew() { Name = "Scene > New Scene", Group = CommandConst.CAT_GENERAL };
            yield return new SceneLoad() { Name = "Scene > Open Scene", Group = CommandConst.CAT_GENERAL };
            yield return new SceneSave() { Name = "Scene > Save Scene", Group = CommandConst.CAT_GENERAL };
            yield return new ScreenShotModeOn() { Name = "Screen Shot > Mode > Enable", Group = CommandConst.CAT_GENERAL };
            yield return new SoftBodyPhysicsToggle() { Name = "Soft Body Physics > Toggle", Group = CommandConst.CAT_GENERAL };
            yield return new TimeScaleChange(0.1f) { Name = "Time Scale > Increase", Group = CommandConst.CAT_GENERAL };
            yield return new TimeScaleChange(-0.1f) { Name = "Time Scale > Decrease", Group = CommandConst.CAT_GENERAL };
            yield return new WorldScaleChange(0.00025f) { Name = "World Scale > Increase", Group = CommandConst.CAT_GENERAL };
            yield return new WorldScaleChange(-0.00025f) { Name = "World Scale > Decrease", Group = CommandConst.CAT_GENERAL };

            // .. adding atoms by type
            foreach(var category in SuperController.singleton.GetAtomCategories())
            {
                foreach(var type in SuperController.singleton.GetAtomTypesByCategory(category))
                {
                    yield return new AtomAdd(type) { Name = $"Add > {category} > {type}", Group = CommandConst.CAT_GENERAL };
                }
            }

            // SELECTED FREE CONTROLLER command
            yield return new ControllerPositionSetLerp(Axis.X, 0, 1) { Name = $"Selected FC > Position > X > Interpolate 0 - 1", Group = CommandConst.CAT_SELECTED_CONTROLLER };
            yield return new ControllerPositionSetLerp(Axis.Y, 0, 1) { Name = $"Selected FC > Position > Y > Interpolate 0 - 1", Group = CommandConst.CAT_SELECTED_CONTROLLER };
            yield return new ControllerPositionSetLerp(Axis.Z, 0, 1) { Name = $"Selected FC > Position > Z > Interpolate 0 - 1", Group = CommandConst.CAT_SELECTED_CONTROLLER };

            yield return new ControllerPositionChange(Axis.X, 0.5f) { Name = "Selected FC > Position > X > Increase Small", Group = CommandConst.CAT_SELECTED_CONTROLLER  };
            yield return new ControllerPositionChange(Axis.X, -0.5f) { Name = "Selected FC > Position > X > Decrease Small", Group = CommandConst.CAT_SELECTED_CONTROLLER  };
            yield return new ControllerPositionChange(Axis.X, 2f) { Name = "Selected FC > Position > X > Increase Medium", Group = CommandConst.CAT_SELECTED_CONTROLLER  };
            yield return new ControllerPositionChange(Axis.X, -2f) { Name = "Selected FC > Position > X > Decrease Medium", Group = CommandConst.CAT_SELECTED_CONTROLLER  };
            yield return new ControllerPositionChange(Axis.X, 5.0f) { Name = "Selected FC > Position > X > Increase Large", Group = CommandConst.CAT_SELECTED_CONTROLLER  };
            yield return new ControllerPositionChange(Axis.X, -5.0f) { Name = "Selected FC > Position > X > Decrease Large", Group = CommandConst.CAT_SELECTED_CONTROLLER  };

            yield return new ControllerPositionChange(Axis.Y, 0.5f) { Name = "Selected FC > Position > Y Increase Small", Group = CommandConst.CAT_SELECTED_CONTROLLER  };
            yield return new ControllerPositionChange(Axis.Y, -0.5f) { Name = "Selected FC > Position > Y Decrease Small", Group = CommandConst.CAT_SELECTED_CONTROLLER  };
            yield return new ControllerPositionChange(Axis.Y, 2f) { Name = "Selected FC > Position > Y Increase Medium", Group = CommandConst.CAT_SELECTED_CONTROLLER  };
            yield return new ControllerPositionChange(Axis.Y, -2f) { Name = "Selected FC > Position > Y Decrease Medium", Group = CommandConst.CAT_SELECTED_CONTROLLER  };
            yield return new ControllerPositionChange(Axis.Y, 5.0f) { Name = "Selected FC > Position > Y Increase Large", Group = CommandConst.CAT_SELECTED_CONTROLLER  };
            yield return new ControllerPositionChange(Axis.Y, -5.0f) { Name = "Selected FC > Position > Y Decrease Large", Group = CommandConst.CAT_SELECTED_CONTROLLER  };

            yield return new ControllerPositionChange(Axis.Z, 0.5f) { Name = "Selected FC > Position > Z Increase Small", Group = CommandConst.CAT_SELECTED_CONTROLLER  };
            yield return new ControllerPositionChange(Axis.Z, -0.5f) { Name = "Selected FC > Position > Z Decrease Small", Group = CommandConst.CAT_SELECTED_CONTROLLER  };
            yield return new ControllerPositionChange(Axis.Z, 2f) { Name = "Selected FC > Position > Z Increase Medium", Group = CommandConst.CAT_SELECTED_CONTROLLER  };
            yield return new ControllerPositionChange(Axis.Z, -2f) { Name = "Selected FC > Position > Z Decrease Medium", Group = CommandConst.CAT_SELECTED_CONTROLLER  };
            yield return new ControllerPositionChange(Axis.Z, 5.0f) { Name = "Selected FC > Position > Z Increase Large", Group = CommandConst.CAT_SELECTED_CONTROLLER  };
            yield return new ControllerPositionChange(Axis.Z, -5.0f) { Name = "Selected FC > Position > Z Decrease Large", Group = CommandConst.CAT_SELECTED_CONTROLLER  };

            yield return new ControllerRotationChange(Axis.X, 0.25f) { Name = "Selected FC > Rotation > X Increase Small", Group = CommandConst.CAT_SELECTED_CONTROLLER  };
            yield return new ControllerRotationChange(Axis.X, -0.25f) { Name = "Selected FC > Rotation > X Decrease Small", Group = CommandConst.CAT_SELECTED_CONTROLLER  };
            yield return new ControllerRotationChange(Axis.X, 0.5f) { Name = "Selected FC > Rotation > X Increase Medium", Group = CommandConst.CAT_SELECTED_CONTROLLER  };
            yield return new ControllerRotationChange(Axis.X, -0.5f) { Name = "Selected FC > Rotation > X Decrease Medium", Group = CommandConst.CAT_SELECTED_CONTROLLER  };
            yield return new ControllerRotationChange(Axis.X, 2.0f) { Name = "Selected FC > Rotation > X Increase Large", Group = CommandConst.CAT_SELECTED_CONTROLLER  };
            yield return new ControllerRotationChange(Axis.X, -2.0f) { Name = "Selected FC > Rotation > X Decrease Large", Group = CommandConst.CAT_SELECTED_CONTROLLER  };

            yield return new ControllerRotationChange(Axis.Y, 0.25f) { Name = "Selected FC > Rotation > Y Increase Small", Group = CommandConst.CAT_SELECTED_CONTROLLER  };
            yield return new ControllerRotationChange(Axis.Y, -0.25f) { Name = "Selected FC > Rotation > Y Decrease Small", Group = CommandConst.CAT_SELECTED_CONTROLLER  };
            yield return new ControllerRotationChange(Axis.Y, 0.5f) { Name = "Selected FC > Rotation > Y Increase Medium", Group = CommandConst.CAT_SELECTED_CONTROLLER  };
            yield return new ControllerRotationChange(Axis.Y, -0.5f) { Name = "Selected FC > Rotation > Y Decrease Medium", Group = CommandConst.CAT_SELECTED_CONTROLLER  };
            yield return new ControllerRotationChange(Axis.Y, 2.0f) { Name = "Selected FC > Rotation > Y Increase Large", Group = CommandConst.CAT_SELECTED_CONTROLLER  };
            yield return new ControllerRotationChange(Axis.Y, -2.0f) { Name = "Selected FC > Rotation > Y Decrease Large", Group = CommandConst.CAT_SELECTED_CONTROLLER  };

            yield return new ControllerRotationChange(Axis.Z, 0.25f) { Name = "Selected FC > Rotation > Z Increase Small", Group = CommandConst.CAT_SELECTED_CONTROLLER  };
            yield return new ControllerRotationChange(Axis.Z, -0.25f) { Name = "Selected FC > Rotation > Z Decrease Small", Group = CommandConst.CAT_SELECTED_CONTROLLER  };
            yield return new ControllerRotationChange(Axis.Z, 0.5f) { Name = "Selected FC > Rotation > Z Increase Medium", Group = CommandConst.CAT_SELECTED_CONTROLLER  };
            yield return new ControllerRotationChange(Axis.Z, -0.5f) { Name = "Selected FC > Rotation > Z Decrease Medium", Group = CommandConst.CAT_SELECTED_CONTROLLER  };
            yield return new ControllerRotationChange(Axis.Z, 2.0f) { Name = "Selected FC > Rotation > Z Increase Large", Group = CommandConst.CAT_SELECTED_CONTROLLER  };
            yield return new ControllerRotationChange(Axis.Z, -2.0f) { Name = "Selected FC > Rotation > Z Decrease Large", Group = CommandConst.CAT_SELECTED_CONTROLLER  };

            // SELECTED ATOM commands
            yield return new AtomDelete() { Name = "Selected > Delete", Group = CommandConst.CAT_SELECTEDATOM };
            yield return new AtomHiddenToggle() { Name = "Selected > Hide > Toggle", Group = CommandConst.CAT_SELECTEDATOM };
            yield return new AtomDump() { Name = "Selected > Print Object Structure", Group = CommandConst.CAT_SELECTEDATOM };

            yield return new AtomPositionSetLerp(Axis.X, 0, 1) { Name = $"Selected > Position > X > Interpolate 0 - 1", Group = CommandConst.CAT_SELECTEDATOM };
            yield return new AtomPositionSetLerp(Axis.Y, 0, 1) { Name = $"Selected > Position > Y > Interpolate 0 - 1", Group = CommandConst.CAT_SELECTEDATOM };
            yield return new AtomPositionSetLerp(Axis.Z, 0, 1) { Name = $"Selected > Position > Z > Interpolate 0 - 1", Group = CommandConst.CAT_SELECTEDATOM };

            yield return new AtomPositionChange(Axis.X, 0.5f) { Name = "Selected > Position > X > Increase Small", Group = CommandConst.CAT_SELECTEDATOM  };
            yield return new AtomPositionChange(Axis.X, -0.5f) { Name = "Selected > Position > X > Decrease Small", Group = CommandConst.CAT_SELECTEDATOM  };
            yield return new AtomPositionChange(Axis.X, 2f) { Name = "Selected > Position > X > Increase Medium", Group = CommandConst.CAT_SELECTEDATOM  };
            yield return new AtomPositionChange(Axis.X, -2f) { Name = "Selected > Position > X > Decrease Medium", Group = CommandConst.CAT_SELECTEDATOM  };
            yield return new AtomPositionChange(Axis.X, 5.0f) { Name = "Selected > Position > X > Increase Large", Group = CommandConst.CAT_SELECTEDATOM  };
            yield return new AtomPositionChange(Axis.X, -5.0f) { Name = "Selected > Position > X > Decrease Large", Group = CommandConst.CAT_SELECTEDATOM  };

            yield return new AtomPositionChange(Axis.Y, 0.5f) { Name = "Selected > Position > Y Increase Small", Group = CommandConst.CAT_SELECTEDATOM  };
            yield return new AtomPositionChange(Axis.Y, -0.5f) { Name = "Selected > Position > Y Decrease Small", Group = CommandConst.CAT_SELECTEDATOM  };
            yield return new AtomPositionChange(Axis.Y, 2f) { Name = "Selected > Position > Y Increase Medium", Group = CommandConst.CAT_SELECTEDATOM  };
            yield return new AtomPositionChange(Axis.Y, -2f) { Name = "Selected > Position > Y Decrease Medium", Group = CommandConst.CAT_SELECTEDATOM  };
            yield return new AtomPositionChange(Axis.Y, 5.0f) { Name = "Selected > Position > Y Increase Large", Group = CommandConst.CAT_SELECTEDATOM  };
            yield return new AtomPositionChange(Axis.Y, -5.0f) { Name = "Selected > Position > Y Decrease Large", Group = CommandConst.CAT_SELECTEDATOM  };

            yield return new AtomPositionChange(Axis.Z, 0.5f) { Name = "Selected > Position > Z Increase Small", Group = CommandConst.CAT_SELECTEDATOM  };
            yield return new AtomPositionChange(Axis.Z, -0.5f) { Name = "Selected > Position > Z Decrease Small", Group = CommandConst.CAT_SELECTEDATOM  };
            yield return new AtomPositionChange(Axis.Z, 2f) { Name = "Selected > Position > Z Increase Medium", Group = CommandConst.CAT_SELECTEDATOM  };
            yield return new AtomPositionChange(Axis.Z, -2f) { Name = "Selected > Position > Z Decrease Medium", Group = CommandConst.CAT_SELECTEDATOM  };
            yield return new AtomPositionChange(Axis.Z, 5.0f) { Name = "Selected > Position > Z Increase Large", Group = CommandConst.CAT_SELECTEDATOM  };
            yield return new AtomPositionChange(Axis.Z, -5.0f) { Name = "Selected > Position > Z Decrease Large", Group = CommandConst.CAT_SELECTEDATOM  };

            yield return new AtomRotationChange(Axis.X, 0.25f) { Name = "Selected > Rotation > X Increase Small", Group = CommandConst.CAT_SELECTEDATOM  };
            yield return new AtomRotationChange(Axis.X, -0.25f) { Name = "Selected > Rotation > X Decrease Small", Group = CommandConst.CAT_SELECTEDATOM  };
            yield return new AtomRotationChange(Axis.X, 0.5f) { Name = "Selected > Rotation > X Increase Medium", Group = CommandConst.CAT_SELECTEDATOM  };
            yield return new AtomRotationChange(Axis.X, -0.5f) { Name = "Selected > Rotation > X Decrease Medium", Group = CommandConst.CAT_SELECTEDATOM  };
            yield return new AtomRotationChange(Axis.X, 2.0f) { Name = "Selected > Rotation > X Increase Large", Group = CommandConst.CAT_SELECTEDATOM  };
            yield return new AtomRotationChange(Axis.X, -2.0f) { Name = "Selected > Rotation > X Decrease Large", Group = CommandConst.CAT_SELECTEDATOM  };

            yield return new AtomRotationChange(Axis.Y, 0.25f) { Name = "Selected > Rotation > Y Increase Small", Group = CommandConst.CAT_SELECTEDATOM  };
            yield return new AtomRotationChange(Axis.Y, -0.25f) { Name = "Selected > Rotation > Y Decrease Small", Group = CommandConst.CAT_SELECTEDATOM  };
            yield return new AtomRotationChange(Axis.Y, 0.5f) { Name = "Selected > Rotation > Y Increase Medium", Group = CommandConst.CAT_SELECTEDATOM  };
            yield return new AtomRotationChange(Axis.Y, -0.5f) { Name = "Selected > Rotation > Y Decrease Medium", Group = CommandConst.CAT_SELECTEDATOM  };
            yield return new AtomRotationChange(Axis.Y, 2.0f) { Name = "Selected > Rotation > Y Increase Large", Group = CommandConst.CAT_SELECTEDATOM  };
            yield return new AtomRotationChange(Axis.Y, -2.0f) { Name = "Selected > Rotation > Y Decrease Large", Group = CommandConst.CAT_SELECTEDATOM  };

            yield return new AtomRotationChange(Axis.Z, 0.25f) { Name = "Selected > Rotation > Z Increase Small", Group = CommandConst.CAT_SELECTEDATOM  };
            yield return new AtomRotationChange(Axis.Z, -0.25f) { Name = "Selected > Rotation > Z Decrease Small", Group = CommandConst.CAT_SELECTEDATOM  };
            yield return new AtomRotationChange(Axis.Z, 0.5f) { Name = "Selected > Rotation > Z Increase Medium", Group = CommandConst.CAT_SELECTEDATOM  };
            yield return new AtomRotationChange(Axis.Z, -0.5f) { Name = "Selected > Rotation > Z Decrease Medium", Group = CommandConst.CAT_SELECTEDATOM  };
            yield return new AtomRotationChange(Axis.Z, 2.0f) { Name = "Selected > Rotation > Z Increase Large", Group = CommandConst.CAT_SELECTEDATOM  };
            yield return new AtomRotationChange(Axis.Z, -2.0f) { Name = "Selected > Rotation > Z Decrease Large", Group = CommandConst.CAT_SELECTEDATOM  };

            yield return new AtomSelectTab("Animation Pattern") { Name = "Selected > ShowUI > Animation Pattern", Group = CommandConst.CAT_SELECTEDATOM };
            yield return new AtomSelectTab("Animation Trigger") { Name = "Selected > ShowUI > Animation Trigger", Group = CommandConst.CAT_SELECTEDATOM };
            yield return new AtomSelectTab("Animation") { Name = "Selected > ShowUI > Animation", Group = CommandConst.CAT_SELECTEDATOM };
            yield return new AtomSelectTab("Appearance Presets") { Name = "Selected > ShowUI > Appearance Presets", Group = CommandConst.CAT_SELECTEDATOM };
            yield return new AtomSelectTab("Auto Behaviours") { Name = "Selected > ShowUI > Auto Behaviours", Group = CommandConst.CAT_SELECTEDATOM };
            yield return new AtomSelectTab("Camera") { Name = "Selected > ShowUI > Camera", Group = CommandConst.CAT_SELECTEDATOM };
            yield return new AtomSelectTab("Clothing Presets") { Name = "Selected > ShowUI > Clothing Presets", Group = CommandConst.CAT_SELECTEDATOM };
            yield return new AtomSelectTab("Clothing") { Name = "Selected > ShowUI > Clothing", Group = CommandConst.CAT_SELECTEDATOM };
            yield return new AtomSelectTab("Collision Trigger") { Name = "Selected > ShowUI > Collision Trigger", Group = CommandConst.CAT_SELECTEDATOM };
            yield return new AtomSelectTab("Collision Trigger") { Name = "Selected > ShowUI > Collision Trigger", Group = CommandConst.CAT_SELECTEDATOM };
            yield return new AtomSelectTab("Collision Triggers") { Name = "Selected > ShowUI > Collision Triggers", Group = CommandConst.CAT_SELECTEDATOM };
            yield return new AtomSelectTab("Control") { Name = "Selected > ShowUI > Control", Group = CommandConst.CAT_SELECTEDATOM };
            yield return new AtomSelectTab("F Breast Physics 1") { Name = "Selected > ShowUI > F Breast Physics 1", Group = CommandConst.CAT_SELECTEDATOM };
            yield return new AtomSelectTab("F Breast Physics 2") { Name = "Selected > ShowUI > F Breast Physics 2", Group = CommandConst.CAT_SELECTEDATOM };
            yield return new AtomSelectTab("F Breast Presets") { Name = "Selected > ShowUI > F Breast Presets", Group = CommandConst.CAT_SELECTEDATOM };
            yield return new AtomSelectTab("F Eyelash Materials") { Name = "Selected > ShowUI > F Eyelash Materials", Group = CommandConst.CAT_SELECTEDATOM };
            yield return new AtomSelectTab("F Glute Physics") { Name = "Selected > ShowUI > F Glute Physics", Group = CommandConst.CAT_SELECTEDATOM };
            yield return new AtomSelectTab("F Glute Presets") { Name = "Selected > ShowUI > F Glute Presets", Group = CommandConst.CAT_SELECTEDATOM };
            yield return new AtomSelectTab("Female Morphs") { Name = "Selected > ShowUI > Female Morphs", Group = CommandConst.CAT_SELECTEDATOM };
            yield return new AtomSelectTab("General Presets") { Name = "Selected > ShowUI > General Presets", Group = CommandConst.CAT_SELECTEDATOM };
            yield return new AtomSelectTab("Hair Presets") { Name = "Selected > ShowUI > Hair Presets", Group = CommandConst.CAT_SELECTEDATOM };
            yield return new AtomSelectTab("Hair") { Name = "Selected > ShowUI > Hair", Group = CommandConst.CAT_SELECTEDATOM };
            yield return new AtomSelectTab("Hand Control") { Name = "Selected > ShowUI > Hand Control", Group = CommandConst.CAT_SELECTEDATOM };
            yield return new AtomSelectTab("Head Audio") { Name = "Selected > ShowUI > Head Audio", Group = CommandConst.CAT_SELECTEDATOM };
            yield return new AtomSelectTab("Iris Materials") { Name = "Selected > ShowUI > Iris Materials", Group = CommandConst.CAT_SELECTEDATOM };
            yield return new AtomSelectTab("Lacrimals Materials") { Name = "Selected > ShowUI > Lacrimals Materials", Group = CommandConst.CAT_SELECTEDATOM };
            yield return new AtomSelectTab("Light") { Name = "Selected > ShowUI > Light", Group = CommandConst.CAT_SELECTEDATOM };
            yield return new AtomSelectTab("M Eyelash Materials") { Name = "Selected > ShowUI > M Eyelash Materials", Group = CommandConst.CAT_SELECTEDATOM };
            yield return new AtomSelectTab("M Pectoral Physics") { Name = "Selected > ShowUI > M Pectoral Physics", Group = CommandConst.CAT_SELECTEDATOM };
            yield return new AtomSelectTab("Male Morphs") { Name = "Selected > ShowUI > Male Morphs", Group = CommandConst.CAT_SELECTEDATOM };
            yield return new AtomSelectTab("Misc Physics") { Name = "Selected > ShowUI > Misc Physics", Group = CommandConst.CAT_SELECTEDATOM };
            yield return new AtomSelectTab("Morphs Presets") { Name = "Selected > ShowUI > Morphs Presets", Group = CommandConst.CAT_SELECTEDATOM };
            yield return new AtomSelectTab("Mouth Materials") { Name = "Selected > ShowUI > Mouth Materials", Group = CommandConst.CAT_SELECTEDATOM };
            yield return new AtomSelectTab("Move") { Name = "Selected > ShowUI > Move", Group = CommandConst.CAT_SELECTEDATOM };
            yield return new AtomSelectTab("Move") { Name = "Selected > ShowUI > Move", Group = CommandConst.CAT_SELECTEDATOM };
            yield return new AtomSelectTab("Physics Control") { Name = "Selected > ShowUI > Physics Control", Group = CommandConst.CAT_SELECTEDATOM };
            yield return new AtomSelectTab("Physics Object") { Name = "Selected > ShowUI > Physics Object", Group = CommandConst.CAT_SELECTEDATOM };
            yield return new AtomSelectTab("Plugins") { Name = "Selected > ShowUI > Plugins", Group = CommandConst.CAT_SELECTEDATOM };
            yield return new AtomSelectTab("Pose Presets") { Name = "Selected > ShowUI > Pose Presets", Group = CommandConst.CAT_SELECTEDATOM };
            yield return new AtomSelectTab("Preset") { Name = "Selected > ShowUI > Preset", Group = CommandConst.CAT_SELECTEDATOM };
            yield return new AtomSelectTab("Sclera Materials") { Name = "Selected > ShowUI > Sclera Materials", Group = CommandConst.CAT_SELECTEDATOM };
            yield return new AtomSelectTab("Skin Materials 1") { Name = "Selected > ShowUI > Skin Materials 1", Group = CommandConst.CAT_SELECTEDATOM };
            yield return new AtomSelectTab("Skin Materials 2") { Name = "Selected > ShowUI > Skin Materials 2", Group = CommandConst.CAT_SELECTEDATOM };
            yield return new AtomSelectTab("Skin Presets") { Name = "Selected > ShowUI > Skin Presets", Group = CommandConst.CAT_SELECTEDATOM };
            yield return new AtomSelectTab("Skin Select") { Name = "Selected > ShowUI > Skin Select", Group = CommandConst.CAT_SELECTEDATOM };
            yield return new AtomSelectTab("Skin Textures") { Name = "Selected > ShowUI > Skin Textures", Group = CommandConst.CAT_SELECTEDATOM };
            yield return new AtomSelectTab("Teeth Materials") { Name = "Selected > ShowUI > Teeth Materials", Group = CommandConst.CAT_SELECTEDATOM };
            yield return new AtomSelectTab("Tongue Materials") { Name = "Selected > ShowUI > Tongue Materials", Group = CommandConst.CAT_SELECTEDATOM };

            // SPECIFIC ATOM commands
            foreach(var atom in SuperController.singleton.GetSelectableAtoms().OrderBy((a) => a.uid))
            {
                var group = $"{atom.uid}";

                // actions for atoms only
                yield return new AtomHiddenToggle(atom) { Name = $"Atom > {atom.uid} > Hide > Toggle", DisplayName = "Hide > Toggle", Group = group, SubGroup = $"controller:{atom.mainController.name}" };
                yield return new AtomDump(atom) { Name = $"Atom > {atom.uid} > Print Object Structure", DisplayName = "Print Object Structure", Group = group, SubGroup = $"controller:{atom.mainController.name}" };
                yield return new PluginAdd(atom, showFilePrompt: true, openPluginUi: true) { Name = $"Atom > {atom.uid} > Add Plugin", DisplayName = "Add Plugin", Group = group, SubGroup = $"controller:{atom.mainController.name}" };

                // actions for UIButton atoms
                switch(atom.type) {
                    case "UIButton":
                        yield return new UIButtonTriggerCommand(atom) { Name = $"Atom > {atom.uid} > Run Triggers", DisplayName = "Run Triggers", Group = group, SubGroup = $"controller:{atom.mainController.name}" };
                        break;
                    case "AnimationPattern":
                        yield return new AnimationPatternCommand(atom, AnimationPatternCommand.PLAY) { Name = $"Atom > {atom.uid} > Pattern Play", DisplayName = "Pattern Play", Group = group, SubGroup = $"controller:{atom.mainController.name}" };
                        yield return new AnimationPatternCommand(atom, AnimationPatternCommand.RESET_AND_PLAY) { Name = $"Atom > {atom.uid} > Pattern Reset and Play", DisplayName = "Pattern Reset and Play", Group = group, SubGroup = $"controller:{atom.mainController.name}" };
                        yield return new AnimationPatternCommand(atom, AnimationPatternCommand.PAUSE) { Name = $"Atom > {atom.uid} > Pattern Pause", DisplayName = "Pattern Pause", Group = group, SubGroup = $"controller:{atom.mainController.name}" };
                        yield return new AnimationPatternCommand(atom, AnimationPatternCommand.TOGGLE_PAUSE) { Name = $"Atom > {atom.uid} > Pattern Toggle Pause", DisplayName = "Pattern Toggle Pause", Group = group, SubGroup = $"controller:{atom.mainController.name}" };
                        yield return new AnimationPatternCommand(atom, AnimationPatternCommand.RESET ) { Name = $"Atom > {atom.uid} > Pattern Reset", DisplayName = "Pattern Reset", Group = group, SubGroup = $"controller:{atom.mainController.name}" };

                        break;
                }

                foreach(var tab in atom.GetUITabNames())
                {
                    yield return new AtomSelectTab(tab, atom) { Name = $"Atom > {atom.uid} > ShowUI > {tab}", DisplayName = $"ShowUI > {tab}", Group = group, SubGroup = $"controller:{atom.mainController.name}" };
                }

                // actions for free controllers
                foreach(var controller in atom.freeControllers)
                {
                    var subGroup = $"controller:{controller.name}";
                    yield return new AtomSelect(controller) { Name = $"Atom > {atom.uid} > {controller.name} > Select", DisplayName = $"{controller.name} > Select", Group = group, SubGroup = subGroup };

                    yield return new AtomPositionSetLerp(Axis.X, 0, 1, controller) { Name = $"Atom > {atom.uid} > {controller.name} > Position > X > Interpolate 0 - 1", DisplayName = $"{controller.name} > Position > X > Interpolate 0 - 1", Group = group, SubGroup = subGroup };
                    yield return new AtomPositionSetLerp(Axis.Y, 0, 1, controller) { Name = $"Atom > {atom.uid} > {controller.name} > Position > Y > Interpolate 0 - 1", DisplayName = $"{controller.name} > Position > Y > Interpolate 0 - 1", Group = group, SubGroup = subGroup };
                    yield return new AtomPositionSetLerp(Axis.Z, 0, 1, controller) { Name = $"Atom > {atom.uid} > {controller.name} > Position > Z > Interpolate 0 - 1", DisplayName = $"{controller.name} > Position > Z > Interpolate 0 - 1", Group = group, SubGroup = subGroup };

                    yield return new AtomPositionChange(Axis.X, 0.5f, controller) { Name = $"Atom > {atom.uid} > {controller.name} > Position > X > Increase Small", DisplayName = $"{controller.name} > Position > X > Increase Small", Group = group, SubGroup = subGroup };
                    yield return new AtomPositionChange(Axis.X, -0.5f, controller) { Name = $"Atom > {atom.uid} > {controller.name} > Position > X > Decrease Small", DisplayName = $"{controller.name} > Position > X > Decrease Small", Group = group, SubGroup = subGroup };
                    yield return new AtomPositionChange(Axis.X, 2f, controller) { Name = $"Atom > {atom.uid} > {controller.name} > Position > X > Increase Medium", DisplayName = $"{controller.name} > Position > X > Increase Medium", Group = group, SubGroup = subGroup };
                    yield return new AtomPositionChange(Axis.X, -2f, controller) { Name = $"Atom > {atom.uid} > {controller.name} > Position > X > Decrease Medium", DisplayName = $"{controller.name} > Position > X > Decrease Medium", Group = group, SubGroup = subGroup };
                    yield return new AtomPositionChange(Axis.X, 5.0f, controller) { Name = $"Atom > {atom.uid} > {controller.name} > Position > X > Increase Large", DisplayName = $"{controller.name} > Position > X > Increase Large", Group = group, SubGroup = subGroup };
                    yield return new AtomPositionChange(Axis.X, -5.0f, controller) { Name = $"Atom > {atom.uid} > {controller.name} > Position > X > Decrease Large", DisplayName = $"{controller.name} > Position > X > Decrease Large", Group = group, SubGroup = subGroup };

                    yield return new AtomPositionChange(Axis.Y, 0.5f, controller) { Name = $"Atom > {atom.uid} > {controller.name} > Position > Y Increase Small", DisplayName = $"{controller.name} > Position > Y Increase Small", Group = group, SubGroup = subGroup };
                    yield return new AtomPositionChange(Axis.Y, -0.5f, controller) { Name = $"Atom > {atom.uid} > {controller.name} > Position > Y Decrease Small", DisplayName = $"{controller.name} > Position > Y Decrease Small", Group = group, SubGroup = subGroup };
                    yield return new AtomPositionChange(Axis.Y, 2f, controller) { Name = $"Atom > {atom.uid} > {controller.name} > Position > Y Increase Medium", DisplayName = $"{controller.name} > Position > Y Increase Medium", Group = group, SubGroup = subGroup };
                    yield return new AtomPositionChange(Axis.Y, -2f, controller) { Name = $"Atom > {atom.uid} > {controller.name} > Position > Y Decrease Medium", DisplayName = $"{controller.name} > Position > Y Decrease Medium", Group = group, SubGroup = subGroup };
                    yield return new AtomPositionChange(Axis.Y, 5.0f, controller) { Name = $"Atom > {atom.uid} > {controller.name} > Position > Y Increase Large", DisplayName = $"{controller.name} > Position > Y Increase Large", Group = group, SubGroup = subGroup };
                    yield return new AtomPositionChange(Axis.Y, -5.0f, controller) { Name = $"Atom > {atom.uid} > {controller.name} > Position > Y Decrease Large", DisplayName = $"{controller.name} > Position > Y Decrease Large", Group = group, SubGroup = subGroup };

                    yield return new AtomPositionChange(Axis.Z, 0.5f, controller) { Name = $"Atom > {atom.uid} > {controller.name} > Position > Z Increase Small", DisplayName = $"{controller.name} > Position > Z Increase Small", Group = group, SubGroup = subGroup };
                    yield return new AtomPositionChange(Axis.Z, -0.5f, controller) { Name = $"Atom > {atom.uid} > {controller.name} > Position > Z Decrease Small", DisplayName = $"{controller.name} > Position > Z Decrease Small", Group = group, SubGroup = subGroup };
                    yield return new AtomPositionChange(Axis.Z, 2f, controller) { Name = $"Atom > {atom.uid} > {controller.name} > Position > Z Increase Medium", DisplayName = $"{controller.name} > Position > Z Increase Medium", Group = group, SubGroup = subGroup };
                    yield return new AtomPositionChange(Axis.Z, -2f, controller) { Name = $"Atom > {atom.uid} > {controller.name} > Position > Z Decrease Medium", DisplayName = $"{controller.name} > Position > Z Decrease Medium", Group = group, SubGroup = subGroup };
                    yield return new AtomPositionChange(Axis.Z, 5.0f, controller) { Name = $"Atom > {atom.uid} > {controller.name} > Position > Z Increase Large", DisplayName = $"{controller.name} > Position > Z Increase Large", Group = group, SubGroup = subGroup };
                    yield return new AtomPositionChange(Axis.Z, -5.0f, controller) { Name = $"Atom > {atom.uid} > {controller.name} > Position > Z Decrease Large", DisplayName = $"{controller.name} > Position > Z Decrease Large", Group = group, SubGroup = subGroup };

                    yield return new AtomRotationChange(Axis.X, 0.25f, controller) { Name = $"Atom > {atom.uid} > {controller.name} > Rotation > X Increase Small", DisplayName = $"{controller.name} > Rotation > X Increase Small", Group = group, SubGroup = subGroup };
                    yield return new AtomRotationChange(Axis.X, -0.25f, controller) { Name = $"Atom > {atom.uid} > {controller.name} > Rotation > X Decrease Small", DisplayName = $"{controller.name} > Rotation > X Decrease Small", Group = group, SubGroup = subGroup };
                    yield return new AtomRotationChange(Axis.X, 0.5f, controller) { Name = $"Atom > {atom.uid} > {controller.name} > Rotation > X Increase Medium", DisplayName = $"{controller.name} > Rotation > X Increase Medium", Group = group, SubGroup = subGroup };
                    yield return new AtomRotationChange(Axis.X, -0.5f, controller) { Name = $"Atom > {atom.uid} > {controller.name} > Rotation > X Decrease Medium", DisplayName = $"{controller.name} > Rotation > X Decrease Medium", Group = group, SubGroup = subGroup };
                    yield return new AtomRotationChange(Axis.X, 2.0f, controller) { Name = $"Atom > {atom.uid} > {controller.name} > Rotation > X Increase Large", DisplayName = $"{controller.name} > Rotation > X Increase Large", Group = group, SubGroup = subGroup };
                    yield return new AtomRotationChange(Axis.X, -2.0f, controller) { Name = $"Atom > {atom.uid} > {controller.name} > Rotation > X Decrease Large", DisplayName = $"{controller.name} > Rotation > X Decrease Large", Group = group, SubGroup = subGroup };

                    yield return new AtomRotationChange(Axis.Y, 0.25f, controller) { Name = $"Atom > {atom.uid} > {controller.name} > Rotation > Y Increase Small", DisplayName = $"{controller.name} > Rotation > Y Increase Small", Group = group, SubGroup = subGroup };
                    yield return new AtomRotationChange(Axis.Y, -0.25f, controller) { Name = $"Atom > {atom.uid} > {controller.name} > Rotation > Y Decrease Small", DisplayName = $"{controller.name} > Rotation > Y Decrease Small", Group = group, SubGroup = subGroup };
                    yield return new AtomRotationChange(Axis.Y, 0.5f, controller) { Name = $"Atom > {atom.uid} > {controller.name} > Rotation > Y Increase Medium", DisplayName = $"{controller.name} > Rotation > Y Increase Medium", Group = group, SubGroup = subGroup };
                    yield return new AtomRotationChange(Axis.Y, -0.5f, controller) { Name = $"Atom > {atom.uid} > {controller.name} > Rotation > Y Decrease Medium", DisplayName = $"{controller.name} > Rotation > Y Decrease Medium", Group = group, SubGroup = subGroup };
                    yield return new AtomRotationChange(Axis.Y, 2.0f, controller) { Name = $"Atom > {atom.uid} > {controller.name} > Rotation > Y Increase Large", DisplayName = $"{controller.name} > Rotation > Y Increase Large", Group = group, SubGroup = subGroup };
                    yield return new AtomRotationChange(Axis.Y, -2.0f, controller) { Name = $"Atom > {atom.uid} > {controller.name} > Rotation > Y Decrease Large", DisplayName = $"{controller.name} > Rotation > Y Decrease Large", Group = group, SubGroup = subGroup };

                    yield return new AtomRotationChange(Axis.Z, 0.25f, controller) { Name = $"Atom > {atom.uid} > {controller.name} > Rotation > Z Increase Small", DisplayName = $"{controller.name} > Rotation > Z Increase Small", Group = group, SubGroup = subGroup };
                    yield return new AtomRotationChange(Axis.Z, -0.25f, controller) { Name = $"Atom > {atom.uid} > {controller.name} > Rotation > Z Decrease Small", DisplayName = $"{controller.name} > Rotation > Z Decrease Small", Group = group, SubGroup = subGroup };
                    yield return new AtomRotationChange(Axis.Z, 0.5f, controller) { Name = $"Atom > {atom.uid} > {controller.name} > Rotation > Z Increase Medium", DisplayName = $"{controller.name} > Rotation > Z Increase Medium", Group = group, SubGroup = subGroup };
                    yield return new AtomRotationChange(Axis.Z, -0.5f, controller) { Name = $"Atom > {atom.uid} > {controller.name} > Rotation > Z Decrease Medium", DisplayName = $"{controller.name} > Rotation > Z Decrease Medium", Group = group, SubGroup = subGroup };
                    yield return new AtomRotationChange(Axis.Z, 2.0f, controller) { Name = $"Atom > {atom.uid} > {controller.name} > Rotation > Z Increase Large", DisplayName = $"{controller.name} > Rotation > Z Increase Large", Group = group, SubGroup = subGroup };
                    yield return new AtomRotationChange(Axis.Z, -2.0f, controller) { Name = $"Atom > {atom.uid} > {controller.name} > Rotation > Z Decrease Large", DisplayName = $"{controller.name} > Rotation > Z Decrease Large", Group = group, SubGroup = subGroup };
                }

                // actions for plugins on an atom
                foreach(var plugin in atom.GetPluginStorables())
                {
                    var namePrefix = $"{atom.uid} > {plugin.name}";
                    var subGroup = plugin.name;

                    yield return new PluginShowUI(plugin) { Name = $"{namePrefix} > Show UI", DisplayName = "Show UI", Group = group, SubGroup = subGroup };

                    // booleans
                    foreach (var param in plugin.GetBoolParamNames().Where((param) => !param.Equals("enabled")))
                    {
                        yield return new PluginBoolSet(plugin, param, true) { Name = $"{namePrefix} > {param} > On", DisplayName = $"{param} > On", Group = group, SubGroup = subGroup };
                        yield return new PluginBoolSet(plugin, param, false) { Name = $"{namePrefix} > {param} > Off", DisplayName = $"{param} > Off", Group = group, SubGroup = subGroup };
                        yield return new PluginBoolToggle(plugin, param) { Name = $"{namePrefix} > {param} > Toggle", DisplayName = $"{param} > Toggle", Group = group, SubGroup = subGroup };
                    }
                    // actions
                    foreach (var param in plugin.GetCustomActionNames())
                    {
                        yield return new PluginActionCall(plugin, param) { Name = $"{namePrefix} > {param} > Call", DisplayName = $"{param}", Group = group, SubGroup = subGroup };
                    }

                    // floats
                    foreach (var param in plugin.GetFloatParamNames())
                    {
                        yield return new PluginFloatChange(plugin, param, 0.01f) { Name = $"{namePrefix} > {param} > +0.01", DisplayName = $"{param} > +0.01", Group = group, SubGroup = subGroup };
                        yield return new PluginFloatChange(plugin, param, -0.01f) { Name = $"{namePrefix} > {param} > -0.01", DisplayName = $"{param} > -0.01", Group = group, SubGroup = subGroup };
                        yield return new PluginFloatChange(plugin, param, 0.1f) { Name = $"{namePrefix} > {param} > +0.10", DisplayName = $"{param} > +0.10", Group = group, SubGroup = subGroup };
                        yield return new PluginFloatChange(plugin, param, -0.1f) { Name = $"{namePrefix} > {param} > -0.10", DisplayName = $"{param} > -0.10", Group = group, SubGroup = subGroup };
                        yield return new PluginFloatChange(plugin, param, 1.0f) { Name = $"{namePrefix} > {param} > +1.00", DisplayName = $"{param} > +1.00", Group = group, SubGroup = subGroup };
                        yield return new PluginFloatChange(plugin, param, -1.0f) { Name = $"{namePrefix} > {param} > -1.00", DisplayName = $"{param} > -1.00", Group = group, SubGroup = subGroup };
                    }
                    // select boxes (jsonstorablestringchoosers)
                    foreach (var param in plugin.GetStringChooserParamNames())
                    {
                        yield return new PluginStringChooserChange(plugin, param, 1) { Name = $"{namePrefix} > {param} > Next", DisplayName = $"{param} > Next", Group = group, SubGroup = subGroup };
                        yield return new PluginStringChooserChange(plugin, param, -1) { Name = $"{namePrefix} > {param} > Prev", DisplayName = $"{param} > Prev", Group = group, SubGroup = subGroup };
                    }

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
            { "Message Log > Toggle", "BackQuote" },
            { "Rescan Add-on Packages", "F5" },
            { "Hard Reset", "Shift-F5" }

        };

        public KeyChord GetDefaultKeyChordByActionName(string name)
        {
            return _defaultForAction.ContainsKey(name) ? new KeyChord(_defaultForAction[name]) : null;
        }
    }
}
