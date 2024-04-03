using AmdarisProject.models.competition;
using AmdarisProject.models.competitor;
using AmdarisProject.repositories.abstractions;
using AmdarisProject.utils;
using Domain.Enums;
using Domain.Exceptions;
using MediatR;

namespace AmdarisProject.handlers.competition
{
    public record GetCompetitionWinner(ulong CompetitionId) : IRequest<IEnumerable<Competitor>>;
    public class GetCompetitionWinnerHandler(ICompetitionRepository competitionRepository, IMatchRepository matchRepository,
        ICompetitorRepository competitorRepository, IPointRepository pointRepository)
        : IRequestHandler<GetCompetitionWinner, IEnumerable<Competitor>>
    {
        private readonly ICompetitionRepository _competitionRepository = competitionRepository;
        private readonly IMatchRepository _matchRepository = matchRepository;
        private readonly ICompetitorRepository _competitorRepository = competitorRepository;
        private readonly IPointRepository _pointRepository = pointRepository;

        public Task<IEnumerable<Competitor>> Handle(GetCompetitionWinner request, CancellationToken cancellationToken)
        {
            Competition competition = _competitionRepository.GetById(request.CompetitionId);

            if (competition.Status is not CompetitionStatus.FINISHED)
                throw new APIllegalStatusException(nameof(GetCompetitionWinnerHandler), nameof(Handle), competition.Status.ToString());

            IEnumerable<Competitor> firstPlaceCompetitors = Utils.GetCompetitionFirstPlaceCompetitorsUtil(_competitionRepository, _matchRepository,
                _competitorRepository, _pointRepository, request.CompetitionId);

            return Task.FromResult(firstPlaceCompetitors);
        }
    }
}
