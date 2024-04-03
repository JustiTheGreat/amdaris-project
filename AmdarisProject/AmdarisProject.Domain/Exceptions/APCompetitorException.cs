namespace Domain.Exceptions
{
    public class APCompetitorException(string className, string methodName, string message)
        : AmdarisProjectException(className, methodName, message)
    {
    }
}
