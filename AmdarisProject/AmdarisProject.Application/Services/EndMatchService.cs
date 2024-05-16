using AmdarisProject.Application.Abstractions;
using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models;
using Microsoft.Extensions.Logging;

namespace AmdarisProject.Application.Services
{
    public class EndMatchService(IUnitOfWork unitOfWork, ICompetitionMatchCreatorFactoryService competitionMatchCreatorFactoryService,
        ILogger<EndMatchService> logger)
        : IEndMatchService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ICompetitionMatchCreatorFactoryService _competitionMatchCreatorFactoryService = competitionMatchCreatorFactoryService;
        private readonly ILogger<EndMatchService> _logger = logger;

        public async Task<Match> End(Guid matchId, MatchStatus endStatus)
        {
            if (endStatus is not MatchStatus.FINISHED
                    && endStatus is not MatchStatus.SPECIAL_WIN_COMPETITOR_ONE
                    && endStatus is not MatchStatus.SPECIAL_WIN_COMPETITOR_TWO)
                throw new APIllegalStatusException(endStatus);

            Match match = await _unitOfWork.MatchRepository.GetById(matchId)
                ?? throw new APNotFoundException(Tuple.Create(nameof(matchId), matchId));

            if (match.Status is not MatchStatus.STARTED)
                throw new APIllegalStatusException(match.Status);

            if (match.CompetitorOnePoints is null || match.CompetitorTwoPoints is null || match.Competition.GameFormat.WinAt is null)
                throw new AmdarisProjectException("Cannot end this type of match!");

            bool matchHasACompetitorWithTheWinningScore = match.CompetitorOnePoints == match.Competition.GameFormat.WinAt
                || match.CompetitorTwoPoints == match.Competition.GameFormat.WinAt;

            if (endStatus is MatchStatus.FINISHED && !matchHasACompetitorWithTheWinningScore)
                throw new AmdarisProjectException($"Match {match.CompetitorOne.Name}-{match.CompetitorTwo.Name} doesn't have a competitor with the winning number of points!");

            match.Status = endStatus;
            match.EndTime = DateTime.UtcNow;
            match.Winner = match.GetWinner();
            Match updated = await _unitOfWork.MatchRepository.Update(match);

            await _competitionMatchCreatorFactoryService
                .GetCompetitionMatchCreator(updated.Competition.GetType())
                .CreateCompetitionMatches(updated.Competition.Id);

            //TODO automatically end the competition?

            updated = await _unitOfWork.MatchRepository.GetById(updated.Id)
                ?? throw new APNotFoundException(Tuple.Create(nameof(updated.Id), updated.Id));

            _logger.LogInformation("Match {CompetitorOneName}-{CompetitorTwoName} ended with status {Status}!",
                [updated.CompetitorOne.Name, updated.CompetitorTwo.Name, match.Status]);

            return updated;
        }
    }
}
