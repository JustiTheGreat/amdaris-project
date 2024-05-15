using AmdarisProject.Domain.Models;
using AmdarisProject.Domain.Models.CompetitionModels;
using AmdarisProject.Domain.Models.CompetitorModels;
using AmdarisProject.Infrastructure.Options;
using AmdarisProject.Presentation.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace AmdarisProject.Infrastructure.Persistance.Contexts
{
    public class AmdarisProjectDBContext(DbContextOptions dbContextOptions, IOptions<ConnectionStrings> connectionStringsOptions,
        IOptions<AssembliesNames> assembliesNamesOptions)
        : IdentityDbContext<IdentityUser>(dbContextOptions)
    {
        private readonly string _databaseConnection = connectionStringsOptions.Value.DatabaseConnection;
        private readonly string _entityConfigurationAssemblyName = assembliesNamesOptions.Value.EntityConfigurationAssemblyName;
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

            var entityConfigurationAssembly = Assembly.Load(_entityConfigurationAssemblyName);
            modelBuilder.ApplyConfigurationsFromAssembly(entityConfigurationAssembly);
        }
    }
}
