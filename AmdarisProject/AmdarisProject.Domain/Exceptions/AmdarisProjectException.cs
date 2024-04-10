namespace AmdarisProject.Domain.Exceptions
{
    public class AmdarisProjectException(string className, string methodName, string message)
        : Exception(FormatExceptionMessage(className, methodName, message))
    {
        private static string FormatExceptionMessage(string className, string methodName, string message)
            => $"{className}: {methodName}: {message}";
    }
}
