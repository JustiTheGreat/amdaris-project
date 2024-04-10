namespace AmdarisProject.Application.Dtos.CreateDTOs.CompetitorCreateDTOs
{
    public class TeamCreateDTO : CompetitorCreateDTO
    {
        public required ushort TeamSize { get; init; }
        public List<ulong> Players { get; set; } = [];
    }
}
