using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AmdarisProject.Infrastructure.Persistance.Configurations
{
    internal class GameFormatConfiguration : IEntityTypeConfiguration<GameFormat>
    {
        public void Configure(EntityTypeBuilder<GameFormat> builder)
        {
            builder.Property(gameFormat => gameFormat.Name).IsRequired().HasConversion<string>();
            builder.HasOne(gameFormat => gameFormat.GameType).WithMany().IsRequired();
            builder.Property(gameFormat => gameFormat.CompetitorType).IsRequired().HasConversion<string>();
            builder.ToTable(table => table.HasCheckConstraint(
                $"CK_{nameof(GameFormat.CompetitorType)}",
                $"[{nameof(GameFormat.CompetitorType)}] = '{nameof(CompetitorType.PLAYER)}' AND [{nameof(GameFormat.TeamSize)}] = NULL " +
                $"OR [{nameof(GameFormat.CompetitorType)}] = '{nameof(CompetitorType.TEAM)}' AND [{nameof(GameFormat.TeamSize)}] <> NULL"));
            builder.ToTable(table => table.HasCheckConstraint(
                $"CK_WinRules",
                $"[{nameof(GameFormat.WinAt)}] <> NULL OR [{nameof(GameFormat.DurationInMinutes)}] <> NULL"));
        }
    }
}
