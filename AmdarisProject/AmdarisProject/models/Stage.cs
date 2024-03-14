namespace AmdarisProject.models
{
    public class Stage(int stageLevel, IEnumerable<Match> matches) : Model
    {
        public int StageLevel { get; set; } = stageLevel;
        public IEnumerable<Match> Matches { get; set; } = matches;
    }
}
