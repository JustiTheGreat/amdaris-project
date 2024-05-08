using AmdarisProject.Domain.Models;

namespace AmdarisProject.Application.Services.CompetitionMatchCreatorFactoryService.MatchCreatorService
{
    public interface ICompetitionMatchCreatorService
    {
        Task<IEnumerable<Match>> CreateCompetitionMatches(Guid competitionId);
    }
}
