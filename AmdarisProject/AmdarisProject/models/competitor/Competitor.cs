namespace AmdarisProject.models.competitor
{
    public abstract class Competitor
    {
        private static int instances = 0;
        public int Id { get; set; }
        public string Name { get; set; }

        protected Competitor(string name)
        {
            Id = ++instances;
            Name = name;
        }

        public abstract int GetPoints(Match match);
    }
}
