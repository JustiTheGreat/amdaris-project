using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Dtos.ResponseDTOs.CompetitorResponseDTOs;
using AmdarisProject.Application.Utils;
using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models.CompetitionModels;
using AmdarisProject.Domain.Models.CompetitorModels;
using Mapster;
using MediatR;

namespace AmdarisProject.handlers.competition
{
    public record GetCompetitionWinner(ulong CompetitionId) : IRequest<IEnumerable<CompetitorResponseDTO>>;
    public class GetCompetitionWinnersHandler(IUnitOfWork unitOfWork)
        : IRequestHandler<GetCompetitionWinner, IEnumerable<CompetitorResponseDTO>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<IEnumerable<CompetitorResponseDTO>> Handle(GetCompetitionWinner request, CancellationToken cancellationToken)
        {
            Competition competition = await _unitOfWork.CompetitionRepository.GetById(request.CompetitionId)
                ?? throw new APNotFoundException(nameof(StartCompetitionHandler), nameof(Handle),
                    [Tuple.Create(nameof(request.CompetitionId), request.CompetitionId)]);

            if (competition.Status is not CompetitionStatus.FINISHED)
                throw new APIllegalStatusException(nameof(GetCompetitionWinnersHandler), nameof(Handle), competition.Status);

            IEnumerable<Competitor> firstPlaceCompetitors =
                await HandlerUtils.GetCompetitionFirstPlaceCompetitorsUtil(_unitOfWork, request.CompetitionId);
            IEnumerable<CompetitorResponseDTO> response =
                firstPlaceCompetitors.Select(competitor => competitor.Adapt<CompetitorResponseDTO>()).ToList();
            return response;
        }
    }
}
