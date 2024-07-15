using AmdarisProject.Domain.Models;

namespace AmdarisProject.Domain.Extensions
{
    public static class ModelListExtensions
    {
        public static List<Guid> GetIds<T>(this T models) where T : IEnumerable<Model>
            => models.Select(model => model.Id!).ToList();
    }
}
