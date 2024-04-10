using AmdarisProject.Application.Abstractions;
using AmdarisProject.Domain.Models.CompetitionModels;

namespace AmdarisProject.Infrastructure.Repositories
{
    public class CompetitionRepository(AmdarisProjectDBContext dbContext)
        : GenericRepository<Competition>(dbContext), ICompetitionRepository
    {
    }
}
