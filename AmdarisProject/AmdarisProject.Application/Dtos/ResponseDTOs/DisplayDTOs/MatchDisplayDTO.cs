namespace AmdarisProject.Application.Dtos.ResponseDTOs.DisplayDTOs
{
    public class MatchDisplayDTO : DisplayDTO
    {
        public required string Status { get; set; }
        public required DateTime? StartTime { get; set; }
        public required string Competitors { get; set; }
        public required string Score { get; set; }
        public required string Competition { get; set; }
        public required string Winner { get; set; }
    }
}
