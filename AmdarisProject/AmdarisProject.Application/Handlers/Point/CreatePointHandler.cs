using AmdarisProject.models;
using AmdarisProject.models.competitor;
using AmdarisProject.repositories.abstractions;
using AmdarisProject.utils;
using Domain.Exceptions;
using MediatR;

namespace AmdarisProject.handlers.point
{
    public record CreatePoint(ushort Value, ulong MatchId, ulong PlayerId) : IRequest<Point>;
    public class CreatePointHandler(IPointRepository pointRepository, IMatchRepository matchRepository,
        ICompetitorRepository competitorRepository)
        : IRequestHandler<CreatePoint, Point>
    {
        private readonly IPointRepository _pointRepository = pointRepository;
        private readonly IMatchRepository _matchRepository = matchRepository;
        private readonly ICompetitorRepository _competitorRepository = competitorRepository;

        public Task<Point> Handle(CreatePoint request, CancellationToken cancellationToken)
        {
            try
            {
                _pointRepository.GetByPlayerAndMatch(request.PlayerId, request.MatchId);
                throw new AmdarisProjectException(nameof(CreatePointHandler), nameof(Handle),
                    $"Point for player {request.PlayerId} and match {request.MatchId} already exists!");
            }
            catch (APNotFoundException)
            {
            }

            Match match = _matchRepository.GetById(request.MatchId);

            if (!Utils.MatchContainsCompetitor(match, request.PlayerId))
                throw new APCompetitorException(nameof(CreatePointHandler), nameof(Handle),
                    $"Competitor {request.PlayerId} not in match {match.Id}!");

            Player player = (Player)_competitorRepository.GetById(request.PlayerId);
            Point point = new(request.Value, match, player);
            Point created = _pointRepository.Create(point);
            return Task.FromResult(created);
        }
    }
}
