using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Dtos.ResponseDTOs;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models;
using Mapster;
using MediatR;

namespace AmdarisProject.Application.Handlers.MatchHandlers
{
    public record GetMatchById(ulong MatchId) : IRequest<MatchResponseDTO>;
    public class GetMatchByIdHandler(IUnitOfWork unitOfWork)
        : IRequestHandler<GetMatchById, MatchResponseDTO>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<MatchResponseDTO> Handle(GetMatchById request, CancellationToken cancellationToken)
        {
            Match match = await _unitOfWork.MatchRepository.GetById(request.MatchId)
                ?? throw new APNotFoundException(Tuple.Create(nameof(request.MatchId), request.MatchId));

            MatchResponseDTO response = match.Adapt<MatchResponseDTO>();
            return response;
        }
    }
}
