using AmdarisProject.Domain.Enums;

namespace AmdarisProject.Application.Dtos.DisplayDTOs
{
    public class MatchDisplayDTO : DisplayDTO
    {
        public required MatchStatus Status { get; set; }
        public required string CompetitorOneName { get; set; }
        public required string CompetitorTwoName { get; set; }
        public required string CompetitionName { get; set; }
        public required uint? CompetitorOnePoints { get; set; }
        public required uint? CompetitorTwoPoints { get; set; }
        public required string? WinnerName { get; set; }
    }
}
