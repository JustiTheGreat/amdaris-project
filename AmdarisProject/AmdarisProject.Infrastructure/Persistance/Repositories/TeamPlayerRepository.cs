using AmdarisProject.Application.Abstractions.RepositoryAbstractions;
using AmdarisProject.Domain.Models.CompetitorModels;
using Microsoft.EntityFrameworkCore;

namespace AmdarisProject.Infrastructure.Persistance.Repositories
{
    public class TeamPlayerRepository(AmdarisProjectDBContext dbContext)
        : GenericRepository<TeamPlayer>(dbContext), ITeamPlayerRepository
    {
        public async Task<TeamPlayer?> GetByTeamAndPlayer(Guid teamId, Guid playerId)
            => await _dbContext.Set<TeamPlayer>()
            .AsSplitQuery()
            .Include(o => o.Team)
            .Include(o => o.Player)
            .FirstOrDefaultAsync(teamPlayer => teamPlayer.Team.Id.Equals(teamId) && teamPlayer.Player.Id.Equals(playerId));

        public async Task<bool> TeamHasTheRequiredNumberOfActivePlayers(Guid teamId, uint requiredNumberOfActivePlayers)
            => await _dbContext.Set<TeamPlayer>().CountAsync(teamPlayer => teamPlayer.Team.Id.Equals(teamId) && teamPlayer.IsActive)
            == requiredNumberOfActivePlayers;
    }
}
