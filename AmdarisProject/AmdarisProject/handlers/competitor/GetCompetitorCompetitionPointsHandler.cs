using AmdarisProject.repositories.abstractions;
using AmdarisProject.utils;
using MediatR;

namespace AmdarisProject.handlers.competitor
{
    public record GetCompetitorCompetitionPoints(ulong CompetitorId, ulong CompetitionId) : IRequest<uint>;
    public class GetCompetitorCompetitionPointsHandler(ICompetitionRepository competitionRepository,
        IMatchRepository matchRepository, ICompetitorRepository competitorRepository, IPointRepository pointRepository)
        : IRequestHandler<GetCompetitorCompetitionPoints, uint>
    {
        private readonly ICompetitionRepository _competitionRepository = competitionRepository;
        private readonly IMatchRepository _matchRepository = matchRepository;
        private readonly ICompetitorRepository _competitorRepository = competitorRepository;
        private readonly IPointRepository _pointRepository = pointRepository;

        public Task<uint> Handle(GetCompetitorCompetitionPoints request, CancellationToken cancellationToken)
        {
            uint points = Utils.GetCompetitorCompetitionPointsUtil(_matchRepository, _competitorRepository, _pointRepository,
                _competitionRepository, request.CompetitorId, request.CompetitionId);
            return Task.FromResult(points);
        }
    }
}
