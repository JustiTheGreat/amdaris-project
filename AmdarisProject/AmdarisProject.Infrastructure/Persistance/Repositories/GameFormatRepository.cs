using AmdarisProject.Application.Common.Abstractions.RepositoryAbstractions;
using AmdarisProject.Domain.Models;
using AmdarisProject.Infrastructure.Persistance.Contexts;

namespace AmdarisProject.Infrastructure.Persistance.Repositories
{
    public class GameFormatRepository(AmdarisProjectDBContext dbContext)
        : GenericRepository<GameFormat>(dbContext), IGameFormatRepository
    {
    }
}
