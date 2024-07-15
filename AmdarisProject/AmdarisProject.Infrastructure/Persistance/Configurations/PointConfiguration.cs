using AmdarisProject.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AmdarisProject.Infrastructure.Persistance.Configurations
{
    internal class PointConfiguration : IEntityTypeConfiguration<Point>
    {
        public void Configure(EntityTypeBuilder<Point> builder)
        {
            builder.Property(point => point.Value).IsRequired();
            builder
                .HasOne(point => point.Match)
                .WithMany(match => match.Points)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();
            builder
                .HasOne(point => point.Player)
                .WithMany(player => player.Points)
                .IsRequired();
        }
    }
}
