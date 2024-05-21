using AmdarisProject.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AmdarisProject.Infrastructure.Persistance.Configurations
{
    internal class TeamPlayerConfiguration : IEntityTypeConfiguration<TeamPlayer>
    {
        public void Configure(EntityTypeBuilder<TeamPlayer> builder)
        {
            builder
                .HasOne(teamPlayer => teamPlayer.Team)
                .WithMany(team => team.TeamPlayers)
                .IsRequired();
            builder
                .HasOne(teamPlayer => teamPlayer.Player)
                .WithMany(player => player.TeamPlayers)
                .IsRequired();
        }
    }
}
