using AmdarisProject.Domain.Models;

namespace AmdarisProject.Application.Utils.ExtensionMethods
{
    public static class GetModelsIds
    {
        public static List<Guid> GetIds<T>(this T models) where T : IEnumerable<Model>
            => models.Select(model => model.Id!).ToList();
    }
}
