using AmdarisProject.models.competition;

namespace AmdarisProject.models.competitor
{
    public abstract class Competitor(string name, List<Match> matches, List<Competition> competitions) : Model
    {
        public string Name { get; set; } = name;

        public List<Match> Matches { get; set; } = matches;

        public List<Competition> Competitions { get; set; } = competitions;
    }
}
