using Betto.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace Betto.DataAccessLayer
{
    public class BettoDbContext : DbContext
    { 
        public BettoDbContext(DbContextOptions<BettoDbContext> options) 
            : base(options)
        {

        }

        public DbSet<LeagueEntity> Leagues { get; set; }
        public DbSet<CoverageEntity> Coverages { get; set; }
        public DbSet<FixturesEntity> Fixtures { get; set; }
        public DbSet<TeamEntity> Teams { get; set; }
        public DbSet<VenueEntity> Venues { get; set; }
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<GameEntity> Games { get; set; }
        public DbSet<ScoreEntity> Scores { get; set; }
        public DbSet<BetRatesEntity> Rates { get; set; }
        public DbSet<TicketEntity> Tickets { get; set; }
        public DbSet<EventEntity> Events { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<EventEntity>()
                .HasOne(e => e.Ticket)
                .WithMany(t => t.Events)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<TicketEntity>()
                .HasOne(t => t.User)
                .WithMany(u => u.Tickets)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<FixturesEntity>()
                .HasOne(f => f.Coverage)
                .WithOne(c => c.Fixtures)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<CoverageEntity>()
                .HasOne(c => c.League)
                .WithOne(l => l.Coverage)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<VenueEntity>()
                .HasOne(v => v.Team)
                .WithOne(t => t.Venue)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<BetRatesEntity>()
                .HasOne(b => b.Game)
                .WithOne(g => g.Rates)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<GameEntity>()
                .HasOne(g => g.HomeTeam)
                .WithMany(t => t.HomeGames)
                .HasForeignKey(g => g.HomeTeamId)
                .OnDelete(DeleteBehavior.ClientCascade);

            builder.Entity<GameEntity>()
                .HasOne(g => g.AwayTeam)
                .WithMany(t => t.AwayGames)
                .HasForeignKey(g => g.AwayTeamId)
                .OnDelete(DeleteBehavior.ClientCascade);

            builder.Entity<TeamEntity>()
                .HasOne(t => t.League)
                .WithMany(l => l.Teams)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
