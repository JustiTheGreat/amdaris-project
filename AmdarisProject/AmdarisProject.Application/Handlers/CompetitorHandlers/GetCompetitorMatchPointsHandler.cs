using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Utils;
using MediatR;

namespace AmdarisProject.Application.Handlers.CompetitorHandlers
{
    public record GetCompetitorMatchPoints(Guid CompetitorId, Guid MatchId) : IRequest<uint>;
    public class GetCompetitorMatchPointsHandler(IUnitOfWork unitOfWork)
        : IRequestHandler<GetCompetitorMatchPoints, uint>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<uint> Handle(GetCompetitorMatchPoints request, CancellationToken cancellationToken)
        {
            uint points = await HandlerUtils.GetCompetitorMatchPointsUtil(_unitOfWork, request.MatchId, request.CompetitorId);
            return points;
        }
    }
}
