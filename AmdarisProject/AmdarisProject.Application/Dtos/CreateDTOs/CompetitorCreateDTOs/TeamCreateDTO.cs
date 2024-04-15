namespace AmdarisProject.Application.Dtos.CreateDTOs.CompetitorCreateDTOs
{
    public class TeamCreateDTO : CompetitorCreateDTO
    {
        public required ushort TeamSize { get; set; }
        public List<ulong> Players { get; set; } = [];
    }
}
