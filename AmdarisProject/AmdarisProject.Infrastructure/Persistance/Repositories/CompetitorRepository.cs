using AmdarisProject.Application.Common.Abstractions.RepositoryAbstractions;
using AmdarisProject.Domain.Models.CompetitorModels;
using AmdarisProject.Infrastructure.Persistance.Contexts;
using Microsoft.EntityFrameworkCore;
using OnlineBookShop.Application.Common.Models;
using OnlineBookShop.Application.Extensions;

namespace AmdarisProject.Infrastructure.Persistance.Repositories
{
    public class CompetitorRepository(AmdarisProjectDBContext dbContext)
        : GenericRepository<Competitor>(dbContext), ICompetitorRepository
    {
        public new async Task<Competitor?> GetById(Guid id)
            => (Competitor?)await GetPlayerById(id) ?? await GetTeamById(id);

        public async Task<Player?> GetPlayerById(Guid id)
            => await _dbContext.Set<Player>()
            .AsSplitQuery()
            .Include(o => o.Matches)
            .Include(o => o.WonMatches)
            .Include(o => o.Competitions)
            .Include(o => o.Points)
            .Include(o => o.Teams)
            .FirstOrDefaultAsync(item => item.Id.Equals(id));

        public async Task<Team?> GetTeamById(Guid id)
            => await _dbContext.Set<Team>()
            .AsSplitQuery()
            .Include(o => o.Matches)
            .Include(o => o.WonMatches)
            .Include(o => o.Competitions)
            .Include(o => o.Players)
            .FirstOrDefaultAsync(item => item.Id.Equals(id));

        public async Task<IEnumerable<Player>> GetAllPlayers()
            => await _dbContext.Set<Player>().ToListAsync();

        public async Task<IEnumerable<Team>> GetAllTeams()
            => await _dbContext.Set<Team>().ToListAsync();

        public async Task<IEnumerable<Player>> GetPagedPlayers(PagedRequest pagedRequest)
            => await _dbContext.Set<Player>().CreatePaginatedResultAsync(pagedRequest);

        public async Task<IEnumerable<Team>> GetPagedTeams(PagedRequest pagedRequest)
            => await _dbContext.Set<Team>().CreatePaginatedResultAsync(pagedRequest);

        public async Task<bool> PlayerIsInATeam(Guid playerId)
            => await _dbContext.Set<Team>().AnyAsync(team => team.Players.Any(player => player.Id.Equals(playerId)));

        public async Task<IEnumerable<Player>> GetPlayersNotInTeam(Guid teamId)
            => await _dbContext.Set<Player>()
            .Where(player => player.Teams.All(team => !team.Id.Equals(teamId)))
            .ToListAsync();

        public async Task<IEnumerable<Player>> GetPlayersNotInCompetition(Guid competitionId)
            => await _dbContext.Set<Player>()
            .Where(player => player.Competitions.All(competition => !competition.Id.Equals(competitionId)))
            .ToListAsync();

        public async Task<IEnumerable<Team>> GetTeamsNotInCompetition(Guid competitionId)
            => await _dbContext.Set<Team>()
            .Where(team => team.Competitions.All(competition => !competition.Id.Equals(competitionId)))
            .ToListAsync();
    }
}
