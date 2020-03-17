﻿// <auto-generated />
using System;
using Betto.DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Betto.DataAccessLayer.Migrations
{
    [DbContext(typeof(BettoDbContext))]
    [Migration("20200317170001_DeletionChanged")]
    partial class DeletionChanged
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Betto.Model.Entities.CoverageEntity", b =>
                {
                    b.Property<int>("CoverageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("FixturesId")
                        .HasColumnType("int");

                    b.Property<bool>("Odds")
                        .HasColumnType("bit");

                    b.Property<bool>("Players")
                        .HasColumnType("bit");

                    b.Property<bool>("Predictions")
                        .HasColumnType("bit");

                    b.Property<bool>("Standings")
                        .HasColumnType("bit");

                    b.Property<bool>("TopScorers")
                        .HasColumnType("bit");

                    b.HasKey("CoverageId");

                    b.HasIndex("FixturesId")
                        .IsUnique();

                    b.ToTable("Coverages");
                });

            modelBuilder.Entity("Betto.Model.Entities.FixturesEntity", b =>
                {
                    b.Property<int>("FixturesId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Events")
                        .HasColumnType("bit");

                    b.Property<bool>("Lineups")
                        .HasColumnType("bit");

                    b.Property<bool>("PlayerStatistics")
                        .HasColumnType("bit");

                    b.Property<bool>("Statistics")
                        .HasColumnType("bit");

                    b.HasKey("FixturesId");

                    b.ToTable("Fixtures");
                });

            modelBuilder.Entity("Betto.Model.Entities.LeagueEntity", b =>
                {
                    b.Property<int>("LeagueId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Country")
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("CountryCode")
                        .HasColumnType("varchar(10)")
                        .HasMaxLength(10);

                    b.Property<int>("CoverageId")
                        .HasColumnType("int");

                    b.Property<string>("Flag")
                        .HasColumnType("nvarchar(300)")
                        .HasMaxLength(300);

                    b.Property<bool>("IsCurrent")
                        .HasColumnType("bit");

                    b.Property<string>("Logo")
                        .HasColumnType("nvarchar(300)")
                        .HasMaxLength(300);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<int>("Season")
                        .HasColumnType("int");

                    b.Property<DateTime>("SeasonEnd")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("SeasonStart")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Standings")
                        .HasColumnType("bit");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.HasKey("LeagueId");

                    b.HasIndex("CoverageId")
                        .IsUnique();

                    b.ToTable("Leagues");
                });

            modelBuilder.Entity("Betto.Model.Entities.TeamEntity", b =>
                {
                    b.Property<int>("TeamId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Code")
                        .HasColumnType("varchar(10)")
                        .HasMaxLength(10);

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<int>("Founded")
                        .HasColumnType("int");

                    b.Property<string>("IsNational")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("LeagueId")
                        .HasColumnType("int");

                    b.Property<string>("Logo")
                        .IsRequired()
                        .HasColumnType("nvarchar(300)")
                        .HasMaxLength(300);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<int>("VenueId")
                        .HasColumnType("int");

                    b.HasKey("TeamId");

                    b.HasIndex("LeagueId");

                    b.HasIndex("VenueId")
                        .IsUnique();

                    b.ToTable("Teams");
                });

            modelBuilder.Entity("Betto.Model.Entities.VenueEntity", b =>
                {
                    b.Property<int>("VenueId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("VenueAddress")
                        .HasColumnType("nvarchar(200)")
                        .HasMaxLength(200);

                    b.Property<int>("VenueCapacity")
                        .HasColumnType("int");

                    b.Property<string>("VenueCity")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("VenueName")
                        .IsRequired()
                        .HasColumnType("nvarchar(200)")
                        .HasMaxLength(200);

                    b.Property<string>("VenueSurface")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.HasKey("VenueId");

                    b.ToTable("Venues");
                });

            modelBuilder.Entity("Betto.Model.Entities.CoverageEntity", b =>
                {
                    b.HasOne("Betto.Model.Entities.FixturesEntity", "Fixtures")
                        .WithOne("Coverage")
                        .HasForeignKey("Betto.Model.Entities.CoverageEntity", "FixturesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Betto.Model.Entities.LeagueEntity", b =>
                {
                    b.HasOne("Betto.Model.Entities.CoverageEntity", "Coverage")
                        .WithOne("League")
                        .HasForeignKey("Betto.Model.Entities.LeagueEntity", "CoverageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Betto.Model.Entities.TeamEntity", b =>
                {
                    b.HasOne("Betto.Model.Entities.LeagueEntity", "League")
                        .WithMany("Teams")
                        .HasForeignKey("LeagueId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Betto.Model.Entities.VenueEntity", "Venue")
                        .WithOne("Team")
                        .HasForeignKey("Betto.Model.Entities.TeamEntity", "VenueId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
