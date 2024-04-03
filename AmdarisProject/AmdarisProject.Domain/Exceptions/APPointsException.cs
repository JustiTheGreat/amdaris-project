namespace Domain.Exceptions
{
    public class APPointsException(string className, string methodName, string message)
        : AmdarisProjectException(className, methodName, message)
    {
    }
}
