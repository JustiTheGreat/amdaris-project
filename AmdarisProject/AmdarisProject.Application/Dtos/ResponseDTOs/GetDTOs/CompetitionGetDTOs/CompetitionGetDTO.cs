using AmdarisProject.Application.Dtos.ResponseDTOs.DisplayDTOs;
using AmdarisProject.Application.Dtos.ResponseDTOs.GetDTOs;

namespace AmdarisProject.Application.Dtos.ResponseDTOs.CompetitionResponseDTOs
{
    public abstract class CompetitionGetDTO : GetDTO
    {
        public required string Name { get; set; }
        public required string Location { get; set; }
        public required DateTime InitialStartTime { get; set; }
        public required DateTime ActualizedStartTime { get; set; }
        public required string Status { get; set; }
        public required ulong? BreakInMinutes { get; set; }
        public required GameTypeGetDTO GameType { get; set; }
        public required string CompetitorType { get; set; }
        public required uint? TeamSize { get; set; }
        public required uint? WinAt { get; set; }
        public required ulong? DurationInMinutes { get; set; }
        public required List<CompetitorDisplayDTO> Competitors { get; set; } = [];
        public required List<MatchDisplayDTO> Matches { get; set; } = [];
    }
}
