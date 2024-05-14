using AmdarisProject.Application.Test.ModelBuilders;
using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Models;
using AmdarisProject.Domain.Models.CompetitionModels;
using AmdarisProject.Domain.Models.CompetitorModels;
using AmdarisProject.Presentation.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace AmdarisProject.Infrastructure
{
    public class AmdarisProjectDBContext(DbContextOptions dbContextOptions, IOptions<ConnectionStrings> connectionStringsOptions)
        : IdentityDbContext<IdentityUser>(dbContextOptions)
    {
        private readonly string _databaseConnection = connectionStringsOptions.Value.DatabaseConnection;
        public DbSet<Competitor> Competitors { get; set; }
        public DbSet<Competition> Competitions { get; set; }
        public DbSet<GameFormat> GameFormats { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<Point> Points { get; set; }
        public DbSet<TeamPlayer> TeamPlayers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseSqlServer(_databaseConnection);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Competition>()
                .HasDiscriminator<string>("Discriminator")
                .HasValue<OneVSAllCompetition>(nameof(OneVSAllCompetition))
                .HasValue<TournamentCompetition>(nameof(TournamentCompetition));

            modelBuilder.Entity<Competition>().Property(competition => competition.Name).IsRequired();
            modelBuilder.Entity<Competition>().Property(competition => competition.Location).IsRequired();
            modelBuilder.Entity<Competition>().Property(competition => competition.StartTime).IsRequired();
            modelBuilder.Entity<Competition>().Property(competition => competition.Status).IsRequired().HasConversion<string>();
            modelBuilder.Entity<Competition>().HasMany(competition => competition.Competitors).WithMany(competitor => competitor.Competitions);
            modelBuilder.Entity<TournamentCompetition>().Property(tournamentCompetition => tournamentCompetition.StageLevel).IsRequired();
            modelBuilder.Entity<TournamentCompetition>(entity => entity.ToTable(table => table.HasCheckConstraint(
                $"CK_{nameof(TournamentCompetition.StageLevel)}",
                $"[{nameof(TournamentCompetition.StageLevel)}] >= 0")));

            modelBuilder.Entity<GameFormat>().Property(gameFormat => gameFormat.Name).IsRequired().HasConversion<string>();
            modelBuilder.Entity<GameFormat>().Property(gameFormat => gameFormat.GameType).IsRequired().HasConversion<string>();
            modelBuilder.Entity<GameFormat>().Property(gameFormat => gameFormat.CompetitorType).IsRequired().HasConversion<string>();
            modelBuilder.Entity<GameFormat>(entity => entity.ToTable(table => table.HasCheckConstraint(
                $"CK_{nameof(GameFormat.CompetitorType)}",
                $"[{nameof(GameFormat.CompetitorType)}] = '{nameof(CompetitorType.PLAYER)}' AND [{nameof(GameFormat.TeamSize)}] = NULL " +
                $"OR [{nameof(GameFormat.CompetitorType)}] = '{nameof(CompetitorType.TEAM)}' AND [{nameof(GameFormat.TeamSize)}] <> NULL")));
            modelBuilder.Entity<GameFormat>(entity => entity.ToTable(table => table.HasCheckConstraint(
                $"CK_WinRules",
                $"[{nameof(GameFormat.WinAt)}] <> NULL OR [{nameof(GameFormat.DurationInMinutes)}] <> NULL")));

            modelBuilder.Entity<Competitor>().Property(competitor => competitor.Name).IsRequired();
            modelBuilder.Entity<Team>().HasMany(team => team.Players).WithMany(player => player.Teams).UsingEntity<TeamPlayer>();

            modelBuilder.Entity<Match>().Property(match => match.Location).IsRequired();
            modelBuilder.Entity<Match>().Property(match => match.Status).IsRequired().HasConversion<string>();
            modelBuilder.Entity<Match>()
                .HasOne(match => match.CompetitorOne)
                .WithMany(competitor => competitor.Matches)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();
            //modelBuilder.Entity<Match>()
            //    .HasOne(match => match.CompetitorTwo)
            //    .WithMany(competitor => competitor.Matches)
            //    .OnDelete(DeleteBehavior.NoAction)
            //    .IsRequired();
            modelBuilder.Entity<Match>()
                .HasOne(match => match.Winner)
                .WithMany(competitor => competitor.WonMatches)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Match>().HasOne(match => match.Competition).WithMany(competition => competition.Matches).IsRequired();

            modelBuilder.Entity<Point>().Property(point => point.Value).IsRequired();
            modelBuilder.Entity<Point>()
                .HasOne(point => point.Match)
                .WithMany(match => match.Points)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();
            modelBuilder.Entity<Point>()
                .HasOne(point => point.Player)
                .WithMany(player => player.Points)
                .IsRequired();
        }
    }
}
