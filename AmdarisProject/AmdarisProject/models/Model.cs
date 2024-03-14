namespace AmdarisProject.models
{
    public abstract class Model
    {
        private static int instances = 0;
        public int Id { get; set; } = ++instances;

        public void CopyDataFrom(Model model)
        {
        }
    }
}
