using System;
using System.Collections.Generic;

namespace ModuleHW
{
    public static class Extensions
    {
        public static TItem MaxByKey<TItem, TKey>(
this IEnumerable<TItem> items, Func<TItem, TKey> keySelector)
        {
            var comparer = Comparer<TKey>.Default;

            var enumerator = items.GetEnumerator();
            if (!enumerator.MoveNext())
            {
                throw new InvalidOperationException("Collection is empty.");
            }

            TItem maxItem = enumerator.Current;
            TKey maxKey = keySelector(maxItem);

            while (enumerator.MoveNext())
            {
                TKey key = keySelector(enumerator.Current);
                if (comparer.Compare(key, maxKey) > 0)
                {
                    maxItem = enumerator.Current;
                    maxKey = key;
                }
            }

            return maxItem;
        }
    }
}
