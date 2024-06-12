using AmdarisProject.Application.Common.Models;
using AmdarisProject.Domain.Models.CompetitorModels;

namespace AmdarisProject.Application.Abstractions.RepositoryAbstractions
{
    public interface ICompetitorRepository : IGenericRepository<Competitor>
    {
        Task<IEnumerable<Team>> GetAllTeams();

        Task<IEnumerable<Player>> GetAllPlayers();

        Task<Tuple<IEnumerable<Player>, int>> GetPaginatedPlayers(PagedRequest pagedRequest);

        Task<Tuple<IEnumerable<Team>, int>> GetPaginatedTeams(PagedRequest pagedRequest);

        Task<Player?> GetPlayerById(Guid id);

        Task<Team?> GetTeamById(Guid id);

        Task<IEnumerable<Competitor>> GetByIds(IEnumerable<Guid> ids);

        Task<IEnumerable<Player>> GetPlayersNotInTeam(Guid teamId);

        Task<IEnumerable<Player>> GetPlayersNotInCompetition(Guid competitionId);

        Task<IEnumerable<Team>> GetTeamsThatCanBeAddedToCompetition(Guid competitionId, uint requiredTeamSize);
    }
}
