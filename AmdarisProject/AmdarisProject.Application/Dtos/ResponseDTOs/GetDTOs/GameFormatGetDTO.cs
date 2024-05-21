using AmdarisProject.Application.Dtos.ResponseDTOs.GetDTOs;
using AmdarisProject.Domain.Enums;

namespace AmdarisProject.Application.Dtos.ResponseDTOs
{
    public class GameFormatGetDTO : GetDTO
    {
        public required string Name { get; set; }
        public required GameTypeGetDTO GameType { get; set; }
        public required CompetitorType CompetitorType { get; set; }
        public required uint? TeamSize { get; set; }
        public required uint? WinAt { get; set; }
        public required ulong? DurationInMinutes { get; set; }
    }
}
