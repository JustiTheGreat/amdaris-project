using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Dtos.ResponseDTOs;
using MediatR;

namespace AmdarisProject.handlers.competition
{
    public record GetCompetitionRanking(Guid CompetitionId) : IRequest<IEnumerable<RankingItemDTO>>;
    public class GetCompetitionRankingHandler(ICompetitionRankingService competitionRankingService)
        : IRequestHandler<GetCompetitionRanking, IEnumerable<RankingItemDTO>>
    {
        private readonly ICompetitionRankingService _competitionRankingService = competitionRankingService;

        public async Task<IEnumerable<RankingItemDTO>> Handle(GetCompetitionRanking request, CancellationToken cancellationToken)
        {
            IEnumerable<RankingItemDTO> ranking = await _competitionRankingService.GetCompetitionRanking(request.CompetitionId);
            return ranking;
        }
    }
}
