using AmdarisProject.Domain.Enums;

namespace AmdarisProject.Application.Dtos.ResponseDTOs
{
    public class GameFormatGetDTO : GetDTO
    {
        public required string Name { get; set; }
        public required GameType GameType { get; set; }
        public required CompetitorType CompetitorType { get; set; }
        public ushort? TeamSize { get; set; }
        public required uint? WinAt { get; set; }
        public required ulong? DurationInMinutes { get; set; }
    }
}
