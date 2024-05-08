using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Dtos.ResponseDTOs;
using AmdarisProject.Application.Services;
using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Extensions;
using AmdarisProject.Domain.Models;
using AmdarisProject.Domain.Models.CompetitorModels;
using MapsterMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AmdarisProject.handlers.point
{
    public record AddValueToPointValue(Guid PlayerId, Guid MatchId, uint ScoredPoints) : IRequest<PointGetDTO>;
    public class AddValueToPointValueHandler(IUnitOfWork unitOfWork, IMapper mapper, IEndMatchService endMatchService
        , ILogger<AddValueToPointValueHandler> logger)
        : IRequestHandler<AddValueToPointValue, PointGetDTO>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly IEndMatchService _endMatchService = endMatchService;
        private readonly ILogger<AddValueToPointValueHandler> _logger = logger;

        public async Task<PointGetDTO> Handle(AddValueToPointValue request, CancellationToken cancellationToken)
        {
            Match match = await _unitOfWork.MatchRepository.GetById(request.MatchId)
                ?? throw new APNotFoundException(Tuple.Create(nameof(request.MatchId), request.MatchId));

            if (match.Status is not MatchStatus.STARTED)
                throw new APIllegalStatusException(match.Status);

            bool matchHasACompetitorWithTheWinningScore = match.CompetitorOnePoints == match.Competition.GameFormat.WinAt
                || match.CompetitorTwoPoints == match.Competition.GameFormat.WinAt;

            if (matchHasACompetitorWithTheWinningScore)
                throw new AmdarisProjectException($"A competitor of match {match.Id} already has the winning number of points!");

            Point point = await _unitOfWork.PointRepository.GetByPlayerAndMatch(request.PlayerId, request.MatchId)
                ?? throw new APNotFoundException(
                    [Tuple.Create(nameof(request.PlayerId), request.PlayerId),
                    Tuple.Create(nameof(request.MatchId), request.MatchId)]);

            point.Value += request.ScoredPoints;

            Point updated;

            try
            {
                await _unitOfWork.BeginTransactionAsync();
                updated = await _unitOfWork.PointRepository.Update(point);

                if (match.CompetitorOne.IsOrContainsCompetitor(request.PlayerId))
                    match.CompetitorOnePoints += request.ScoredPoints;
                else if (match.CompetitorTwo.IsOrContainsCompetitor(request.PlayerId))
                    match.CompetitorTwoPoints += request.ScoredPoints;

                match = await _unitOfWork.MatchRepository.Update(match);

                if (match.Competition.GameFormat.WinAt != null
                    && (match.CompetitorOnePoints == match.Competition.GameFormat.WinAt
                        || match.CompetitorTwoPoints == match.Competition.GameFormat.WinAt))
                    await _endMatchService.End(request.MatchId, MatchStatus.FINISHED);

                await _unitOfWork.SaveAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }

            _logger.LogInformation("Player {PlayerName} scored {ScoredPoints} points for match {CompetitorOneName}-{CompetitorTwoName} (TotalPoints = {Value})!\n" +
                "Match score: {CompetitorOnePoints}-{CompetitorTwoPoints}",
                [updated.Player.Name, request.ScoredPoints, updated.Match.CompetitorOne.Name, updated.Match.CompetitorTwo.Name,
                    updated.Value,updated.Match.CompetitorOnePoints,updated.Match.CompetitorTwoPoints]);

            PointGetDTO response = _mapper.Map<PointGetDTO>(updated);
            return response;
        }
    }
}