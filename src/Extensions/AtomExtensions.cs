using System.Collections.Generic;
using System.Linq;

namespace LFE.KeyboardShortcuts.Extensions
{
    public static class AtomExtensions {

        public static JSONStorable GetPluginStorable(this Atom atom, string pluginId)
        {
            foreach(var s in atom.GetPluginStorables())
            {
                if(s.name.Equals(pluginId))
                {
                    //SuperController.LogMessage($"returning {atom.uid} => {s.name}");
                    return s;
                }
            }
            //return atom.GetPluginStorables().Where((p) => p.name.Equals(pluginId)).FirstOrDefault();
            return null;
        }

        public static IEnumerable<JSONStorable> GetPluginStorables(this Atom atom)
        {
            MVRPluginManager manager = atom.GetComponentInChildren<MVRPluginManager>();
            if (manager != null)
            {
                var plugins = manager.GetJSON(true, true)["plugins"].AsObject;
                foreach(var pluginId in plugins.Keys)
                {
                    var receivers = atom
                        .GetStorableIDs()
                        .Where((sid) => sid.StartsWith(pluginId))
                        .Select((sid) => atom.GetStorableByID(sid));
                    foreach(var r in receivers)
                    {
                        yield return r;
                    }
                }
            }
        }

        public static UITabSelector GetTabSelector(this Atom atom)
        {
            if(!atom.freeControllers.Any((fc) => fc.selected))
            {
                SuperController.LogError("Unable to get tab selector if a free controller on an atom is not first selected");
                SuperController.LogError("Consider SuperController.singleton.SelectController(atom.mainController)");
                return null;
            }

            return atom
                .transform
                .Find((p) => p.EndsWith("Canvas/Panel/Content"))
                .FirstOrDefault()
                ?.GetComponent<UITabSelector>();
        }

        public static IEnumerable<string> GetUITabNames(this Atom atom)
        {
            switch(atom.type)
            {
                case "AnimationPattern":
                    yield return "Animation Triggers";
                    yield return "Plugins";

                    yield return "Collision Trigger";
                    yield return "Physics Object";
                    yield return "Physics Control";

                    yield return "Animation Pattern";
                    yield return "Animation";
                    yield return "Move";

                    yield return "Preset";
                    yield return "Control";
                    break;
                case "Person":
                    //first column
                    yield return "Clothing";
                    yield return "Clothing Presets";
                    yield return "Hair";
                    yield return "Hair Presets";
                    yield return "Male Morphs";
                    yield return "Female Morphs";
                    yield return "Morphs Presets";

                    yield return "Mouth Materials";
                    yield return "Tongue Materials";
                    yield return "Teeth Materials";
                    yield return "M Eyelash Materials";
                    yield return "F Eyelash Materials";
                    yield return "Lacrimals Materials";
                    yield return "Sclera Materials";
                    yield return "Iris Materials";
                    yield return "Skin Textures";
                    yield return "Skin Materials 2";
                    yield return "Skin Materials 1";
                    yield return "Skin Select";
                    yield return "Skin Presets";

                    //second column
                    yield return "Hand Control";
                    yield return "Head Audio";
                    yield return "Plugins";
                    yield return "Plugin Presets";
                    yield return "Auto Behaviours";

                    yield return "Collision Triggers";
                    yield return "Misc Physics";
                    yield return "M Pectoral Physics";
                    yield return "F Breast Physics 2";
                    yield return "F Breast Physics 1";
                    yield return "F Breast Presets";
                    yield return "F Glute Physics";
                    yield return "F Glute Presets";
                    yield return "Animation";
                    yield return "Move";
                    yield return "Pose Presets";

                    yield return "Appearance Presets";
                    yield return "General Presets";
                    yield return "Control & Physics 2"; // 1.20 and up
                    yield return "Control & Physics 1"; // 1.20 and up
                    yield return "Control"; // 1.19 and before
                    break;
                case "InvisibleLight":
                    yield return "Light";
                    yield return "Plugins";

                    yield return "Collision Trigger";
                    yield return "Physics Object";
                    yield return "Physics Control";

                    yield return "Animation";
                    yield return "Move";

                    yield return "Preset";
                    yield return "Control";
                    break;
                case "WindowCamera":
                    yield return "Camera";
                    yield return "Plugins";

                    yield return "Collision Trigger";
                    yield return "Physics Object";
                    yield return "Physics Control";

                    yield return "Animation";
                    yield return "Move";

                    yield return "Preset";
                    yield return "Control";
                    break;
                default:
                    break;
            }
        }
    }
}
