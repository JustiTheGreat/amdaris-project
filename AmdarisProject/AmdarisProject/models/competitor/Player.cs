namespace AmdarisProject.models.competitor
{
    public class Player : Competitor
    {
        public Dictionary<Match, int> Points = new();

        public Player(string name) : base(name)
        {
        }

        public override int GetPoints(Match match)
        {
            return Points[match];
        }

        public void AddPoints(Match match, int points)
        {
            Points[match] += points;
            Console.WriteLine($"Player {Name} scored {points} points");
        }
    }
}
