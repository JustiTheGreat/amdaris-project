namespace AmdarisProject.Application.Dtos.CreateDTOs.CompetitorCreateDTOs
{
    public class PlayerCreateDTO : CompetitorCreateDTO
    {
        public List<Guid> Points { get; set; } = [];
        public List<Guid> Teams { get; set; } = [];
    }
}
