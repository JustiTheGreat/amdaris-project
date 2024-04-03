using AmdarisProject.models;
using AmdarisProject.models.competitor;
using AmdarisProject.repositories.abstractions;
using Domain.Enums;
using MediatR;

namespace AmdarisProject.handlers.match
{
    public record InitializePointsForMatchPlayers(ulong MatchId) : IRequest<bool>;
    public class InitializePointsForMatchPlayersHandler(IMatchRepository matchRepository, ICompetitorRepository competitorRepository,
        IPointRepository pointRepository)
        : IRequestHandler<InitializePointsForMatchPlayers, bool>
    {
        private readonly IMatchRepository _matchRepository = matchRepository;
        private readonly ICompetitorRepository _competitorRepository = competitorRepository;
        private readonly IPointRepository _pointRepository = pointRepository;

        public Task<bool> Handle(InitializePointsForMatchPlayers request, CancellationToken cancellationToken)
        {
            Match match = _matchRepository.GetById(request.MatchId);


            void createPoint(ulong competitorId)
            {
                Player player = (Player)_competitorRepository.GetById(competitorId);
                _pointRepository.Create(new Point(0, match, player));
            }

            if (match.Competition.CompetitorType is CompetitorType.PLAYER)
            {
                createPoint(match.CompetitorOne.Id);
                createPoint(match.CompetitorTwo.Id);
            }
            else
            {
                ((Team)match.CompetitorOne).Players.ForEach(player => createPoint(player.Id));
                ((Team)match.CompetitorTwo).Players.ForEach(player => createPoint(player.Id));
            }

            return Task.FromResult(true);
        }
    }
}
