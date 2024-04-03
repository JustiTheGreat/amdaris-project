using AmdarisProject.models;
using AmdarisProject.repositories.abstractions;
using AmdarisProject.utils;
using MediatR;

namespace AmdarisProject.handlers.competition
{
    public record CreateCompetitionMatches(ulong CompetitionId) : IRequest<IEnumerable<Match>>;
    public class CreateCompetitionMatchesHandler(ICompetitionRepository competitionRepository, IMatchRepository matchRepository,
        ICompetitorRepository competitorRepository, IPointRepository pointRepository, IStageRepository stageRepository)
        : IRequestHandler<CreateCompetitionMatches, IEnumerable<Match>>
    {
        private readonly ICompetitionRepository _competitionRepository = competitionRepository;
        private readonly IMatchRepository _matchRepository = matchRepository;
        private readonly ICompetitorRepository _competitorRepository = competitorRepository;
        private readonly IPointRepository _pointRepository = pointRepository;
        private readonly IStageRepository _stageRepository = stageRepository;

        public Task<IEnumerable<Match>> Handle(CreateCompetitionMatches request, CancellationToken cancellationToken)
        {
            //TODO CreateBonusMatches
            IEnumerable<Match> createdMatches = Utils.CreateCompetitionMatchesUtil(_competitionRepository, _matchRepository,
                _competitorRepository, _pointRepository, _stageRepository, request.CompetitionId);
            return Task.FromResult(createdMatches);
        }
    }
}
