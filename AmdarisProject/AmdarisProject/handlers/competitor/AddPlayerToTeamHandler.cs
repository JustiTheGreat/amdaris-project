using AmdarisProject.models.competitor;
using AmdarisProject.repositories.abstractions;
using AmdarisProject.utils;
using AmdarisProject.utils.exceptions;
using MediatR;

namespace AmdarisProject.handlers.competitor
{
    public record AddPlayerToTeam(ulong PlayerId, ulong TeamId) : IRequest<Competitor>;
    public class AddPlayerToTeamHandler(ICompetitorRepository competitorRepository)
        : IRequestHandler<AddPlayerToTeam, Competitor>
    {
        private readonly ICompetitorRepository _competitorRepository = competitorRepository;

        public Task<Competitor> Handle(AddPlayerToTeam request, CancellationToken cancellationToken)
        {
            //TODO what kind of persistence a team should have (per competition, or forever)
            Team team = (Team)_competitorRepository.GetById(request.TeamId);

            if (team.Players.Count == team.TeamSize)
                throw new APCompetitorNumberException(nameof(AddPlayerToTeamHandler), nameof(Handle), $"Team {team.Id} is full!");

            if (Utils.TeamContainsPlayer(team, request.PlayerId))
                throw new APCompetitorException(nameof(AddPlayerToTeamHandler), nameof(Handle),
                    $"Player {request.PlayerId} is already a member of team {team.Id}!");

            Player player = (Player)_competitorRepository.GetById(request.PlayerId);
            team.Players.Add(player);
            Competitor updated = _competitorRepository.Update(team);
            return Task.FromResult(updated);
        }
    }
}
