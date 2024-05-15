using AmdarisProject.Domain.Models.CompetitorModels;
using OnlineBookShop.Application.Common.Models;

namespace AmdarisProject.Application.Common.Abstractions.RepositoryAbstractions
{
    public interface ICompetitorRepository : IGenericRepository<Competitor>
    {
        Task<IEnumerable<Team>> GetAllTeams();

        Task<IEnumerable<Player>> GetAllPlayers();

        Task<IEnumerable<Player>> GetPagedPlayers(PagedRequest pagedRequest);

        Task<IEnumerable<Team>> GetPagedTeams(PagedRequest pagedRequest);

        Task<Player?> GetPlayerById(Guid id);

        Task<Team?> GetTeamById(Guid id);

        Task<bool> PlayerIsInATeam(Guid playerId);

        Task<IEnumerable<Player>> GetPlayersNotInTeam(Guid teamId);

        Task<IEnumerable<Player>> GetPlayersNotInCompetition(Guid competitionId);

        Task<IEnumerable<Team>> GetTeamsNotInCompetition(Guid competitionId);
    }
}
