using System.Text;

namespace AmdarisProject.Domain.Exceptions
{
    public class APNotFoundException : AmdarisProjectException
    {
        public APNotFoundException(string message) : base(message) { }

        public APNotFoundException(Tuple<string, Guid> id) : base(FormatId(id)) { }

        public APNotFoundException(List<Tuple<string, Guid>> ids) : base(FormatIds(ids)) { }

        private static string FormatId(Tuple<string, Guid> id) => $"{id.Item1}={id.Item2};";

        private static string FormatIds(List<Tuple<string, Guid>> ids)
        {
            StringBuilder stringBuilder = new();
            ids.ForEach(id => stringBuilder = stringBuilder.Append(FormatId(id)));
            return stringBuilder.ToString();
        }
    }
}
