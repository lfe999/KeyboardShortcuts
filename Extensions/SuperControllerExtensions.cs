using System;
using System.Collections.Generic;
using System.Linq;

namespace LFE.KeyboardShortcuts.Extensions
{
    public static class SuperControllerExtensions
    {
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
        /// Get all of the atoms that have a controller
        /// </summary>
        /// <param name="sc"></param>
        /// <returns></returns>
        public static IEnumerable<Atom> GetSelectableAtoms(this SuperController sc)
        {
            return sc.GetAtoms().Where((a) => a.mainController != null);
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
