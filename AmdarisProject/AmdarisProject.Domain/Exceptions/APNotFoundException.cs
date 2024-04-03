namespace Domain.Exceptions
{
    public class APNotFoundException(string className, string methodName, string message)
        : AmdarisProjectException(className, methodName, message)
    {
    }
}
