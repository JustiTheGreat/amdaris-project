using AmdarisProject.Domain.Enums;

namespace AmdarisProject.Domain.Exceptions
{
    public class APIllegalStatusException : AmdarisProjectException
    {
        public APIllegalStatusException(CompetitionStatus status)
            : base(status.ToString()) { }

        public APIllegalStatusException(MatchStatus status)
            : base(status.ToString()) { }
    }
}
