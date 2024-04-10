using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Utils;
using MediatR;

namespace AmdarisProject.Application.Handlers.CompetitorHandlers
{
    public record GetCompetitorCompetitionWins(ulong CompetitorId, ulong CompetitionId) : IRequest<uint>;
    public class GetCompetitorCompetitionWinsHandler(IUnitOfWork unitOfWork)
        : IRequestHandler<GetCompetitorCompetitionWins, uint>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<uint> Handle(GetCompetitorCompetitionWins request, CancellationToken cancellationToken)
        {
            uint wins = await HandlerUtils.GetCompetitorCompetitionWinsUtil(_unitOfWork, request.CompetitorId, request.CompetitionId);
            return wins;
        }
    }
}
