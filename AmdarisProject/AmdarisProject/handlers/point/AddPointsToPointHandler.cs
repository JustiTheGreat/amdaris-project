using AmdarisProject.models;
using AmdarisProject.repositories.abstractions;
using AmdarisProject.utils;
using AmdarisProject.utils.exceptions;
using AmdarisProject.utils.Exceptions;
using MediatR;

namespace AmdarisProject.handlers.point
{
    public record AddPointsToPoint(ulong PlayerId, ulong MatchId, ushort PointsToBeAdded) : IRequest<Point>;
    public class AddPointsToPointHandler(IPointRepository pointRepository, IMatchRepository matchRepository,
        ICompetitorRepository competitorRepository)
        : IRequestHandler<AddPointsToPoint, Point>
    {
        private readonly IPointRepository _pointRepository = pointRepository;
        private readonly IMatchRepository _matchRepository = matchRepository;
        private readonly ICompetitorRepository _competitorRepository = competitorRepository;

        public Task<Point> Handle(AddPointsToPoint request, CancellationToken cancellationToken)
        {
            Match match = _matchRepository.GetById(request.MatchId);

            if (match.Status is not utils.enums.MatchStatus.STARTED)
                throw new APIllegalStatusException(nameof(AddPointsToPointHandler), nameof(Handle), match.Status.ToString());

            if (Utils.MatchHasACompetitorWithTheWinningScoreUtil(_matchRepository, _competitorRepository, _pointRepository, match.Id))
                throw new APPointsException(nameof(AddPointsToPointHandler), nameof(Handle),
                    $"A competitor of match {match.Id} already has the winning score!");

            Point point = _pointRepository.GetByPlayerAndMatch(request.MatchId, request.PlayerId);
            point.Value += request.PointsToBeAdded;
            Point updated = _pointRepository.Update(point);
            return Task.FromResult(updated);
        }
    }
}