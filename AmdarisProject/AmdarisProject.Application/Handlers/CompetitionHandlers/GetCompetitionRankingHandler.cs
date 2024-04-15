using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Dtos;
using AmdarisProject.Application.Utils;
using MediatR;

namespace AmdarisProject.handlers.competition
{
    public record GetCompetitionRanking(Guid CompetitionId) : IRequest<IEnumerable<RankingItemDTO>>;
    public class GetCompetitionRankingHandler(IUnitOfWork unitOfWork)
        : IRequestHandler<GetCompetitionRanking, IEnumerable<RankingItemDTO>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<IEnumerable<RankingItemDTO>> Handle(GetCompetitionRanking request, CancellationToken cancellationToken)
        {
            IEnumerable<RankingItemDTO> ranking = await HandlerUtils.GetCompetitionRankingUtil(_unitOfWork, request.CompetitionId);
            return ranking;
        }
    }
}
