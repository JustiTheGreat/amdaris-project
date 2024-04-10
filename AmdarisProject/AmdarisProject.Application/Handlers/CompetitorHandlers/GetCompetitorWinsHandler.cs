using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Utils;
using AmdarisProject.Domain.Enums;
using MediatR;

namespace AmdarisProject.Application.Handlers.CompetitorHandlers
{
    public record GetCompetitorWins(ulong CompetitorId, GameType GameType) : IRequest<uint>;
    public class GetCompetitorWinsHandler(IUnitOfWork unitOfWork)
        : IRequestHandler<GetCompetitorWins, uint>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<uint> Handle(GetCompetitorWins request, CancellationToken cancellationToken)
        {
            uint wins = (uint)(await _unitOfWork.MatchRepository.GetAllByCompetitorAndGameType(request.CompetitorId, request.GameType))
                .Count(match => HandlerUtils.CompetitorIsPartOfTheWinningSideUtil(_unitOfWork, match.Id, request.CompetitorId).Result);
            return wins;
        }
    }
}
