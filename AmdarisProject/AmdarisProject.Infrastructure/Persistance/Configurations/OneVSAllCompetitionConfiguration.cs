using AmdarisProject.Domain.Models.CompetitionModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AmdarisProject.Infrastructure.Persistance.Configurations
{
    internal class OneVSAllCompetitionConfiguration : IEntityTypeConfiguration<OneVSAllCompetition>
    {
        public void Configure(EntityTypeBuilder<OneVSAllCompetition> builder)
        {
        }
    }
}
