using System.Collections.Generic;
using System.Linq;
using Betto.Model.ViewModels;

namespace Betto.Helpers.Extensions
{
    public static class CollectionExtension
    {
        public static ICollection<T> GetEmptyIfNull<T>(this ICollection<T> list) => 
            list 
            ?? Enumerable.Empty<T>()
                .ToList();

        public static ICollection<UserRankingPositionViewModel> FillPositions(
            this ICollection<UserRankingPositionViewModel> ranking)
        {
            var position = 1;

            foreach (var rank in ranking)
            {
                rank.RankingPosition = position;
                ++position;
            }

            return ranking;
        }
    }
}
