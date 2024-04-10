namespace AmdarisProject.Domain.Exceptions
{
    public class APNullReferenceException(string className, string methodName, string message)
        : AmdarisProjectException(className, methodName, message)
    { }
}
