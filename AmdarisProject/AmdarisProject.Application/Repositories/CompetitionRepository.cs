using AmdarisProject.models.competition;
using Domain.Exceptions;

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
            stored.Status = competition.Status;
            stored.WinAt = competition.WinAt;
            stored.DurationInSeconds = competition.DurationInSeconds;
            stored.BreakInSeconds = competition.BreakInSeconds;
            stored.GameType = competition.GameType;
            stored.CompetitorType = competition.CompetitorType;
            stored.TeamSize = competition.TeamSize;
            return stored;
        }
    }
}
