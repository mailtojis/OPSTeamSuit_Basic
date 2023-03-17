using System;
using System.Collections.Generic;

namespace OPSTeamSuitUI
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> DisticntBy<T, Tkey>(this IEnumerable<T> source, Func<T, Tkey> keyselect)
        {
            HashSet<Tkey> keys = new HashSet<Tkey>();
            foreach (var item in source)
            {
                if (keys.Add(keyselect(item))) yield return item;
            }
        }
    }
}
