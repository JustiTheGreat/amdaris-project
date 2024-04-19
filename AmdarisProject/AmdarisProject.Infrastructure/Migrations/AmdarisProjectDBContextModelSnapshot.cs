﻿// <auto-generated />
using System;
using AmdarisProject.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AmdarisProject.Infrastructure.Migrations
{
    [DbContext(typeof(AmdarisProjectDBContext))]
    partial class AmdarisProjectDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("AmdarisProject.Domain.Models.CompetitionModels.Competition", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal?>("BreakInSeconds")
                        .HasColumnType("decimal(20,0)");

                    b.Property<string>("CompetitorType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasMaxLength(21)
                        .HasColumnType("nvarchar(21)");

                    b.Property<decimal?>("DurationInSeconds")
                        .HasColumnType("decimal(20,0)");

                    b.Property<string>("GameType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("TeamSize")
                        .HasColumnType("int");

                    b.Property<long?>("WinAt")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.ToTable("Competitions", t =>
                        {
                            t.HasCheckConstraint("CK_StageLevel", "[StageLevel] >= 0");

                            t.HasCheckConstraint("CK_competitor_type", "[CompetitorType] = 'Player' AND [TeamSize] = NULL OR [CompetitorType] = 'Team' AND [TeamSize] <> NULL");

                            t.HasCheckConstraint("CK_win_rules", "[WinAt] <> NULL OR ([DurationInSeconds] <> NULL AND [BreakInSeconds] <> NULL)");
                        });

                    b.HasDiscriminator<string>("Discriminator").HasValue("Competition");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("AmdarisProject.Domain.Models.CompetitorModels.Competitor", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasMaxLength(13)
                        .HasColumnType("nvarchar(13)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Competitors");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Competitor");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("AmdarisProject.Domain.Models.Match", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CompetitionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CompetitorOneId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<long?>("CompetitorOnePoints")
                        .HasColumnType("bigint");

                    b.Property<Guid>("CompetitorTwoId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<long?>("CompetitorTwoPoints")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("EndTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("StageIndex")
                        .HasColumnType("int");

                    b.Property<int?>("StageLevel")
                        .HasColumnType("int");

                    b.Property<DateTime?>("StartTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("WinnerId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("CompetitionId");

                    b.HasIndex("CompetitorOneId");

                    b.HasIndex("CompetitorTwoId");

                    b.HasIndex("WinnerId");

                    b.ToTable("Matches");
                });

            modelBuilder.Entity("AmdarisProject.Domain.Models.Point", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("MatchId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("PlayerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<long>("Value")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("MatchId");

                    b.HasIndex("PlayerId");

                    b.ToTable("Points");
                });

            modelBuilder.Entity("CompetitionCompetitor", b =>
                {
                    b.Property<Guid>("CompetitionsId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CompetitorsId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("CompetitionsId", "CompetitorsId");

                    b.HasIndex("CompetitorsId");

                    b.ToTable("CompetitionCompetitor");
                });

            modelBuilder.Entity("PlayerTeam", b =>
                {
                    b.Property<Guid>("PlayersId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("TeamsId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("PlayersId", "TeamsId");

                    b.HasIndex("TeamsId");

                    b.ToTable("PlayerTeam");
                });

            modelBuilder.Entity("AmdarisProject.Domain.Models.CompetitionModels.TournamentCompetition", b =>
                {
                    b.HasBaseType("AmdarisProject.Domain.Models.CompetitionModels.Competition");

                    b.Property<int>("StageLevel")
                        .HasColumnType("int");

                    b.ToTable(t =>
                        {
                            t.HasCheckConstraint("CK_StageLevel", "[StageLevel] >= 0");

                            t.HasCheckConstraint("CK_competitor_type", "[CompetitorType] = 'Player' AND [TeamSize] = NULL OR [CompetitorType] = 'Team' AND [TeamSize] <> NULL");

                            t.HasCheckConstraint("CK_win_rules", "[WinAt] <> NULL OR ([DurationInSeconds] <> NULL AND [BreakInSeconds] <> NULL)");
                        });

                    b.HasDiscriminator().HasValue("TournamentCompetition");
                });

            modelBuilder.Entity("AmdarisProject.Domain.Models.CompetitorModels.Player", b =>
                {
                    b.HasBaseType("AmdarisProject.Domain.Models.CompetitorModels.Competitor");

                    b.HasDiscriminator().HasValue("Player");
                });

            modelBuilder.Entity("AmdarisProject.Domain.Models.CompetitorModels.Team", b =>
                {
                    b.HasBaseType("AmdarisProject.Domain.Models.CompetitorModels.Competitor");

                    b.Property<int>("TeamSize")
                        .HasColumnType("int");

                    b.HasDiscriminator().HasValue("Team");
                });

            modelBuilder.Entity("AmdarisProject.Domain.Models.Match", b =>
                {
                    b.HasOne("AmdarisProject.Domain.Models.CompetitionModels.Competition", "Competition")
                        .WithMany("Matches")
                        .HasForeignKey("CompetitionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AmdarisProject.Domain.Models.CompetitorModels.Competitor", "CompetitorOne")
                        .WithMany("Matches")
                        .HasForeignKey("CompetitorOneId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("AmdarisProject.Domain.Models.CompetitorModels.Competitor", "CompetitorTwo")
                        .WithMany()
                        .HasForeignKey("CompetitorTwoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AmdarisProject.Domain.Models.CompetitorModels.Competitor", "Winner")
                        .WithMany("WonMatches")
                        .HasForeignKey("WinnerId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.Navigation("Competition");

                    b.Navigation("CompetitorOne");

                    b.Navigation("CompetitorTwo");

                    b.Navigation("Winner");
                });

            modelBuilder.Entity("AmdarisProject.Domain.Models.Point", b =>
                {
                    b.HasOne("AmdarisProject.Domain.Models.Match", "Match")
                        .WithMany("Points")
                        .HasForeignKey("MatchId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("AmdarisProject.Domain.Models.CompetitorModels.Player", "Player")
                        .WithMany("Points")
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Match");

                    b.Navigation("Player");
                });

            modelBuilder.Entity("CompetitionCompetitor", b =>
                {
                    b.HasOne("AmdarisProject.Domain.Models.CompetitionModels.Competition", null)
                        .WithMany()
                        .HasForeignKey("CompetitionsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AmdarisProject.Domain.Models.CompetitorModels.Competitor", null)
                        .WithMany()
                        .HasForeignKey("CompetitorsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PlayerTeam", b =>
                {
                    b.HasOne("AmdarisProject.Domain.Models.CompetitorModels.Player", null)
                        .WithMany()
                        .HasForeignKey("PlayersId")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.HasOne("AmdarisProject.Domain.Models.CompetitorModels.Team", null)
                        .WithMany()
                        .HasForeignKey("TeamsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AmdarisProject.Domain.Models.CompetitionModels.Competition", b =>
                {
                    b.Navigation("Matches");
                });

            modelBuilder.Entity("AmdarisProject.Domain.Models.CompetitorModels.Competitor", b =>
                {
                    b.Navigation("Matches");

                    b.Navigation("WonMatches");
                });

            modelBuilder.Entity("AmdarisProject.Domain.Models.Match", b =>
                {
                    b.Navigation("Points");
                });

            modelBuilder.Entity("AmdarisProject.Domain.Models.CompetitorModels.Player", b =>
                {
                    b.Navigation("Points");
                });
#pragma warning restore 612, 618
        }
    }
}
