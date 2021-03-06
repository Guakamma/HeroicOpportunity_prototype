using System;
using System.Collections.Generic;


static class EnumerableExtensions {
    public static T MaxBy<T,U>(this IEnumerable<T> source, Func<T,U> selector)
        where U : IComparable<U> {
        if (source == null) throw new ArgumentNullException(nameof(source));
        bool first = true;
        T maxObj = default(T);
        U maxKey = default(U);
        foreach (var item in source) {
            if (first) {
                maxObj = item;
                maxKey = selector(maxObj);
                first = false;
            } else {
                U currentKey = selector(item);
                if (currentKey.CompareTo(maxKey) > 0) {
                    maxKey = currentKey;
                    maxObj = item;
                }
            }
        }
        if (first) throw new InvalidOperationException("Sequence is empty.");
        return maxObj;
    }
}
