using System;
using System.Collections.Generic;

namespace Simple.CQRS.Extensions
{
    public static class EnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> sequence, Action<T> action)
        {
            foreach (T item in sequence)
            {
                action(item);
            }
        }
    }
}
