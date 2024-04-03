using AmdarisProject.models.competitor;
using AmdarisProject.repositories.abstractions;
using Domain.Exceptions;

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

            if (stored is Team team)
                team.TeamSize = ((Team)competitor).TeamSize;

            return stored;
        }

        public IEnumerable<Team> GetAllTeams()
            => _dataSet.Where(competitor => competitor is Team).Select(competitor => (Team)competitor).ToList();

        public IEnumerable<Player> GetAllPlayers()
            => _dataSet.Where(competitor => competitor is Player).Select(competitor => (Player)competitor).ToList();
    }
}
