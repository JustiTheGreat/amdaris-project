using AmdarisProject.models.competitor;
using AmdarisProject.repositories.abstractions;
using AmdarisProject.utils;
using MediatR;

namespace AmdarisProject.handlers.competition
{
    public record GetCompetitionFirstPlaceCompetitors(ulong CompetitionId) : IRequest<IEnumerable<Competitor>>;
    public class GetCompetitionFirstPlaceCompetitorsHandler(ICompetitionRepository competitionRepository, IMatchRepository matchRepository,
        ICompetitorRepository competitorRepository, IPointRepository pointRepository)
        : IRequestHandler<GetCompetitionFirstPlaceCompetitors, IEnumerable<Competitor>>
    {
        private readonly ICompetitionRepository _competitionRepository = competitionRepository;
        private readonly IMatchRepository _matchRepository = matchRepository;
        private readonly ICompetitorRepository _competitorRepository = competitorRepository;
        private readonly IPointRepository _pointRepository = pointRepository;

        public Task<IEnumerable<Competitor>> Handle(GetCompetitionFirstPlaceCompetitors request, CancellationToken cancellationToken)
        {
            IEnumerable<Competitor> firstPlaceCompetitors = Utils.GetCompetitionFirstPlaceCompetitorsUtil(_competitionRepository, _matchRepository,
                _competitorRepository, _pointRepository, request.CompetitionId);
            return Task.FromResult(firstPlaceCompetitors);
        }
    }
}
