using System;
using System.Collections.Generic;
using System.Linq;
using Qommon.Collections;

namespace Interactivity.Extensions
{
    internal static partial class Extensions
    {
        public static ReadOnlyCollection<T> AsReadOnlyCollection<T>(this IEnumerable<T> collection)
            => new ReadOnlyCollection<T>(collection.ToArray());

        public static ReadOnlyList<T> AsReadOnlyList<T>(this IEnumerable<T> collection)
            => new ReadOnlyList<T>(collection.ToArray());

        public static ReadOnlyDictionary<TKey, TValue> AsReadOnlyDictionary<TKey, TValue>(this Dictionary<TKey, TValue> dictionary)
            => new ReadOnlyDictionary<TKey, TValue>(dictionary);

        public static int FindIndex<T>(this IEnumerable<T> collection, Predicate<T> match)
        {
            int i = 0;

            foreach (var item in collection)
            {
                if (match.Invoke(item))
                {
                    return i;
                }

                i++;
            }

            return -1;
        }
    }
}
