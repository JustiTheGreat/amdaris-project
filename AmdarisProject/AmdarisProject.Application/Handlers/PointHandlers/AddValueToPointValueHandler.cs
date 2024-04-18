using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Dtos.ResponseDTOs;
using AmdarisProject.Application.ExtensionMethods;
using AmdarisProject.Application.Services;
using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models;
using MapsterMapper;
using MediatR;

namespace AmdarisProject.handlers.point
{
    public record AddValueToPointValue(Guid PlayerId, Guid MatchId, ushort PointsToBeAdded) : IRequest<PointResponseDTO>;
    public class AddValueToPointValueHandler(IUnitOfWork unitOfWork, IMapper mapper, IEndMatchService endMatchService)
        : IRequestHandler<AddValueToPointValue, PointResponseDTO>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly IEndMatchService _endMatchService = endMatchService;

        public async Task<PointResponseDTO> Handle(AddValueToPointValue request, CancellationToken cancellationToken)
        {
            Match match = await _unitOfWork.MatchRepository.GetById(request.MatchId)
                ?? throw new APNotFoundException(Tuple.Create(nameof(request.MatchId), request.MatchId));

            if (match.Status is not MatchStatus.STARTED)
                throw new APIllegalStatusException(match.Status);

            bool matchHasACompetitorWithTheWinningScore = match.CompetitorOnePoints == match.Competition.WinAt
                || match.CompetitorTwoPoints == match.Competition.WinAt;

            if (matchHasACompetitorWithTheWinningScore)
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

                if (match.CompetitorOne.IsOrContainsCompetitor(request.PlayerId))
                    match.CompetitorOnePoints += request.PointsToBeAdded;
                else if (match.CompetitorTwo.IsOrContainsCompetitor(request.PlayerId))
                    match.CompetitorTwoPoints += request.PointsToBeAdded;

                match = await _unitOfWork.MatchRepository.Update(match);

                if (match.Competition.WinAt != null
                    && (match.CompetitorOnePoints == match.Competition.WinAt || match.CompetitorTwoPoints == match.Competition.WinAt))
                    await _endMatchService.End(request.MatchId, MatchStatus.FINISHED);

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