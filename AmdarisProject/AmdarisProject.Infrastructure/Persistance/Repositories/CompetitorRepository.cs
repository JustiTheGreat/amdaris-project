﻿using AmdarisProject.Application.Abstractions.RepositoryAbstractions;
using AmdarisProject.Application.Common.Models;
using AmdarisProject.Domain.Models.CompetitionModels;
using AmdarisProject.Domain.Models.CompetitorModels;
using AmdarisProject.Infrastructure.Persistance.Contexts;
using AmdarisProject.Infrastructure.Persistance.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace AmdarisProject.Infrastructure.Persistance.Repositories
{
    public class CompetitorRepository(AmdarisProjectDBContext dbContext)
        : GenericRepository<Competitor>(dbContext), ICompetitorRepository
    {
        public new async Task<Competitor?> GetById(Guid id)
            => await _dbContext.Set<Competitor>()
            .AsSplitQuery()
            .Include(o => o.Matches)
            .Include(o => o.WonMatches)
            .Include(o => o.Competitions)
            .Include(o => ((Player)o).Points)
            .Include(o => ((Player)o).Teams)
            .Include(o => ((Team)o).Players)
            .Include(o => ((Team)o).TeamPlayers).ThenInclude(o => o.Player).ThenInclude(o => o.Competitions)
            .Include(o => ((Team)o).TeamPlayers).ThenInclude(o => o.Player).ThenInclude(o => o.Matches)
            .FirstOrDefaultAsync(item => item.Id.Equals(id));

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
            .Include(o => o.TeamPlayers)
            .Include(o => o.Players)
            .FirstOrDefaultAsync(item => item.Id.Equals(id));

        //TODO throw exception if one id is not found?
        public async Task<IEnumerable<Competitor>> GetByIds(IEnumerable<Guid> ids)
            => (await Task.WhenAll(ids.Select(GetById)))
            .Where(competitor => competitor is not null)
            .ToList()!;

        public async Task<IEnumerable<Player>> GetAllPlayers()
            => await _dbContext.Set<Player>().ToListAsync();

        public async Task<IEnumerable<Team>> GetAllTeams()
            => await _dbContext.Set<Team>().ToListAsync();

        public async Task<Tuple<IEnumerable<Player>, int>> GetPaginatedPlayers(PagedRequest pagedRequest)
            => await _dbContext.Set<Player>()
            .Include(o => o.Matches)
            .Include(o => o.Competitions)
            .Include(o => o.Teams)
            .CreatePaginatedResultAsync(pagedRequest);

        public async Task<Tuple<IEnumerable<Team>, int>> GetPaginatedTeams(PagedRequest pagedRequest)
            => await _dbContext.Set<Team>()
            .Include(o => o.Matches)
            .Include(o => o.Competitions)
            .Include(o => o.TeamPlayers)
            .CreatePaginatedResultAsync(pagedRequest);

        public async Task<Tuple<IEnumerable<Player>, int>> GetPlayersNotInTeam(Guid teamId, PagedRequest pagedRequest)
            => await _dbContext.Set<Player>()
            //TODO .Where(player => player.Teams.All(team => !team.Id.Equals(teamId)))
            .Where(player => !player.Teams.Any())
            .CreatePaginatedResultAsync(pagedRequest);

        public async Task<Tuple<IEnumerable<Player>, int>> GetPlayersNotInCompetition(Guid competitionId, PagedRequest pagedRequest)
            => await _dbContext.Set<Player>()
            .AsSplitQuery()
            .Where(player => player.Competitions.All(competition => !competition.Id.Equals(competitionId)))
            .CreatePaginatedResultAsync(pagedRequest);

        public async Task<Tuple<IEnumerable<Team>, int>> GetTeamsThatCanBeAddedToCompetition(Guid competitionId, uint requiredTeamSize, PagedRequest pagedRequest)
            => await _dbContext.Set<Team>()
            .AsSplitQuery()
            .Where(team =>
                team.TeamPlayers.Count(teamPlayer => teamPlayer.IsActive) == requiredTeamSize
                && team.Competitions.All(competition => !competition.Id.Equals(competitionId)))
            .CreatePaginatedResultAsync(pagedRequest);
    }
}
