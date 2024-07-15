using AmdarisProject.Domain.Models.CompetitionModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AmdarisProject.Infrastructure.Persistance.Configurations
{
    internal class TournamentCompetitionConfiguration : IEntityTypeConfiguration<TournamentCompetition>
    {
        public void Configure(EntityTypeBuilder<TournamentCompetition> builder)
        {
            builder.Property(tournamentCompetition => tournamentCompetition.StageLevel).IsRequired();
            builder.ToTable(table => table.HasCheckConstraint(
                $"CK_{nameof(TournamentCompetition.StageLevel)}",
                $"[{nameof(TournamentCompetition.StageLevel)}] >= 0"));
        }
    }
}
