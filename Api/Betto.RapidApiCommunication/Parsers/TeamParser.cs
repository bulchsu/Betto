using System.Collections.Generic;
using System.Linq;
using Betto.Model.Entities;
using Newtonsoft.Json;

namespace Betto.RapidApiCommunication.Parsers
{
    public class TeamParser : IParser<TeamEntity>
    {
        public IEnumerable<TeamEntity> Parse(string rawJson)
        {
            var teams = JsonConvert.DeserializeAnonymousType(rawJson, new
            {
                Api = new
                {
                    Teams = default(List<TeamEntity>)
                }
            })?.Api?.Teams;

            var venues = JsonConvert.DeserializeAnonymousType(rawJson, new //venues are coming with response while teams are sent, wanted to seperate them into another entity
            {
                Api = new
                {
                    Teams = default(List<VenueEntity>) //it has to be named Teams, if not the parse will fail this way
                }
            })?.Api?.Teams;

            if (teams != null && venues != null)
            {
                for (var i = 0; i < teams.Count; i++)
                {
                    teams.ElementAt(i).Venue = venues.ElementAt(i);
                }
            }

            return teams;
        }
    }
}
