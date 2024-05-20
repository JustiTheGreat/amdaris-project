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
            Match match = await _unitOfWork.MatchRepository.GetById(matchId)
                ?? throw new APNotFoundException(Tuple.Create(nameof(matchId), matchId));

            match.EndMatch(endStatus);

            Match updated = await _unitOfWork.MatchRepository.Update(match);

            await _competitionMatchCreatorFactoryService
                .GetCompetitionMatchCreator(updated.Competition.GetType())
                .CreateCompetitionMatches(updated.Competition.Id);

            //TODO automatically end the competition?

            _logger.LogInformation("Match {CompetitorOneName}-{CompetitorTwoName} ended with status {Status}!",
                [updated.CompetitorOne.Name, updated.CompetitorTwo.Name, match.Status]);

            return updated;
        }
    }
}
