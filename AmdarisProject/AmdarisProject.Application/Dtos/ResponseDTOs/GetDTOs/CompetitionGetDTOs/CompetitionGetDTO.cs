using AmdarisProject.Application.Dtos.DisplayDTOs;
using AmdarisProject.Application.Dtos.DisplayDTOs.CompetitorDisplayDTOs;
using AmdarisProject.Domain.Enums;

namespace AmdarisProject.Application.Dtos.ResponseDTOs.CompetitionResponseDTOs
{
    public class CompetitionGetDTO : GetDTO
    {
        public required string Name { get; set; }
        public required string Location { get; set; }
        public required DateTime StartTime { get; set; }
        public required CompetitionStatus Status { get; set; }
        public required ulong? BreakInSeconds { get; set; }
        public required GameType GameType { get; set; }
        public required CompetitorType CompetitorType { get; set; }
        public required ushort? TeamSize { get; set; }
        public required uint? WinAt { get; set; }
        public required ulong? DurationInSeconds { get; set; }
        public required List<CompetitorDisplayDTO> Competitors { get; set; } = [];
        public required List<MatchDisplayDTO> Matches { get; set; } = [];
    }
}
