﻿using AmdarisProject.Application.Abstractions.RepositoryAbstractions;
using AmdarisProject.Application.Common.Models;
using AmdarisProject.Domain.Models.CompetitorModels;
using AmdarisProject.Infrastructure.Persistance.Contexts;
using AmdarisProject.Infrastructure.Persistance.Extensions;
using Microsoft.EntityFrameworkCore;

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
            .Include(o => o.TeamPlayers)
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

        public async Task<IEnumerable<Player>> GetPaginatedPlayers(PagedRequest pagedRequest)
            => await _dbContext.Set<Player>().CreatePaginatedResultAsync(pagedRequest);

        public async Task<IEnumerable<Team>> GetPaginatedTeams(PagedRequest pagedRequest)
            => await _dbContext.Set<Team>().CreatePaginatedResultAsync(pagedRequest);

        public async Task<IEnumerable<Player>> GetPlayersNotInTeam(Guid teamId)
            => await _dbContext.Set<Player>()
            .Where(player => player.Teams.All(team => !team.Id.Equals(teamId)))
            .ToListAsync();

        public async Task<IEnumerable<Player>> GetPlayersNotInCompetition(Guid competitionId)
            => await _dbContext.Set<Player>()
            .Where(player => player.NotInCompetition(competitionId))
            .ToListAsync();

        public async Task<IEnumerable<Team>> GetTeamsThatCanBeAddedToCompetition(Guid competitionId, uint requiredTeamSize)
            => await _dbContext.Set<Team>()
            .Where(team =>
                team.HasTheRequiredNumberOfActivePlayers(requiredTeamSize)
                && team.Competitions.All(competition => !competition.Id.Equals(competitionId))
                && team.Players.All(player => player.NotInCompetition(competitionId)))
            .ToListAsync();
    }
}