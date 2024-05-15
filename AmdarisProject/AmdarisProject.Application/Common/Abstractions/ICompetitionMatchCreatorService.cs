using AmdarisProject.Domain.Models;

namespace AmdarisProject.Application.Common.Abstractions
{
    public interface ICompetitionMatchCreatorService
    {
        Task<IEnumerable<Match>> CreateCompetitionMatches(Guid competitionId);
    }
}
