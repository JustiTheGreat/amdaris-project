namespace AmdarisProject.Application.Dtos.CreateDTOs.CompetitorCreateDTOs
{
    public class TeamCreateDTO : CompetitorCreateDTO
    {
        public required ushort TeamSize { get; set; }
        public List<Guid> Players { get; set; } = [];
    }
}
