using AmdarisProject.models;
using AmdarisProject.repositories.abstractions;
using AmdarisProject.utils;
using AmdarisProject.utils.enums;
using MediatR;

namespace AmdarisProject.handlers.competitor
{
    public record GetCompetitorRating(ulong CompetitorId, GameType GameType) : IRequest<double>;
    public class GetCompetitiorRatingHandler(IMatchRepository matchRepository, ICompetitorRepository competitorRepository,
        IPointRepository pointRepository)
        : IRequestHandler<GetCompetitorRating, double>
    {
        private readonly IMatchRepository _matchRepository = matchRepository;
        private readonly ICompetitorRepository _competitorRepository = competitorRepository;
        private readonly IPointRepository _pointRepository = pointRepository;

        public Task<double> Handle(GetCompetitorRating request, CancellationToken cancellationToken)
        {
            IEnumerable<Match> playedMatches = _matchRepository.GetAllByCompetitorAndGameType(request.CompetitorId, request.GameType);
            double rating = Utils.GetCompetitorWinRatingOfMatchesUtil(_matchRepository, _competitorRepository, _pointRepository,
                playedMatches, request.CompetitorId);
            return Task.FromResult(rating);
        }
    }
}
