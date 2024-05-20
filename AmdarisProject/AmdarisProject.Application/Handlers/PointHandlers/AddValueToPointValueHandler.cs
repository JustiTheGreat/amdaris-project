using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Dtos.ResponseDTOs;
using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AmdarisProject.handlers.point
{
    public record AddValueToPointValue(Guid MatchId, Guid PlayerId, uint ScoredPoints) : IRequest<PointGetDTO>;
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
            Point point = await _unitOfWork.PointRepository.GetByMatchAndPlayer(request.MatchId, request.PlayerId)
                ?? throw new APNotFoundException(
                    [Tuple.Create(nameof(request.MatchId), request.MatchId),
                    Tuple.Create(nameof(request.PlayerId), request.PlayerId)]);

            point.AddValue(request.ScoredPoints);

            Point updated;

            try
            {
                await _unitOfWork.BeginTransactionAsync();
                updated = await _unitOfWork.PointRepository.Update(point);

                if (point.Match.ACompetitorHasTheWinningScore())
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