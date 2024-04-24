using AmdarisProject.Domain.Models.CompetitorModels;

namespace AmdarisProject.Domain.Extensions
{
    public static class TeamContainsPlayer
    {
        public static bool ContainsPlayer(this Team team, Guid playerId)
            => team.Players.Any(player => player.Id.Equals(playerId));
    }
}
