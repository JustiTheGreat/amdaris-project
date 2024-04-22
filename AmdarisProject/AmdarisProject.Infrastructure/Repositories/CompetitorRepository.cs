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

        public async Task<IEnumerable<Player>> GetPlayersInTeam(Guid teamId)
            => await _dbContext.Set<Player>()
            .Where(player => player.Teams.Any(team => team.Id.Equals(teamId)))
            .ToListAsync();

        public async Task<IEnumerable<Player>> GetPlayersNotInTeam(Guid teamId)
            => await _dbContext.Set<Player>()
            .Where(player => player.Teams.All(team => !team.Id.Equals(teamId)))
            .ToListAsync();

        //TODO should be moved to handler
        public async Task<IEnumerable<Player>> GetPlayersNotInCompetition(Guid competitionId)
            => throw new NotImplementedException();

        //TODO should be moved to handler
        public async Task<IEnumerable<Team>> GetTeamsThatCanBeAddedToCompetition(Guid competitionId)
            => throw new NotImplementedException();
        //await _dbContext.Set<Team>()
        //        .Where(team => team.Players.Count() == team.TeamSize
        //            && !team.Competitions.Exists(competition => competition.Id.Equals(competitionId))
        //            && team.Competitions.FirstOrDefault(c => c.Id.Equals(competitionId)
        //                && c.CompetitorType == CompetitorType.TEAM
        //                && c.TeamSize == team.TeamSize) != null
        //                && team.Players.All(player =>
        //                    !team.Competitions.FirstOrDefault(c =>
        //                        c.Id.Equals(competitionId)
        //                        && c.CompetitorType == CompetitorType.TEAM
        //                        && c.TeamSize == team.TeamSize).ContainsCompetitor(player.Id)))
        //        .ToListAsync();
    }
}
