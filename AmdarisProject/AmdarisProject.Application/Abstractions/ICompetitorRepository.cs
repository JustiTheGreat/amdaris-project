using AmdarisProject.Domain.Models.CompetitorModels;

namespace AmdarisProject.Application.Abstractions
{
    public interface ICompetitorRepository : IGenericRepository<Competitor>
    {
        Task<IEnumerable<Team>> GetAllTeams();

        Task<IEnumerable<Player>> GetAllPlayers();

        Task<Player?> GetPlayerById(Guid id);

        Task<Team?> GetTeamById(Guid id);

        Task<bool> PlayerIsInATeam(Guid playerId);

        Task<IEnumerable<Player>> GetPlayersNotInTeam(Guid teamId);

        Task<IEnumerable<Player>> GetPlayersNotInCompetition(Guid competitionId);

        Task<IEnumerable<Team>> GetTeamsNotInCompetition(Guid competitionId);
    }
}
