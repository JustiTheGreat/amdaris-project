using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Dtos.ResponseDTOs;
using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models;
using AmdarisProject.Domain.Models.CompetitorModels;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AmdarisProject.Application.Handlers.MatchHandlers
{
    public record StartMatch(Guid MatchId) : IRequest<MatchGetDTO>;
    public class StartMatchHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<StartMatchHandler> logger)
        : IRequestHandler<StartMatch, MatchGetDTO>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<StartMatchHandler> _logger = logger;

        public async Task<MatchGetDTO> Handle(StartMatch request, CancellationToken cancellationToken)
        {
            Match match = await _unitOfWork.MatchRepository.GetById(request.MatchId)
                ?? throw new APNotFoundException(Tuple.Create(nameof(request.MatchId), request.MatchId));

            bool lateStart = match.Start();

            Match updated;

            try
            {
                await _unitOfWork.BeginTransactionAsync();
                updated = await _unitOfWork.MatchRepository.Update(match);

                if (match.Status is MatchStatus.STARTED)
                {
                    await InitializePointsForMatchPlayers(match);

                    if (lateStart) await UpdateUnstartedMatchesStartTimes(match);
                }

                await _unitOfWork.SaveAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }

            _logger.LogInformation("Match {CompetitorOneName}-{CompetitorTwoName} has started!",
                [updated.CompetitorOne.Name, updated.CompetitorTwo.Name]);

            MatchGetDTO response = _mapper.Map<MatchGetDTO>(updated);
            return response;
        }

        private async Task InitializePointsForMatchPlayers(Match match)
        {
            async Task CreatePoint(Player player)
            {
                Point created = await _unitOfWork.PointRepository.Create(new()
                {
                    Value = 0,
                    Player = player,
                    Match = match,
                });

                _logger.LogInformation("Initialized point for player {PlayerName} for match {MatchId} with {Value}!",
                    [created.Player.Name, created.Match.Id, created.Value]);
            }

            async Task CreatePoints(Team team)
            {
                foreach (TeamPlayer teamPlayer in team.TeamPlayers)
                    if (teamPlayer.IsActive)
                        await CreatePoint(teamPlayer.Player);

            }

            if (match.Competition.GameFormat.CompetitorType is CompetitorType.PLAYER)
            {
                await CreatePoint((Player)match.CompetitorOne);
                await CreatePoint((Player)match.CompetitorTwo);
            }
            else
            {
                await CreatePoints((Team)match.CompetitorOne);
                await CreatePoints((Team)match.CompetitorTwo);
            }
        }

        private async Task UpdateUnstartedMatchesStartTimes(Match match)
        {
            IEnumerable<Match> matches = await _unitOfWork.MatchRepository
                .GetNotStartedByCompetitionOrderedByStartTime(match.Competition.Id);
            int i = 0;

            foreach (Match m in matches)
            {
                m.ActualizedStartTime = ((DateTimeOffset)match.ActualizedStartTime!).AddMinutes(
                        (ulong)++i * (match.Competition.GameFormat.DurationInMinutes! + match.Competition.BreakInMinutes ?? 0));
                m.ActualizedEndTime = ((DateTimeOffset)m.ActualizedStartTime).AddMinutes(match.Competition.GameFormat.DurationInMinutes ?? 0);
                await _unitOfWork.MatchRepository.Update(m);
            }

            _logger.LogInformation("Updated the starting time for the rest of the not started matches (Count = {Count})!",
                    [matches.Count()]);
        }
    }
}
