using System.Collections.Generic;
using System.Linq;

namespace LFE.KeyboardShortcuts.Extensions
{
    public static class AtomExtensions {

        public static UITabSelector GetTabSelector(this Atom atom)
        {
            if(!atom.mainController.selected)
            {
                SuperController.LogError("Unable to get tab selector if the atom is not first selected");
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
                    yield return "Control";
                    break;
                default:
                    break;
            }
        }
    }
}
