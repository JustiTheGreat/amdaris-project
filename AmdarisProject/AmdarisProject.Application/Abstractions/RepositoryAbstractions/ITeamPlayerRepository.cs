using AmdarisProject.Domain.Models;

namespace AmdarisProject.Application.Abstractions.RepositoryAbstractions
{
    public interface ITeamPlayerRepository : IGenericRepository<TeamPlayer>
    {
        Task<TeamPlayer?> GetByTeamAndPlayer(Guid teamId, Guid playerId);
    }
}
