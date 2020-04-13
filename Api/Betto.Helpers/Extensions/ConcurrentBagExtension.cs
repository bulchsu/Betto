using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Betto.Helpers.Extensions
{
    public static class ConcurrentBagExtension
    {
        public static void AddRange<T>(this ConcurrentBag<T> bag, IEnumerable<T> newElements)
        {
            foreach (var element in newElements)
            {
                bag.Add(element);
            }
        }
    }
}
