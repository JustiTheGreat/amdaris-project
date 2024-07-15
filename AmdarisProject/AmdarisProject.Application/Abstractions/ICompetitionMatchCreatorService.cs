using AmdarisProject.Domain.Models;

namespace AmdarisProject.Application.Abstractions
{
    public interface ICompetitionMatchCreatorService
    {
        Task<IEnumerable<Match>> CreateCompetitionMatches(Guid competitionId);
    }
}
