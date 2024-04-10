using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Dtos.ResponseDTOs;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models;
using Mapster;
using MediatR;

namespace AmdarisProject.handlers.point
{
    public record GetPointByPlayerAndMatch(ulong PlayerId, ulong MatchId) : IRequest<PointResponseDTO>;
    public class GetPointByPlayerAndMatchHandler(IUnitOfWork unitOfWork)
        : IRequestHandler<GetPointByPlayerAndMatch, PointResponseDTO>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<PointResponseDTO> Handle(GetPointByPlayerAndMatch request, CancellationToken cancellationToken)
        {
            Point point = await _unitOfWork.PointRepository.GetByPlayerAndMatch(request.PlayerId, request.MatchId)
                ?? throw new APNotFoundException(
                    [Tuple.Create(nameof(request.PlayerId), request.PlayerId), 
                    Tuple.Create(nameof(request.MatchId), request.MatchId)]);

            PointResponseDTO response = point.Adapt<PointResponseDTO>();
            return response;
        }
    }
}
