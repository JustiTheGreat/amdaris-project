using AmdarisProject.Application.Abstractions;
using AmdarisProject.Domain.Models;

namespace AmdarisProject.Infrastructure.Repositories
{
    public class StageRepository(AmdarisProjectDBContext dbContext)
        : GenericRepository<Stage>(dbContext), IStageRepository
    {
    }
}
