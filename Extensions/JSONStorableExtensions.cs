using System;
using System.Collections.Generic;
using System.Linq;

namespace LFE.KeyboardShortcuts.Extensions
{
    public static class JSONStorableExtensions
    {
        private static readonly List<string> _commonActionNames = new List<string>
        {
            "SaveToStore1", "SaveToStore2", "SaveToStore3", 
            "RestoreAllFromStore1", "RestoreAllFromStore2", "RestoreAllFromStore3",
            "RestorePhysicsFromStore1", "RestorePhysicsFromStore2", "RestorePhysicsFromStore3",
            "RestoreAppearanceFromStore1", "RestoreAppearanceFromStore2", "RestoreAppearanceFromStore3",
            "RestoreAllFromDefaults", "RestorePhysicalFromDefaults", "RestoreAppearanceFromDefaults"
        };

        public static List<string> GetCustomActionNames(this JSONStorable storable)
        {
            return storable.GetActionNames().Where((a) => !_commonActionNames.Contains(a)).ToList();
        }
    }
}
