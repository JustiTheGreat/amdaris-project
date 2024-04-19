using AmdarisProject.Domain.Models;

namespace AmdarisProject.Application.Services.CompetitionMatchCreatorFactoryService.MatchCreators
{
    public interface ICompetitionMatchCreator
    {
        Task<IEnumerable<Match>> CreateCompetitionMatches(Guid competitionId);
    }
}
