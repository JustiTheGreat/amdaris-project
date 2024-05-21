using AmdarisProject.Domain.Models.CompetitionModels;

namespace AmdarisProject.Domain.Models.CompetitorModels
{
    public class Team : Competitor
    {
        public virtual required List<Player> Players { get; set; } = [];

        public bool ContainsPlayer(Guid playerId)
            => Players.Any(player => player.Id.Equals(playerId));

        public override bool IsOrContainsCompetitor(Guid competitorId)
            => Id.Equals(competitorId) || ContainsPlayer(competitorId);

        public bool HasTheRequiredNumberOfActivePlayers(uint requiredNumberOfActivePlayers)
            => TeamPlayers.Count(teamPlayer => teamPlayer.IsActive) == requiredNumberOfActivePlayers;

        public bool ContainsAPlayerPartOfAnotherTeamFromCompetition(Competition competition)
            => Players.Any(player => competition.ContainsPlayer(player.Id));
    }
}
