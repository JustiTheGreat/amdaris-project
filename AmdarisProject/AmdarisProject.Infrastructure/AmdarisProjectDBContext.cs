using Microsoft.EntityFrameworkCore;
using AmdarisProject.models;
using AmdarisProject.models.competitor;
using AmdarisProject.models.competition;

namespace AmdarisProject
{
    public class AmdarisProjectDBContext : DbContext
    {
        public DbSet<Competitor> Competitors { get; set; }
        public DbSet<Competition> Competitions { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<Point> Points { get; set; }
        public DbSet<Stage> Stage { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=ROMOB41072;Database=AmdarisProject;Trusted_Connection=True;TrustServerCertificate=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Match>().HasOne(match => match.CompetitorOne).WithMany().OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Match>().HasOne(match => match.CompetitorTwo).WithMany().OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Competitor>().HasMany(competitor => competitor.Matches);
        }
    }
}
