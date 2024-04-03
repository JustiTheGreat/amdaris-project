using AmdarisProject.repositories.abstractions;
using AmdarisProject.utils;
using MediatR;

namespace AmdarisProject.handlers.competitor
{
    public record GetCompetitorCompetitionWins(ulong CompetitorId, ulong CompetitionId) : IRequest<uint>;
    public class GetCompetitorCompetitionWinsHandler(IMatchRepository matchRepository, ICompetitorRepository competitorRepository,
        IPointRepository pointRepository, ICompetitionRepository competitionRepository)
        : IRequestHandler<GetCompetitorCompetitionWins, uint>
    {
        private readonly IMatchRepository _matchRepository = matchRepository;
        private readonly ICompetitorRepository _competitorRepository = competitorRepository;
        private readonly IPointRepository _pointRepository = pointRepository;
        private readonly ICompetitionRepository _competitionRepository = competitionRepository;

        public Task<uint> Handle(GetCompetitorCompetitionWins request, CancellationToken cancellationToken)
        {
            uint wins = Utils.GetCompetitorCompetitionWinsUtil(_matchRepository, _competitorRepository, _pointRepository,
                _competitionRepository, request.CompetitorId, request.CompetitionId);
            return Task.FromResult(wins);
        }
    }
}
