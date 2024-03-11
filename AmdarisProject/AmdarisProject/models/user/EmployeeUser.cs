using AmdarisProject.models.competition;

namespace AmdarisProject.models.user
{
    public class EmployeeUser : User
    {
        public List<Competition>? Competitions { get; set; }
    }
}
