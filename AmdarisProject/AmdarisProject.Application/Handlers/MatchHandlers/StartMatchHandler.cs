using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Dtos.CreateDTOs;
using AmdarisProject.Application.Dtos.ResponseDTOs;
using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models;
using AmdarisProject.Domain.Models.CompetitorModels;
using MapsterMapper;
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

            if (match.Competition.Status is not CompetitionStatus.STARTED)
                throw new APIllegalStatusException(match.Competition.Status);

            bool anotherMatchIsBeingPlayed = match.Competition.Matches.Any(match => match.Status is MatchStatus.STARTED);

            if (anotherMatchIsBeingPlayed)
                throw new AmdarisProjectException("Cannot start a match while another one is being played!");

            if (match.Status is not MatchStatus.NOT_STARTED)
                throw new APIllegalStatusException(match.Status);

            if (match.Competition.GameFormat.CompetitorType is CompetitorType.TEAM)
            {
                bool teamOneHasTheRequiredNumberOfCompetitors = await _unitOfWork.TeamPlayerRepository
                    .TeamHasTheRequiredNumberOfActivePlayers(match.CompetitorOne.Id, (ushort)match.Competition.GameFormat.TeamSize!);
                bool teamTwoHasTheRequiredNumberOfCompetitors = await _unitOfWork.TeamPlayerRepository
                    .TeamHasTheRequiredNumberOfActivePlayers(match.CompetitorTwo.Id, (ushort)match.Competition.GameFormat.TeamSize!);

                match.Status =
                    teamOneHasTheRequiredNumberOfCompetitors && teamTwoHasTheRequiredNumberOfCompetitors ? MatchStatus.STARTED
                    : !teamOneHasTheRequiredNumberOfCompetitors && teamTwoHasTheRequiredNumberOfCompetitors ? MatchStatus.SPECIAL_WIN_COMPETITOR_TWO
                    : teamOneHasTheRequiredNumberOfCompetitors && !teamTwoHasTheRequiredNumberOfCompetitors ? MatchStatus.SPECIAL_WIN_COMPETITOR_ONE
                    : MatchStatus.CANCELED;
            }
            else match.Status = MatchStatus.STARTED;

            Match updated;

            try
            {
                await _unitOfWork.BeginTransactionAsync();

                if (match.Status is MatchStatus.STARTED)
                {
                    DateTime now = DateTime.UtcNow;
                    bool lateStart = match.StartTime is not null && now > match.StartTime;
                    match.StartTime = now;
                    match.CompetitorOnePoints = 0;
                    match.CompetitorTwoPoints = 0;

                    updated = await _unitOfWork.MatchRepository.Update(match);

                    await InitializePointsForMatchPlayers(match);

                    if (lateStart) await UpdateUnstartedMatchesStartTimes(match);
                }
                else updated = await _unitOfWork.MatchRepository.Update(match);

                await _unitOfWork.SaveAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }

            updated = await _unitOfWork.MatchRepository.GetById(match.Id)
                ?? throw new APNotFoundException(Tuple.Create(nameof(request.MatchId), match.Id));

            _logger.LogInformation("Match {CompetitorOneName}-{CompetitorTwoName} has started!",
                [match.CompetitorOne.Name, match.CompetitorTwo.Name]);

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

            async Task CreatePointsForTeamPlayers(Team team)
            {
                foreach (Player player in team.Players)
                    await CreatePoint(player);
            }

            if (match.Competition.GameFormat.CompetitorType is CompetitorType.PLAYER)
            {
                await CreatePoint((Player)match.CompetitorOne);
                await CreatePoint((Player)match.CompetitorTwo);
            }
            else
            {
                await CreatePointsForTeamPlayers((Team)match.CompetitorOne);
                await CreatePointsForTeamPlayers((Team)match.CompetitorTwo);
            }
        }

        private async Task UpdateUnstartedMatchesStartTimes(Match match)
        {
            IEnumerable<Match> matches = await _unitOfWork.MatchRepository
                .GetNotStartedByCompetitionOrderedByStartTime(match.Competition.Id);
            int i = 0;

            foreach (Match m in matches)
            {
                m.StartTime = ((DateTime)match.StartTime!).AddSeconds(
                        (ulong)++i * (match.Competition.GameFormat.DurationInMinutes! + match.Competition.BreakInMinutes ?? 0));
                await _unitOfWork.MatchRepository.Update(m);
            }

            _logger.LogInformation("Updated the starting time for the rest of the not started matches (Count = {Count})!",
                    [matches.Count()]);
        }
    }
}
