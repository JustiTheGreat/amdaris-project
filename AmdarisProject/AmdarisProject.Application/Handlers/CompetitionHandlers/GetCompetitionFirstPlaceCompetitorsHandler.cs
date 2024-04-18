using AmdarisProject.Application.Dtos.ResponseDTOs.CompetitorResponseDTOs;
using AmdarisProject.Application.Services;
using AmdarisProject.Domain.Models.CompetitorModels;
using Mapster;
using MapsterMapper;
using MediatR;

namespace AmdarisProject.handlers.competition
{
    public record GetCompetitionFirstPlaceCompetitors(Guid CompetitionId) : IRequest<IEnumerable<CompetitorResponseDTO>>;
    public class GetCompetitionFirstPlaceCompetitorsHandler(IMapper mapper, ICompetitionRankingService competitionRankingService)
        : IRequestHandler<GetCompetitionFirstPlaceCompetitors, IEnumerable<CompetitorResponseDTO>>
    {
        private readonly IMapper _mapper = mapper;
        private readonly ICompetitionRankingService _competitionRankingService = competitionRankingService;

        public async Task<IEnumerable<CompetitorResponseDTO>> Handle(GetCompetitionFirstPlaceCompetitors request, CancellationToken cancellationToken)
        {
            IEnumerable<Competitor> firstPlaceCompetitors =
                await _competitionRankingService.GetCompetitionFirstPlaceCompetitors(request.CompetitionId);
            IEnumerable<CompetitorResponseDTO> response = firstPlaceCompetitors.Select(_mapper.Map<CompetitorResponseDTO>).ToList();
            return response;
        }
    }
}
