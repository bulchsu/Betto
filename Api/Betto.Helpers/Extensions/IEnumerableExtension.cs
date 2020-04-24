using System.Collections.Generic;
using System.Linq;

namespace Betto.Helpers.Extensions
{
    public static class IEnumerableExtension
    {
        public static float Product(this IEnumerable<float> collection)
        {
            var product = collection.FirstOrDefault();

            for (var i = 1; i < collection.Count(); i++)
            {
                product *= collection.ElementAt(i);
            }

            return product;
        }
    }
}
