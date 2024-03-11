namespace AmdarisProject.models
{
    public class Stage
    {
        private static int instances = 0;
        public int Id { get; set; }
        public int StageLevel { get; set; }
        public IEnumerable<Match> Matches { get; set; }

        public Stage(int stageLevel, IEnumerable<Match> matches)
        {
            Id = ++instances;
            StageLevel = stageLevel;
            Matches = matches;
        }
    }
}
