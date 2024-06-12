using AmdarisProject.Application.Dtos.ResponseDTOs.GetDTOs;

namespace AmdarisProject.Application.Dtos.ResponseDTOs
{
    public class GameFormatGetDTO : GetDTO
    {
        public required string Name { get; set; }
        public required GameTypeGetDTO GameType { get; set; }
        public required string CompetitorType { get; set; }
        public required uint? TeamSize { get; set; }
        public required uint? WinAt { get; set; }
        public required ulong? DurationInMinutes { get; set; }
    }
}
