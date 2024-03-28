using AmdarisProject.models;
using AmdarisProject.models.competitor;
using AmdarisProject.repositories.abstractions;
using AmdarisProject.utils;
using AmdarisProject.utils.enums;
using AmdarisProject.utils.Exceptions;
using MediatR;

namespace AmdarisProject.handlers.match
{
    public record GetMatchWinner(ulong MatchId) : IRequest<Competitor?>;
    public class GetMatchWinnerHandler(IMatchRepository matchRepository, ICompetitorRepository competitorRepository,
            IPointRepository pointRepository)
        : IRequestHandler<GetMatchWinner, Competitor?>
    {
        private readonly IMatchRepository _matchRepository = matchRepository;
        private readonly ICompetitorRepository _competitorRepository = competitorRepository;
        private readonly IPointRepository _pointRepository = pointRepository;

        public Task<Competitor?> Handle(GetMatchWinner request, CancellationToken cancellationToken)
        {
            Match match = _matchRepository.GetById(request.MatchId);

            if (match.Status is MatchStatus.NOT_STARTED or MatchStatus.STARTED or MatchStatus.CANCELED)
                throw new APIllegalStatusException(nameof(GetMatchWinnerHandler), nameof(Handle), match.Status.ToString());

            Competitor? winner = null;

            if (match.Status is MatchStatus.SPECIAL_WIN_COMPETITOR_ONE)
                winner = match.CompetitorOne;
            else if (match.Status is MatchStatus.SPECIAL_WIN_COMPETITOR_TWO)
                winner = match.CompetitorTwo;
            else
            {
                uint pointsCompetitorOne = Utils.GetCompetitorMatchPointsUtil(_matchRepository, _competitorRepository, _pointRepository,
                    match.Id, match.CompetitorOne.Id);
                uint pointsCompetitorTwo = Utils.GetCompetitorMatchPointsUtil(_matchRepository, _competitorRepository, _pointRepository,
                    match.Id, match.CompetitorTwo.Id);
                winner = pointsCompetitorOne == pointsCompetitorTwo ? null
                    : pointsCompetitorOne > pointsCompetitorTwo ? match.CompetitorOne
                    : match.CompetitorTwo;
            }

            return Task.FromResult(winner);
        }
    }
}
