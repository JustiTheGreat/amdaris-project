using AmdarisProject.Application.Common.Models;
using AmdarisProject.Domain.Models.CompetitionModels;
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

        Task<Tuple<IEnumerable<Player>, int>> GetPlayersNotInTeam(Guid teamId, PagedRequest pagedRequest);

        Task<Tuple<IEnumerable<Player>, int>> GetPlayersNotInCompetition(Guid competitionId, PagedRequest pagedRequest);

        Task<Tuple<IEnumerable<Team>, int>> GetTeamsThatCanBeAddedToCompetition(Guid competitionId, uint requiredTeamSize, PagedRequest pagedRequest);
    }
}
