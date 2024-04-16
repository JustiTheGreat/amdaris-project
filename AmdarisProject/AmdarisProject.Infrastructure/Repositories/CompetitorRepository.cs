using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Utils;
using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Models.CompetitorModels;
using Microsoft.EntityFrameworkCore;

namespace AmdarisProject.Infrastructure.Repositories
{
    public class CompetitorRepository(AmdarisProjectDBContext dbContext)
        : GenericRepository<Competitor>(dbContext), ICompetitorRepository
    {
        public async Task<IEnumerable<Team>> GetAllTeams()
            => await _dbContext.Set<Competitor>()
            .Where(competitor => competitor is Team)
            .Select(competitor => (Team)competitor)
            .ToListAsync();

        public async Task<IEnumerable<Player>> GetAllPlayers()
            => await _dbContext.Set<Competitor>()
            .Where(competitor => competitor is Player)
            .Select(competitor => (Player)competitor)
            .ToListAsync();

        public async Task<IEnumerable<Player>> GetTeamPlayers(Guid teamId)
            => await _dbContext.Set<Competitor>()
            .Where(competitor => (competitor is Player) && ((Player)competitor).Teams.Exists(team => team.Id.Equals(teamId)))
            .Select(competitor => (Player)competitor)
            .ToListAsync();

        public async Task<IEnumerable<Player>> GetPlayersNotInTeam(Guid teamId)
            => await _dbContext.Set<Competitor>()
            .Where(competitor => (competitor is Player) && ((Player)competitor).Teams.All(team => !team.Id.Equals(teamId)))
            .Select(competitor => (Player)competitor)
            .ToListAsync();

        //TODO should be moved to handler
        public async Task<IEnumerable<Player>> GetPlayersNotInCompetition(Guid competitionId)
            => throw new NotImplementedException();

        //TODO should be moved to handler
        public async Task<IEnumerable<Team>> GetTeamsThatCanBeAddedToCompetition(Guid competitionId)
            => await _dbContext.Set<Competitor>()
                .Where(competitor => competitor is Team
                    && ((Team)competitor).Players.Count() == ((Team)competitor).TeamSize
                    && !competitor.Competitions.Exists(competition => competition.Id.Equals(competitionId)))
                .Select(competitor => (Team)competitor)
                .Where(team => team.Competitions.FirstOrDefault(c => c.Id.Equals(competitionId)
                    && c.CompetitorType == CompetitorType.TEAM
                    && c.TeamSize == team.TeamSize) != null
                    && team.Players.All(player =>
                        !HandlerUtils.CompetitionContainsCompetitor(team.Competitions.FirstOrDefault(c =>
                                c.Id.Equals(competitionId)
                                && c.CompetitorType == CompetitorType.TEAM
                                && c.TeamSize == team.TeamSize)!, player.Id)))
                .ToListAsync();
    }
}
