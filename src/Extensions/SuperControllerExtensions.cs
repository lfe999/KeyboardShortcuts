using System;
using System.Collections.Generic;
using System.Linq;

namespace LFE.KeyboardShortcuts.Extensions
{
    public static class SuperControllerExtensions
    {
        /// <summary>
        /// Get a FreeControllerV3 by name
        /// </summary>
        /// <param name="sc"></param>
        /// <param name="atomUid"></param>
        /// <param name="controllerName"></param>
        /// <returns></returns>
        public static FreeControllerV3 GetFreeController(this SuperController sc, string atomUid, string controllerName)
        {
            if(atomUid == null || controllerName == null)
            {
                return null;
            }
            return sc.GetAtomByUid(atomUid)?.freeControllers?.Where((fc) => fc.name.Equals(controllerName)).FirstOrDefault();
        }

        /// <summary>
        /// Get all plugin storables
        /// </summary>
        /// <param name="sc"></param>
        /// <returns></returns>
        public static IEnumerable<JSONStorable> GetAllPlugins(this SuperController sc)
        {
            return sc.GetAtoms().SelectMany((a) => a.GetPluginStorables());
        }

        /// <summary>
        /// Get a plugin instance
        /// </summary>
        /// <param name="sc"></param>
        /// <param name="atomUid"></param>
        /// <param name="pluginName"></param>
        /// <returns></returns>
        public static JSONStorable GetPluginStorable(this SuperController sc, string atomUid, string pluginName)
        {
            if(atomUid == null || pluginName == null) { return null; }
            return sc.GetAtomByUid(atomUid)?.GetPluginStorable(pluginName);
        }

        /// <summary>
        /// Get all of the atoms that have a controller
        /// </summary>
        /// <param name="sc"></param>
        /// <returns></returns>
        public static IEnumerable<Atom> GetSelectableAtoms(this SuperController sc)
        {
            return sc.GetAtoms().Where((a) => a.mainController != null);
        }

        public static IEnumerable<FreeControllerV3> GetSelectableControllers(this SuperController sc)
        {
            return sc.GetSelectableAtoms().SelectMany(a => a.freeControllers, (a, c) => c);
        }

        public static IEnumerable<string> GetAtomTypesByCategory(this SuperController sc, string category)
        {
            if(!sc.GetAtomCategories().Any((c) => c.Equals(category)))
            {
                yield break;
            }

            var originalCategory = sc.atomCategoryPopup.currentValue;
            try
            {
                sc.atomCategoryPopup.currentValue = category;
                foreach (var type in sc.atomPrefabPopup.popupValues)
                {
                    yield return type;
                }
            }
            finally
            {
                sc.atomCategoryPopup.currentValue = originalCategory;
            }
        }
    }
}
