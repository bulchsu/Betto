using System.Collections.Generic;
using Betto.Model.Entities;
using Newtonsoft.Json;

namespace Betto.RapidApiCommunication.Parsers
{
    public class GameParser : IParser<GameEntity>
    {
        public IEnumerable<GameEntity> Parse(string rawJson)
        {
            var games = JsonConvert.DeserializeAnonymousType(rawJson, new
            {
                Api = new
                {
                    Fixtures = default(List<GameEntity>)
                }
            })?.Api?.Fixtures;

            return games;
        }
    }
}
