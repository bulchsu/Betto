using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Betto.Model.Entities
{
    public class GameEntity : BaseGameEntity
    {
        [Required]
        public LeagueEntity League { get; set; }
        [ForeignKey(nameof(League)), JsonProperty("league_id", NullValueHandling = NullValueHandling.Ignore)]
        public int LeagueId { get; set; }
        [Required, JsonProperty("event_date", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime EventDate { get; set; }
        [Required, JsonProperty("event_timestamp", NullValueHandling = NullValueHandling.Ignore)]
        public long EventTimeStamp { get; set; }
        [Required]
        public long FirstHalfStart { get; set; }
        [Required]
        public long SecondHalfStart { get; set; }
        [Required, MaxLength(100)]
        public string Round { get; set; }
        [Required, MaxLength(100)]
        public string Status { get; set; }
        [Required, MaxLength(50)]
        public string StatusShort { get; set; }
        [Required]
        public int Elapsed { get; set; }
        [MaxLength(200)]
        public string Venue { get; set; }
        [MaxLength(200)]
        public string Referee { get; set; }
        [Required]
        public TeamEntity HomeTeam { get; set; }
        [ForeignKey(nameof(HomeTeam))]
        public int HomeTeamId { get; set; }
        [Required]
        public TeamEntity AwayTeam { get; set; }
        [ForeignKey(nameof(AwayTeam))]
        public int AwayTeamId { get; set; }
        [Required]
        public int GoalsHomeTeam { get; set; }
        [Required]
        public int GoalsAwayTeam { get; set; }
        [InverseProperty("Game")]
        public ScoreEntity Score { get; set; }
        [JsonProperty("fixture_id", NullValueHandling = NullValueHandling.Ignore)]
        public int RapidApiExternalId { get; set; }
        public BetRatesEntity Rates { get; set; }
    }
}
