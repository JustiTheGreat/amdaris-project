using AmdarisProject.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AmdarisProject.Infrastructure.Persistance.Configurations
{
    internal class MatchConfiguration : IEntityTypeConfiguration<Match>
    {
        public void Configure(EntityTypeBuilder<Match> builder)
        {
            builder.Property(match => match.Location).IsRequired();
            builder.Property(match => match.Status).IsRequired().HasConversion<string>();
            builder
                .HasOne(match => match.CompetitorOne)
                .WithMany(competitor => competitor.Matches)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();
            //builder
            //    .HasOne(match => match.CompetitorTwo)
            //    .WithMany(competitor => competitor.Matches)
            //    .OnDelete(DeleteBehavior.NoAction)
            //    .IsRequired();
            builder
                .HasOne(match => match.Winner)
                .WithMany(competitor => competitor.WonMatches)
                .OnDelete(DeleteBehavior.NoAction);
            builder
                .HasOne(match => match.Competition)
                .WithMany(competition => competition.Matches)
                .IsRequired();
        }
    }
}
