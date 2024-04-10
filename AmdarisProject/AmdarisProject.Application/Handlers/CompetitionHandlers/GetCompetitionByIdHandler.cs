using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Dtos.ResponseDTOs.CompetitionResponseDTOs;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models.CompetitionModels;
using Mapster;
using MediatR;

namespace AmdarisProject.handlers.competition
{
    public record GetCompetitionById(ulong CompetitionId) : IRequest<CompetitionResponseDTO>;
    public class GetCompetitionByIdHandler(IUnitOfWork unitOfWork)
        : IRequestHandler<GetCompetitionById, CompetitionResponseDTO>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<CompetitionResponseDTO> Handle(GetCompetitionById request, CancellationToken cancellationToken)
        {
            Competition competition = await _unitOfWork.CompetitionRepository.GetById(request.CompetitionId)
                ?? throw new APNotFoundException(Tuple.Create(nameof(request.CompetitionId), request.CompetitionId));

            CompetitionResponseDTO response = competition.Adapt<CompetitionResponseDTO>();
            return response;
        }
    }
}
