using AmdarisProject.models.competition;

namespace AmdarisProject.models
{
    public class Stage : Model
    {
        public ushort StageLevel { get; set; }
        public List<Match> Matches { get; set; }
        public Competition Competition { get; set; }

        public Stage() { }

        public Stage(ushort stageLevel, List<Match> matches, Competition competition)
        {
            StageLevel = stageLevel;
            Matches = matches;
            Competition = competition;
        }
    }
}
