using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Services.CompetitionMatchCreatorServices;
using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models;
using AmdarisProject.Domain.Models.CompetitorModels;

namespace AmdarisProject.Application.Services
{
    public class EndMatchService(IUnitOfWork unitOfWork, ICompetitionMatchCreatorFactoryService competitionMatchCreatorFactoryService)
        : IEndMatchService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ICompetitionMatchCreatorFactoryService _competitionMatchCreatorFactoryService = competitionMatchCreatorFactoryService;

        public async Task<Match> End(Guid matchId, MatchStatus endStatus)
        {
            if (endStatus is not MatchStatus.FINISHED
                    or MatchStatus.SPECIAL_WIN_COMPETITOR_ONE
                    or MatchStatus.SPECIAL_WIN_COMPETITOR_TWO)
                throw new APIllegalStatusException(endStatus);

            Match match = await _unitOfWork.MatchRepository.GetById(matchId)
                ?? throw new APNotFoundException(Tuple.Create(nameof(matchId), matchId));

            if (match.Status is not MatchStatus.STARTED)
                throw new APIllegalStatusException(match.Status);

            if (match.CompetitorOnePoints is null || match.CompetitorTwoPoints is null || match.Competition.WinAt is null)
                throw new AmdarisProjectException("Cannot end this type of match!");

            bool matchHasACompetitorWithTheWinningScore = match.CompetitorOnePoints == match.Competition.WinAt
                || match.CompetitorTwoPoints == match.Competition.WinAt;

            if (endStatus is MatchStatus.FINISHED && !matchHasACompetitorWithTheWinningScore)
                throw new AmdarisProjectException($"Match {match.Id} doesn't have a competitor with the winning number of points!");

            match.Status = endStatus;
            match.EndTime = DateTime.Now;
            match.Winner = GetMatchWinner(match);
            Match updated = await _unitOfWork.MatchRepository.Update(match);

            await _competitionMatchCreatorFactoryService
                .GetCompetitionMatchCreatorService(updated.Competition.GetType())
                .CreateCompetitionMatches(updated.Competition.Id);

            updated = await _unitOfWork.MatchRepository.GetById(updated.Id)
                ?? throw new APNotFoundException(Tuple.Create(nameof(updated.Id), updated.Id));

            //TODO remove
            Console.WriteLine($"Competition {match.Competition.Name}: Match between " +
                $"{match.CompetitorOne.Name} and {match.CompetitorTwo.Name} now has status {match.Status} and score " +
                $"{updated.CompetitorOnePoints}-{updated.CompetitorTwoPoints}!");
            //

            return updated;
        }

        private static Competitor? GetMatchWinner(Match match)
            => match.Status is MatchStatus.SPECIAL_WIN_COMPETITOR_ONE ? match.CompetitorOne
            : match.Status is MatchStatus.SPECIAL_WIN_COMPETITOR_TWO ? match.CompetitorTwo
            : match.Status is MatchStatus.FINISHED ?
                (match.CompetitorOnePoints > match.CompetitorTwoPoints ? match.CompetitorOne
                : match.CompetitorOnePoints < match.CompetitorTwoPoints ? match.CompetitorTwo
                : null)
            : throw new APIllegalStatusException(match.Status);
    }
}
