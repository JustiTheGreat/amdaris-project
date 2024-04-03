using AmdarisProject.models;
using AmdarisProject.repositories.abstractions;
using MediatR;

namespace AmdarisProject.handlers.point
{
    public record GetPointByPlayerAndMatch(ulong PlayerId, ulong MatchId) : IRequest<Point>;
    public class GetPointByPlayerAndMatchHandler(IPointRepository pointRepository)
        : IRequestHandler<GetPointByPlayerAndMatch, Point>
    {
        private readonly IPointRepository _pointRepository = pointRepository;

        public Task<Point> Handle(GetPointByPlayerAndMatch request, CancellationToken cancellationToken)
        {
            Point point = _pointRepository.GetByPlayerAndMatch(request.PlayerId, request.MatchId);
            return Task.FromResult(point);
        }
    }
}
