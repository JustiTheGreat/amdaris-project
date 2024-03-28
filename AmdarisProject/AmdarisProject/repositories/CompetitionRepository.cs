using AmdarisProject.models.competition;
using AmdarisProject.utils.Exceptions;

namespace AmdarisProject.repositories.abstractions
{
    public class CompetitionRepository : GenericRepository<Competition>, ICompetitionRepository
    {
        public override Competition Update(Competition competition)
        {
            if (competition is null)
                throw new APArgumentException(nameof(CompetitionRepository), nameof(Update), nameof(competition));

            Competition stored = GetById(competition.Id);
            stored.Name = competition.Name;
            stored.Location = competition.Location;
            stored.StartTime = competition.StartTime;
            stored.GameRules = competition.GameRules;
            stored.Status = competition.Status;
            return stored;
        }
    }
}
