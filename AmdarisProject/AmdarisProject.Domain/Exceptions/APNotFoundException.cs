using System.Text;

namespace AmdarisProject.Domain.Exceptions
{
    public class APNotFoundException : AmdarisProjectException
    {
        public APNotFoundException(Tuple<string, ulong> id) : base(FormatId(id)) { }

        public APNotFoundException(List<Tuple<string, ulong>> ids) : base(FormatIds(ids)) { }

        private static string FormatId(Tuple<string, ulong> id) => $"{id.Item1}={id.Item2};";

        private static string FormatIds(List<Tuple<string, ulong>> ids)
        {
            StringBuilder stringBuilder = new();
            ids.ForEach(id => stringBuilder = stringBuilder.Append(FormatId(id)));
            return stringBuilder.ToString();
        }
    }
}
