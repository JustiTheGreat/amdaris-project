using AmdarisProject.dtos;
using AmdarisProject.repositories.abstractions;
using AmdarisProject.utils;
using MediatR;

namespace AmdarisProject.handlers.competition
{
    public record GetCompetitionRanking(ulong CompetitionId) : IRequest<IEnumerable<RankingItem>>;
    public class GetCompetitionRankingHandler(ICompetitionRepository competitionRepository,
        IMatchRepository matchRepository, ICompetitorRepository competitorRepository, IPointRepository pointRepository)
        : IRequestHandler<GetCompetitionRanking, IEnumerable<RankingItem>>
    {
        private readonly ICompetitionRepository _competitionRepository = competitionRepository;
        private readonly IMatchRepository _matchRepository = matchRepository;
        private readonly ICompetitorRepository _competitorRepository = competitorRepository;
        private readonly IPointRepository _pointRepository = pointRepository;

        public Task<IEnumerable<RankingItem>> Handle(GetCompetitionRanking request, CancellationToken cancellationToken)
        {
            IEnumerable<RankingItem> ranking = Utils.GetCompetitionRankingUtil(_competitionRepository, _matchRepository, _competitorRepository,
                _pointRepository, request.CompetitionId);
            return Task.FromResult(ranking);
        }
    }
}
