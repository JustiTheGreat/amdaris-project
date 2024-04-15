using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Utils;
using MediatR;

namespace AmdarisProject.Application.Handlers.CompetitorHandlers
{
    public record GetCompetitorCompetitionPoints(Guid CompetitorId, Guid CompetitionId) : IRequest<uint>;
    public class GetCompetitorCompetitionPointsHandler(IUnitOfWork unitOfWork)
        : IRequestHandler<GetCompetitorCompetitionPoints, uint>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<uint> Handle(GetCompetitorCompetitionPoints request, CancellationToken cancellationToken)
        {
            uint points = await HandlerUtils.GetCompetitorCompetitionPointsUtil(_unitOfWork, request.CompetitorId, request.CompetitionId);
            return points;
        }
    }
}
