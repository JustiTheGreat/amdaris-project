using System.Text;

namespace AmdarisProject.Domain.Exceptions
{
    public class APNotFoundException(string className, string methodName, List<Tuple<string, ulong>> ids)
        : AmdarisProjectException(className, methodName, FormatIds(ids))
    {
        private static string FormatIds(List<Tuple<string, ulong>> ids)
        {
            StringBuilder stringBuilder = new();
            ids.ForEach(id => stringBuilder = stringBuilder.Append($"{id.Item1}={id.Item2};"));
            return stringBuilder.ToString();
        }
    }
}
