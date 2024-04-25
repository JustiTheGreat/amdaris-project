using AmdarisProject.Application.Abstractions;
using AmdarisProject.Domain.Models.CompetitorModels;
using Microsoft.EntityFrameworkCore;

namespace AmdarisProject.Infrastructure.Repositories
{
    public class CompetitorRepository(AmdarisProjectDBContext dbContext)
        : GenericRepository<Competitor>(dbContext), ICompetitorRepository
    {
        public async Task<IEnumerable<Team>> GetAllTeams()
            => await _dbContext.Set<Team>().ToListAsync();

        public async Task<IEnumerable<Player>> GetAllPlayers()
            => await _dbContext.Set<Player>().ToListAsync();

        //TODO add GameType to team
        //public async Task<bool> PlayerIsAlreadyInATeamForCompetition(Guid playerId, Guid competition)
        //    => await _dbContext.Set<Team>()
        //    .Where(team => team.Competitions.Any(team=>)team.Players.Count == team.TeamSize && team.TeamSize == teamSize
        //        && team.Competitions.All(competition => !competition.Id.Equals(competitionId)))
        //    .ToListAsync();

        public async Task<IEnumerable<Player>> GetPlayersInTeam(Guid teamId)
            => await _dbContext.Set<Player>()
            .Where(player => player.Teams.Any(team => team.Id.Equals(teamId)))
            .ToListAsync();

        public async Task<IEnumerable<Player>> GetPlayersNotInTeam(Guid teamId)
            => await _dbContext.Set<Player>()
            .Where(player => player.Teams.All(team => !team.Id.Equals(teamId)))
            .ToListAsync();

        public async Task<IEnumerable<Player>> GetPlayersNotInCompetition(Guid competitionId)
            => await _dbContext.Set<Player>()
            .Where(player => player.Competitions.All(competition => !competition.Id.Equals(competitionId)))
            .ToListAsync();

        public async Task<IEnumerable<Team>> GetFullTeamsWithTeamSizeNotInCompetition(Guid competitionId, ushort teamSize)
            => await _dbContext.Set<Team>()
            .Where(team => team.Players.Count == team.TeamSize && team.TeamSize == teamSize
                && team.Competitions.All(competition => !competition.Id.Equals(competitionId)))
            .ToListAsync();
    }
}
