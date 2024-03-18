using AmdarisProject.utils;

namespace AmdarisProject.models
{
    public class Stage : Model
    {
        public int StageLevel { get; set; }
        public IEnumerable<Match> Matches { get; set; }

        public Stage(IEnumerable<Match> matches)
        {
            if (matches is null || !matches.Any())
                throw new ArgumentException(MessageFormatter.Format(nameof(Stage), nameof(Stage), nameof(matches)));

            int numberOfMatches = matches.Count();
            int stageLevel = 0;
            while (numberOfMatches != 1)
            {
                if (numberOfMatches % 2 == 1)
                    throw new ArgumentException(MessageFormatter.Format(nameof(Stage), nameof(Stage), nameof(matches)));

                stageLevel++;
                numberOfMatches /= numberOfMatches;
            }

            StageLevel = stageLevel;
            Matches = matches;
        }
    }
}
