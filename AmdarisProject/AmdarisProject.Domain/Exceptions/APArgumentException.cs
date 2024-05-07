using System.Text;

namespace AmdarisProject.Domain.Exceptions
{
    public class APArgumentException : AmdarisProjectException
    {
        public APArgumentException(string name) : base(name) { }

        public APArgumentException(List<string> names) : base(FormatNames(names)) { }

        private static string FormatNames(List<string> names)
        {
            StringBuilder stringBuilder = new();
            names.ForEach(name => stringBuilder = stringBuilder.Append($"{name}; "));
            return stringBuilder.ToString();
        }
    }
}
