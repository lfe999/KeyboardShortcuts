using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace LFE.KeyboardShortcuts.Extensions
{
    public static class UITabSelectorExtensions
    {

        public static bool HasTabName(this UITabSelector tabUi, string name)
        {
            if(name == null)
            {
                return false;
            }
            return tabUi.GetTabNames().Any((n) => n.Equals(name));
        }

        public static IEnumerable<string> GetTabNames(this UITabSelector tabUi)
        {
            var tabContainer = tabUi?.transform?.Find("ToggleContainer");
            if(tabContainer == null)
            {
                yield break;
            }

            for(var i=0; i< tabContainer.childCount; i++)
            {
                var tab = tabContainer.GetChild(i) as RectTransform;
                if(tab != null)
                {
                    yield return tab.name;
                }
            }
        }
    }
}
