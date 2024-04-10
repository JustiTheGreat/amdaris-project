using AmdarisProject.Domain.Enums;

namespace AmdarisProject.Domain.Exceptions
{
    public class APIllegalStatusException : AmdarisProjectException
    {
        public APIllegalStatusException(string className, string methodName, CompetitionStatus status)
            : base(className, methodName, status.ToString()) { }

        public APIllegalStatusException(string className, string methodName, MatchStatus status)
            : base(className, methodName, status.ToString()) { }
    }
}
