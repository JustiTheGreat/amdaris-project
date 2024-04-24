using AmdarisProject.Domain.Enums;

namespace AmdarisProject.Application.Dtos.DisplayDTOs
{
    public class MatchDisplayDTO : DisplayDTO
    {
        public MatchStatus Status { get; set; }
        public string CompetitorOneName { get; set; }
        public string CompetitorTwoName { get; set; }
        public string CompetitionName { get; set; }
        public uint? CompetitorOnePoints { get; set; }
        public uint? CompetitorTwoPoints { get; set; }
        public string? WinnerName { get; set; }
    }
}
