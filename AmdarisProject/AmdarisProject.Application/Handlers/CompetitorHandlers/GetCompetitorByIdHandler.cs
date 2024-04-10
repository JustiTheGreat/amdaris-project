using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Dtos.ResponseDTOs.CompetitorResponseDTOs;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models.CompetitorModels;
using Mapster;
using MediatR;

namespace AmdarisProject.Application.Handlers.CompetitorHandlers
{
    public record GetCompetitorById(ulong CompetitorId) : IRequest<CompetitorResponseDTO>;
    public class GetCompetitorByIdHandler(IUnitOfWork unitOfWork)
        : IRequestHandler<GetCompetitorById, CompetitorResponseDTO>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<CompetitorResponseDTO> Handle(GetCompetitorById request, CancellationToken cancellationToken)
        {
            Competitor competitor = await _unitOfWork.CompetitorRepository.GetById(request.CompetitorId)
                ?? throw new APNotFoundException(Tuple.Create(nameof(request.CompetitorId), request.CompetitorId));

            CompetitorResponseDTO response = competitor.Adapt<CompetitorResponseDTO>();
            return response;
        }
    }
}
