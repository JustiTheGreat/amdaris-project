using AmdarisProject.Domain.Models.CompetitorModels;

namespace AmdarisProject.Application.Common.Abstractions.RepositoryAbstractions
{
    public interface ITeamPlayerRepository : IGenericRepository<TeamPlayer>
    {
        Task<TeamPlayer?> GetByTeamAndPlayer(Guid teamId, Guid playerId);

        Task<bool> TeamHasTheRequiredNumberOfActivePlayers(Guid teamId, uint requiredNumberOfActivePlayers);
    }
}
