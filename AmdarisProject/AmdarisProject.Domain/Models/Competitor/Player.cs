using AmdarisProject.models.competition;

namespace AmdarisProject.models.competitor
{
    public class Player : Competitor
    {
        public virtual List<Point> Points { get; set; }
        public virtual List<Team> Teams { get; set; }

        public Player()
        {
        }

        public Player(string name, List<Match> matches, List<Competition> competitions, List<Point> points, List<Team> teams)
            : base(name, matches, competitions)
        {
            Points = points;
            Teams = teams;
        }
    }
}
