using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Dtos.ResponseDTOs;
using AmdarisProject.Application.Utils;
using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models;
using AmdarisProject.Domain.Models.CompetitorModels;
using MapsterMapper;
using MediatR;

namespace AmdarisProject.handlers.point
{
    public record AddToValue(Guid PlayerId, Guid MatchId, ushort PointsToBeAdded) : IRequest<PointResponseDTO>;
    public class AddValueToPointValueHandler(IUnitOfWork unitOfWork, IMapper mapper)
        : IRequestHandler<AddToValue, PointResponseDTO>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<PointResponseDTO> Handle(AddToValue request, CancellationToken cancellationToken)
        {
            Match match = await _unitOfWork.MatchRepository.GetById(request.MatchId)
                ?? throw new APNotFoundException(Tuple.Create(nameof(request.MatchId), request.MatchId));

            if (match.Status is not MatchStatus.STARTED)
                throw new APIllegalStatusException(match.Status);

            if (match.CompetitorOnePoints is null || match.CompetitorTwoPoints is null || match.Competition.WinAt is null)
                throw new AmdarisProjectException("Cannot end this type of match!");

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

                if (PlayerIsPartOfMatchCompetitor(match.Competition.CompetitorType, match.CompetitorOne, request.PlayerId))
                    match.CompetitorOnePoints += request.PointsToBeAdded;
                else if (PlayerIsPartOfMatchCompetitor(match.Competition.CompetitorType, match.CompetitorTwo, request.PlayerId))
                    match.CompetitorTwoPoints += request.PointsToBeAdded;

                await _unitOfWork.MatchRepository.Update(match);
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

        private static bool PlayerIsPartOfMatchCompetitor(CompetitorType competitorType, Competitor competitor, Guid playerId)
            => competitorType is CompetitorType.PLAYER && competitor.Id.Equals(playerId)
            || competitorType is CompetitorType.TEAM && HandlerUtils.TeamContainsPlayer((Team)competitor, playerId);
    }
}