using AmdarisProject.utils.enums;

namespace AmdarisProject.models.competitor
{
    public abstract class Competitor(string name) : Model
    {
        public string Name { get; set; } = name;

        public abstract int GetPoints(Match match);

        public abstract void InitializePointsForMatch(Match match);

        public abstract double GetRating(GameType gameType);
    }
}
