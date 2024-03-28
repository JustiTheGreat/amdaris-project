using AmdarisProject.utils.Exceptions;

namespace AmdarisProject.utils.exceptions
{
    public class APCompetitorNumberException(string className, string methodName, string message)
        : AmdarisProjectException(className, methodName, message)
    {
    }
}
