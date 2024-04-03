namespace AmdarisProject.models
{
    public abstract class Model
    {
        private static ulong instances = 0;
        public ulong Id { get; set; } = ++instances;
    }
}
