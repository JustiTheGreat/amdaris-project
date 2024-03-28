namespace AmdarisProject.utils.Exceptions
{
    public class APIllegalStatusException(string className, string methodName, string message)
        : AmdarisProjectException(className, methodName, message)
    {
    }
}
