using AmdarisProject.utils.Exceptions;

namespace AmdarisProject.utils.exceptions
{
    public class APPointsException(string className, string methodName, string message)
        : AmdarisProjectException(className, methodName, message)
    {
    }
}
