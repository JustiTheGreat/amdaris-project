using AmdarisProject.models;
using AmdarisProject.repositories.abstractions;
using AmdarisProject.utils;
using MediatR;

namespace AmdarisProject.handlers.competitor
{
    public record GetCompetitorCompetitionRating(ulong CompetitorId, ulong CompetitionId) : IRequest<double>;
    public class GetCompetitorCompetitionRatingHandler(IMatchRepository matchRepository, ICompetitorRepository competitorRepository,
        IPointRepository pointRepository)
        : IRequestHandler<GetCompetitorCompetitionRating, double>
    {
        private readonly IMatchRepository _matchRepository = matchRepository;
        private readonly ICompetitorRepository _competitorRepository = competitorRepository;
        private readonly IPointRepository _pointRepository = pointRepository;

        public Task<double> Handle(GetCompetitorCompetitionRating request, CancellationToken cancellationToken)
        {
            IEnumerable<Match> playedMatches = _matchRepository.GetAllByCompetitorAndCompetition(request.CompetitorId, request.CompetitionId);
            double rating = Utils.GetCompetitorWinRatingOfMatchesUtil(_matchRepository, _competitorRepository, _pointRepository,
                playedMatches, request.CompetitorId);
            return Task.FromResult(rating);
        }
    }
}
