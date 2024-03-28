using AmdarisProject.models.competition;

namespace AmdarisProject.models.competitor
{
    public class Player(string name, List<Match> matches, List<Competition> competitions, List<Point> points, List<Team> teams)
        : Competitor(name, matches, competitions)
    {
        public List<Point> Points { get; set; } = points;
        public List<Team> Teams { get; set; } = teams;
    }
}
