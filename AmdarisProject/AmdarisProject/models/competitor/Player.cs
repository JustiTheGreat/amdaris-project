namespace AmdarisProject.models.competitor
{
    public class Player : Competitor
    {
        public Dictionary<Match, int> Points = [];

        public Player(string name) : base(name)
        {
        }

        public override int GetPoints(Match match)
        {
            return Points[match];
        }

        public void AddPoints(Match match, int points)
        {
            Points.Add(match, Points.GetValueOrDefault(match, 0) + points);
            Console.WriteLine($"Player {Name} scored {points} points");
        }
    }
}
