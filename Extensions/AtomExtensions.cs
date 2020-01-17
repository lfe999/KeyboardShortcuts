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
                    yield return "Clothing";
                    yield return "Clothing Presets";
                    yield return "Hair";
                    yield return "Male Morphs";
                    yield return "Female Morphs";
                    yield return "Skin Textures";
                    yield return "Iris Materials";
                    yield return "Sclera Materials";
                    yield return "Auto Behaviors";
                    yield return "Collision Triggers";
                    yield return "Plugins";
                    yield return "Animation";
                    yield return "Move";
                    yield return "Control";
                    break;
                default:
                    break;
            }
        }
    }
}
