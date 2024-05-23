using AmdarisProject.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AmdarisProject.Infrastructure.Persistance.Configurations
{
    public class GameTypeConfiguration : IEntityTypeConfiguration<GameType>
    {
        public void Configure(EntityTypeBuilder<GameType> builder)
        {
        }
    }
}
