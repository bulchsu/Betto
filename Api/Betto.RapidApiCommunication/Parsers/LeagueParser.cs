using System.Collections.Generic;
using Betto.Model.Entities;
using Newtonsoft.Json;

namespace Betto.RapidApiCommunication.Parsers
{
    public class LeagueParser : IParser<LeagueEntity>
    {
        public IEnumerable<LeagueEntity> Parse(string rawJson)
        {
            var leagues = JsonConvert.DeserializeAnonymousType(rawJson, new
            {
                Api = new
                {
                    Leagues = default(List<LeagueEntity>)
                }
            })?.Api?.Leagues;

            return leagues;
        }
    }
}
