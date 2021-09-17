#if !NETSTANDARD2_1_OR_GREATER
using System.Collections.Generic;

namespace System.Linq
{
    internal static class LinqExtensions
    {
        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> source, IEqualityComparer<T>? comparer = null)
        {
            return new HashSet<T>(source, comparer);
        }
    }
}
#endif