using AmdarisProject.models.competitor;

namespace AmdarisProject.repositories.abstractions
{
    public interface ICompetitorRepository : IGenericRepository<Competitor>
    {
        public IEnumerable<Team> GetAllTeams();

        public IEnumerable<Player> GetAllPlayers();
    }
}
