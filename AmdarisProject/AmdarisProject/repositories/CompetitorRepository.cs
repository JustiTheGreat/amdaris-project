using AmdarisProject.models.competitor;
using AmdarisProject.repositories.abstractions;
using AmdarisProject.utils.Exceptions;

namespace AmdarisProject.repositories
{
    public class CompetitorRepository : GenericRepository<Competitor>, ICompetitorRepository
    {
        public override Competitor Update(Competitor competitor)
        {
            if (competitor is null)
                throw new APArgumentException(nameof(CompetitorRepository), nameof(Update), nameof(competitor));

            Competitor stored = GetById(competitor.Id);
            stored.Name = competitor.Name;
            return stored;
        }
    }
}
