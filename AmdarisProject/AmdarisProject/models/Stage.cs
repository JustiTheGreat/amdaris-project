using AmdarisProject.models.competition;

namespace AmdarisProject.models
{
    public class Stage(ushort stageLevel, List<Match> matches, Competition competition) : Model
    {
        public ushort StageLevel { get; set; } = stageLevel;
        public List<Match> Matches { get; set; } = matches;
        public Competition Competition { get; set; } = competition;
    }
}
