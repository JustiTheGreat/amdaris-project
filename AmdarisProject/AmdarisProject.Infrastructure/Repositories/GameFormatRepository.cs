using AmdarisProject.Application.Abstractions;
using AmdarisProject.Domain.Models;

namespace AmdarisProject.Infrastructure.Repositories
{
    public class GameFormatRepository(AmdarisProjectDBContext dbContext) : GenericRepository<GameFormat>(dbContext), IGameFormatRepository
    {
    }
}
