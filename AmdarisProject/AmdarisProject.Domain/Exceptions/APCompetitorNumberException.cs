namespace Domain.Exceptions
{
    public class APCompetitorNumberException(string className, string methodName, string message)
        : AmdarisProjectException(className, methodName, message)
    {
    }
}
