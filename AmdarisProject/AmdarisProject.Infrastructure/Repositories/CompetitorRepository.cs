using AmdarisProject.Application.Abstractions;
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

        public async Task<IEnumerable<Player>> GetTeamPlayers(ulong teamId)
            => await _dbContext.Set<Competitor>()
            .Where(competitor => (competitor is Player) && ((Player)competitor).Teams.Exists(team => team.Id == teamId))
            .Select(competitor => (Player)competitor)
            .ToListAsync();
    }
}
