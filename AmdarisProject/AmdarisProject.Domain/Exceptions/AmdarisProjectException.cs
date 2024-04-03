namespace Domain.Exceptions
{
    public class AmdarisProjectException(string className, string methodName, string message)
        : Exception(Format(className, methodName, message))
    {
        public static string Format(string className, string methodName, string message)
            => $"{className}: {methodName}: {message}";
    }
}
