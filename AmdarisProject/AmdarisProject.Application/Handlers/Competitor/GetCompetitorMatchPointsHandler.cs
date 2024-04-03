using AmdarisProject.repositories.abstractions;
using AmdarisProject.utils;
using MediatR;

namespace AmdarisProject.handlers.competitor
{
    public record GetCompetitorMatchPoints(ulong CompetitorId, ulong MatchId) : IRequest<uint>;
    public class GetCompetitorMatchPointsHandler(ICompetitorRepository competitorRepository,
        IPointRepository pointRepository, IMatchRepository matchRepository)
        : IRequestHandler<GetCompetitorMatchPoints, uint>
    {
        private readonly ICompetitorRepository _competitorRepository = competitorRepository;
        private readonly IPointRepository _pointRepository = pointRepository;
        private readonly IMatchRepository _matchRepository = matchRepository;

        public Task<uint> Handle(GetCompetitorMatchPoints request, CancellationToken cancellationToken)
        {
            uint points = Utils.GetCompetitorMatchPointsUtil(_matchRepository, _competitorRepository, _pointRepository,
                request.MatchId, request.CompetitorId);
            return Task.FromResult(points);
        }
    }
}
