using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Dtos.ResponseDTOs;
using AmdarisProject.Application.Utils;
using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models;
using MapsterMapper;
using MediatR;

namespace AmdarisProject.handlers.point
{
    public record AddPointsToPoint(ulong PlayerId, ulong MatchId, ushort PointsToBeAdded) : IRequest<PointResponseDTO>;
    public class AddPointsToPointHandler(IUnitOfWork unitOfWork, IMapper mapper)
        : IRequestHandler<AddPointsToPoint, PointResponseDTO>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<PointResponseDTO> Handle(AddPointsToPoint request, CancellationToken cancellationToken)
        {
            Match match = await _unitOfWork.MatchRepository.GetById(request.MatchId)
                ?? throw new APNotFoundException(Tuple.Create(nameof(request.MatchId), request.MatchId));

            if (match.Status is not MatchStatus.STARTED)
                throw new APIllegalStatusException(match.Status);

            if (await HandlerUtils.MatchHasACompetitorWithTheWinningScoreUtil(_unitOfWork, match.Id))
                throw new AmdarisProjectException($"A competitor of match {match.Id} already has the winning number of points!");

            Point point = await _unitOfWork.PointRepository.GetByPlayerAndMatch(request.PlayerId, request.MatchId)
                ?? throw new APNotFoundException(
                    [Tuple.Create(nameof(request.PlayerId), request.PlayerId),
                    Tuple.Create(nameof(request.MatchId), request.MatchId)]);

            point.Value += request.PointsToBeAdded;

            Point updated;

            try
            {
                await _unitOfWork.BeginTransactionAsync();
                updated = await _unitOfWork.PointRepository.Update(point);
                await _unitOfWork.SaveAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }

            PointResponseDTO response = _mapper.Map<PointResponseDTO>(updated);
            return response;
        }
    }
}