using AmdarisProject.Domain.Models.CompetitorModels;

namespace AmdarisProject.Application.ExtensionMethods
{
    public static class TeamContainsPlayer
    {
        public static bool ContainsPlayer(this Team team, Guid playerId)
            => team.Players.Any(player => player.Id.Equals(playerId));
    }
}
