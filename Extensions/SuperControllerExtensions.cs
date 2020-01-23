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

    }
}
