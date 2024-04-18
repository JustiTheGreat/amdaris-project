using AmdarisProject.Domain.Models;

namespace AmdarisProject.Application.Services
{
    public interface ICreateCompetitionMatchesService
    {
        Task<IEnumerable<Match>> CreateCompetitionMatches(Guid competitionId);
    }
}