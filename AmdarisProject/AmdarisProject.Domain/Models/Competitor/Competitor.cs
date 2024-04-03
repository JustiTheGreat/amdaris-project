using AmdarisProject.models.competition;

namespace AmdarisProject.models.competitor
{
    public abstract class Competitor : Model
    {
        public string Name { get; set; }

        public List<Match> Matches { get; set; }

        public List<Competition> Competitions { get; set; }

        protected Competitor()
        {
        }

        protected Competitor(string name, List<Match> matches, List<Competition> competitions)
        {
            Name = name;
            Matches = matches;
            Competitions = competitions;
        }
    }
}
