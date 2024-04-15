using AmdarisProject.Domain.Models.CompetitorModels;

namespace AmdarisProject.Application.Abstractions
{
    public interface ICompetitorRepository : IGenericRepository<Competitor>
    {
        Task<IEnumerable<Team>> GetAllTeams();

        Task<IEnumerable<Player>> GetAllPlayers();

        Task<IEnumerable<Player>> GetTeamPlayers(Guid teamId);
    }
}
