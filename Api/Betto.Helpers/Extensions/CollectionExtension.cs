using System.Collections.Generic;
using System.Linq;

namespace Betto.Helpers.Extensions
{
    public static class CollectionExtension
    {
        public static ICollection<T> GetEmptyIfNull<T>(this ICollection<T> list)
            => list ?? Enumerable.Empty<T>().ToList();
    }
}
