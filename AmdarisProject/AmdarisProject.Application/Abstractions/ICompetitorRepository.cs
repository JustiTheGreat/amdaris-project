using AmdarisProject.Domain.Models.CompetitorModels;

namespace AmdarisProject.Application.Abstractions
{
    public interface ICompetitorRepository : IGenericRepository<Competitor>
    {
        Task<IEnumerable<Team>> GetAllTeams();

        Task<IEnumerable<Player>> GetAllPlayers();

        Task<bool> PlayerIsInATeam(Guid playerId);

        Task<IEnumerable<Player>> GetPlayersInTeam(Guid teamId);

        Task<IEnumerable<Player>> GetPlayersNotInTeam(Guid teamId);

        Task<IEnumerable<Player>> GetPlayersNotInCompetition(Guid competitionId);

        Task<IEnumerable<Team>> GetFullTeamsWithTeamSizeNotInCompetition(Guid competitionId, ushort teamSize);
    }
}
