using AmdarisProject.Application.Abstractions.RepositoryAbstractions;
using AmdarisProject.Domain.Models;
using AmdarisProject.Infrastructure.Persistance.Contexts;

namespace AmdarisProject.Infrastructure.Persistance.Repositories
{
    public class GameTypeRepository(AmdarisProjectDBContext dbContext)
        : GenericRepository<GameType>(dbContext), IGameTypeRepository
    {
    }
}
