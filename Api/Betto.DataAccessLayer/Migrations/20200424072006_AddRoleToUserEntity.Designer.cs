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
    [Migration("20200424072006_AddRoleToUserEntity")]
    partial class AddRoleToUserEntity
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Betto.Model.Entities.BetRatesEntity", b =>
                {
                    b.Property<int>("BetRateId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<float>("AwayTeamWinRate")
                        .HasColumnType("real");

                    b.Property<int>("GameId")
                        .HasColumnType("int");

                    b.Property<float>("HomeTeamWinRate")
                        .HasColumnType("real");

                    b.Property<float>("TieRate")
                        .HasColumnType("real");

                    b.HasKey("BetRateId");

                    b.HasIndex("GameId")
                        .IsUnique();

                    b.ToTable("Rates");
                });

            modelBuilder.Entity("Betto.Model.Entities.CoverageEntity", b =>
                {
                    b.Property<int>("CoverageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("LeagueId")
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

                    b.HasIndex("LeagueId")
                        .IsUnique();

                    b.ToTable("Coverages");
                });

            modelBuilder.Entity("Betto.Model.Entities.EventEntity", b =>
                {
                    b.Property<int>("EventId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("BetType")
                        .HasColumnType("int");

                    b.Property<float>("ConfirmedRate")
                        .HasColumnType("real");

                    b.Property<int>("EventStatus")
                        .HasColumnType("int");

                    b.Property<int>("GameId")
                        .HasColumnType("int");

                    b.Property<int>("TicketId")
                        .HasColumnType("int");

                    b.HasKey("EventId");

                    b.HasIndex("GameId");

                    b.HasIndex("TicketId");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("Betto.Model.Entities.FixturesEntity", b =>
                {
                    b.Property<int>("FixturesId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CoverageId")
                        .HasColumnType("int");

                    b.Property<bool>("Events")
                        .HasColumnType("bit");

                    b.Property<bool>("Lineups")
                        .HasColumnType("bit");

                    b.Property<bool>("PlayerStatistics")
                        .HasColumnType("bit");

                    b.Property<bool>("Statistics")
                        .HasColumnType("bit");

                    b.HasKey("FixturesId");

                    b.HasIndex("CoverageId")
                        .IsUnique();

                    b.ToTable("Fixtures");
                });

            modelBuilder.Entity("Betto.Model.Entities.GameEntity", b =>
                {
                    b.Property<int>("GameId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AwayTeamId")
                        .HasColumnType("int");

                    b.Property<int>("Elapsed")
                        .HasColumnType("int");

                    b.Property<DateTime>("EventDate")
                        .HasColumnType("datetime2");

                    b.Property<long>("EventTimeStamp")
                        .HasColumnType("bigint");

                    b.Property<long>("FirstHalfStart")
                        .HasColumnType("bigint");

                    b.Property<int>("GoalsAwayTeam")
                        .HasColumnType("int");

                    b.Property<int>("GoalsHomeTeam")
                        .HasColumnType("int");

                    b.Property<int>("HomeTeamId")
                        .HasColumnType("int");

                    b.Property<int>("LeagueId")
                        .HasColumnType("int");

                    b.Property<int>("RapidApiExternalId")
                        .HasColumnType("int");

                    b.Property<string>("Referee")
                        .HasColumnType("nvarchar(200)")
                        .HasMaxLength(200);

                    b.Property<string>("Round")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<long>("SecondHalfStart")
                        .HasColumnType("bigint");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("StatusShort")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("Venue")
                        .HasColumnType("nvarchar(200)")
                        .HasMaxLength(200);

                    b.HasKey("GameId");

                    b.HasIndex("AwayTeamId");

                    b.HasIndex("HomeTeamId");

                    b.HasIndex("LeagueId");

                    b.ToTable("Games");
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

                    b.Property<int>("RapidApiExternalId")
                        .HasColumnType("int");

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

                    b.ToTable("Leagues");
                });

            modelBuilder.Entity("Betto.Model.Entities.PaymentEntity", b =>
                {
                    b.Property<int>("PaymentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double>("Amount")
                        .HasColumnType("float");

                    b.Property<DateTime>("PaymentDateTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("PaymentId");

                    b.HasIndex("UserId");

                    b.ToTable("Payments");
                });

            modelBuilder.Entity("Betto.Model.Entities.ScoreEntity", b =>
                {
                    b.Property<int>("GameId")
                        .HasColumnType("int");

                    b.Property<string>("ExtraTime")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullTime")
                        .HasColumnType("nvarchar(20)")
                        .HasMaxLength(20);

                    b.Property<string>("HalfTime")
                        .HasColumnType("nvarchar(20)")
                        .HasMaxLength(20);

                    b.Property<string>("Penalty")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("GameId");

                    b.ToTable("Scores");
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

                    b.Property<int?>("Founded")
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

                    b.Property<int>("RapidApiExternalId")
                        .HasColumnType("int");

                    b.HasKey("TeamId");

                    b.HasIndex("LeagueId");

                    b.ToTable("Teams");
                });

            modelBuilder.Entity("Betto.Model.Entities.TicketEntity", b =>
                {
                    b.Property<int>("TicketId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreationDateTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("RevealDateTime")
                        .HasColumnType("datetime2");

                    b.Property<double>("Stake")
                        .HasColumnType("float");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<float>("TotalConfirmedRate")
                        .HasColumnType("real");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("TicketId");

                    b.HasIndex("UserId");

                    b.ToTable("Tickets");
                });

            modelBuilder.Entity("Betto.Model.Entities.UserEntity", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double>("AccountBalance")
                        .HasColumnType("float");

                    b.Property<string>("MailAddress")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("varbinary(64)")
                        .HasMaxLength(64);

                    b.Property<string>("Role")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Betto.Model.Entities.VenueEntity", b =>
                {
                    b.Property<int>("VenueId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(200)")
                        .HasMaxLength(200);

                    b.Property<int>("Capacity")
                        .HasColumnType("int");

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(200)")
                        .HasMaxLength(200);

                    b.Property<string>("Surface")
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<int>("TeamId")
                        .HasColumnType("int");

                    b.HasKey("VenueId");

                    b.HasIndex("TeamId")
                        .IsUnique();

                    b.ToTable("Venues");
                });

            modelBuilder.Entity("Betto.Model.Entities.BetRatesEntity", b =>
                {
                    b.HasOne("Betto.Model.Entities.GameEntity", "Game")
                        .WithOne("Rates")
                        .HasForeignKey("Betto.Model.Entities.BetRatesEntity", "GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Betto.Model.Entities.CoverageEntity", b =>
                {
                    b.HasOne("Betto.Model.Entities.LeagueEntity", "League")
                        .WithOne("Coverage")
                        .HasForeignKey("Betto.Model.Entities.CoverageEntity", "LeagueId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Betto.Model.Entities.EventEntity", b =>
                {
                    b.HasOne("Betto.Model.Entities.GameEntity", "Game")
                        .WithMany()
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Betto.Model.Entities.TicketEntity", "Ticket")
                        .WithMany("Events")
                        .HasForeignKey("TicketId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Betto.Model.Entities.FixturesEntity", b =>
                {
                    b.HasOne("Betto.Model.Entities.CoverageEntity", "Coverage")
                        .WithOne("Fixtures")
                        .HasForeignKey("Betto.Model.Entities.FixturesEntity", "CoverageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Betto.Model.Entities.GameEntity", b =>
                {
                    b.HasOne("Betto.Model.Entities.TeamEntity", "AwayTeam")
                        .WithMany("AwayGames")
                        .HasForeignKey("AwayTeamId")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.HasOne("Betto.Model.Entities.TeamEntity", "HomeTeam")
                        .WithMany("HomeGames")
                        .HasForeignKey("HomeTeamId")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.HasOne("Betto.Model.Entities.LeagueEntity", "League")
                        .WithMany("Games")
                        .HasForeignKey("LeagueId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Betto.Model.Entities.PaymentEntity", b =>
                {
                    b.HasOne("Betto.Model.Entities.UserEntity", "User")
                        .WithMany("Payments")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Betto.Model.Entities.ScoreEntity", b =>
                {
                    b.HasOne("Betto.Model.Entities.GameEntity", "Game")
                        .WithOne("Score")
                        .HasForeignKey("Betto.Model.Entities.ScoreEntity", "GameId")
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
                });

            modelBuilder.Entity("Betto.Model.Entities.TicketEntity", b =>
                {
                    b.HasOne("Betto.Model.Entities.UserEntity", "User")
                        .WithMany("Tickets")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Betto.Model.Entities.VenueEntity", b =>
                {
                    b.HasOne("Betto.Model.Entities.TeamEntity", "Team")
                        .WithOne("Venue")
                        .HasForeignKey("Betto.Model.Entities.VenueEntity", "TeamId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
