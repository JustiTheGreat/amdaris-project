using AmdarisProject.Domain.Models;
using AmdarisProject.Domain.Models.CompetitionModels;
using AmdarisProject.Domain.Models.CompetitorModels;
using Microsoft.EntityFrameworkCore;

namespace AmdarisProject.Infrastructure
{
    public class AmdarisProjectDBContext : DbContext
    {
        public DbSet<Competitor> Competitors { get; set; }
        public DbSet<Competition> Competitions { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<Point> Points { get; set; }
        public DbSet<Stage> Stage { get; set; }

        public AmdarisProjectDBContext() { }

        public AmdarisProjectDBContext(DbContextOptions<AmdarisProjectDBContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseSqlServer("Server=ROMOB41072;Database=AmdarisProject2;Trusted_Connection=True;TrustServerCertificate=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Competition>().Property(competition => competition.Name).IsRequired();
            modelBuilder.Entity<Competition>().Property(competition => competition.Location).IsRequired();
            modelBuilder.Entity<Competition>().Property(competition => competition.StartTime).IsRequired();
            modelBuilder.Entity<Competition>().Property(competition => competition.Status).IsRequired().HasConversion<string>();
            modelBuilder.Entity<Competition>().Property(competition => competition.GameType).IsRequired().HasConversion<string>();
            modelBuilder.Entity<Competition>().Property(competition => competition.CompetitorType).IsRequired().HasConversion<string>();
            modelBuilder.Entity<Competition>()
                .HasMany(competition => competition.Competitors)
                .WithMany(competitor => competitor.Competitions);

            modelBuilder.Entity<Competition>(entity => entity.ToTable(table => table.HasCheckConstraint(
                "CK_win_rules",
                "[WinAt] <> NULL OR ([DurationInSeconds] <> NULL AND [BreakInSeconds] <> NULL)")));
            modelBuilder.Entity<Competition>(entity => entity.ToTable(table => table.HasCheckConstraint(
                "CK_competitor_type",
                "[CompetitorType] = 'Player' AND [TeamSize] = NULL OR [CompetitorType] = 'Team' AND [TeamSize] <> NULL")));

            modelBuilder.Entity<Competitor>().Property(competitor => competitor.Name).IsRequired();
            modelBuilder.Entity<Team>().Property(team => team.TeamSize).IsRequired();

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
            modelBuilder.Entity<Match>().HasOne(match => match.Competition).WithMany(competition => competition.Matches).IsRequired();
            modelBuilder.Entity<Match>().HasOne(match => match.Stage).WithMany(stage => stage.Matches);

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

            modelBuilder.Entity<Stage>()
                .HasOne(stage => stage.TournamentCompetition)
                .WithMany(tournamentCompetition => tournamentCompetition.Stages)
                .IsRequired();
        }
    }
}
