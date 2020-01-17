using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LFE.KeyboardShortcuts.Extensions
{
    public static class TransformExtensions {

        /// <summary>
        /// Finds a child where full child path matches the predicate
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="predicate">Function that takes full child path name and returns bool if it should be matched</param>
        /// <returns></returns>
        public static IEnumerable<Transform> Find(this Transform transform, Func<string, bool> predicate)
        {
            return transform.GetChildPathNames()
                .Where(predicate)
                .Select((p) => transform.Find(p))
                .Where((t) => t != null);
        }





        /// <summary>
        /// Return a list of all fully qualified child transform path names recursively
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="basePath"></param>
        /// <returns></returns>
        public static IEnumerable<string> GetChildPathNames(this Transform transform, string basePath = null)
        {
            foreach (Transform child in transform)
            {
                var path = basePath == null ? child.name : $"{basePath}/{child.name}";
                yield return path;
                foreach (var childPath in child.GetChildPathNames(basePath: path))
                {
                    yield return childPath;
                }
            }
        }
    }
}
