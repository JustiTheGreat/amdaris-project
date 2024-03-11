namespace AmdarisProject.utils
{
    public class MessageFormatter
    {
        public static string Format(string className, string methodName, string message)
        {
            return $"{className}: {methodName}: {message}";
        }
    }
}
