using System;
using System.Collections.Generic;
using System.Linq;

namespace LFE.KeyboardShortcuts.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> LoopOnceStartingWhen<T>(this IEnumerable<T> data, Func<T, bool> predicate)
        {
            var skipped = new List<T>();
            bool predicateMatched = false;
            foreach(T item in data)
            {
                if (!predicateMatched)
                {
                    skipped.Add(item);
                    if (predicate(item))
                    {
                        predicateMatched = true;
                    }
                }
                else
                {
                    yield return item;
                }
            }
            foreach(var item in skipped)
            {
                yield return item;
            }
        }
    }
}
