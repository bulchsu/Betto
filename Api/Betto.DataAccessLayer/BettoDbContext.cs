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

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
            => base.OnConfiguring(builder);

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<FixturesEntity>()
                .HasOne(f => f.Coverage)
                .WithOne(c => c.Fixtures)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<VenueEntity>()
                .HasOne(v => v.Team)
                .WithOne(t => t.Venue)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<CoverageEntity>()
                .HasOne(c => c.League)
                .WithOne(l => l.Coverage)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<TeamEntity>()
                .HasOne(t => t.League)
                .WithMany(l => l.Teams)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
