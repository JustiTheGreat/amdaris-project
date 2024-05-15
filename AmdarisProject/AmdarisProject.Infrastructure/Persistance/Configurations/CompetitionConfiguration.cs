using AmdarisProject.Domain.Models.CompetitionModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AmdarisProject.Infrastructure.Persistance.Configurations
{
    internal class CompetitionConfiguration : IEntityTypeConfiguration<Competition>
    {
        public void Configure(EntityTypeBuilder<Competition> builder)
        {
            builder
                .HasDiscriminator<string>("Discriminator")
                .HasValue<OneVSAllCompetition>(nameof(OneVSAllCompetition))
                .HasValue<TournamentCompetition>(nameof(TournamentCompetition));
            builder.Property(competition => competition.Name).IsRequired();
            builder.Property(competition => competition.Location).IsRequired();
            builder.Property(competition => competition.StartTime).IsRequired();
            builder.Property(competition => competition.Status).IsRequired().HasConversion<string>();
            builder
                .HasMany(competition => competition.Competitors)
                .WithMany(competitor => competitor.Competitions);
        }
    }
}
