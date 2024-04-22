using AmdarisProject.Domain.Enums;

namespace AmdarisProject.Application.Dtos.CreateDTOs
{
    public class GameFormatCreateDTO : CreateDTO
    {
        public string Name { get; set; }
        public GameType GameType { get; set; }
        public CompetitorType CompetitorType { get; set; }
        public ushort? TeamSize { get; set; }
        public uint? WinAt { get; set; }
        public ulong? DurationInSeconds { get; set; }
    }
}
