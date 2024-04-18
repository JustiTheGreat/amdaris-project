using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Dtos.ResponseDTOs.CompetitorResponseDTOs;
using AmdarisProject.Application.Services;
using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models.CompetitionModels;
using AmdarisProject.Domain.Models.CompetitorModels;
using MapsterMapper;
using MediatR;

namespace AmdarisProject.handlers.competition
{
    public record GetCompetitionWinner(Guid CompetitionId) : IRequest<IEnumerable<CompetitorResponseDTO>>;
    public class GetCompetitionWinnersHandler(IUnitOfWork unitOfWork, IMapper mapper,
        ICompetitionRankingService competitionRankingService)
        : IRequestHandler<GetCompetitionWinner, IEnumerable<CompetitorResponseDTO>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ICompetitionRankingService _competitionRankingService = competitionRankingService;

        public async Task<IEnumerable<CompetitorResponseDTO>> Handle(GetCompetitionWinner request, CancellationToken cancellationToken)
        {
            Competition competition = await _unitOfWork.CompetitionRepository.GetById(request.CompetitionId)
                ?? throw new APNotFoundException(Tuple.Create(nameof(request.CompetitionId), request.CompetitionId));

            if (competition.Status is not CompetitionStatus.FINISHED)
                throw new APIllegalStatusException(competition.Status);

            IEnumerable<Competitor> firstPlaceCompetitors =
                await _competitionRankingService.GetCompetitionFirstPlaceCompetitors(request.CompetitionId);
            IEnumerable<CompetitorResponseDTO> response = firstPlaceCompetitors.Select(_mapper.Map<CompetitorResponseDTO>).ToList();
            return response;
        }
    }
}
