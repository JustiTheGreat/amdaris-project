namespace AmdarisProject.Application.Dtos.CreateDTOs.CompetitorCreateDTOs
{
    public class PlayerCreateDTO : CompetitorCreateDTO
    {
        public List<ulong> Points { get; set; } = [];
        public List<ulong> Teams { get; set; } = [];
    }
}
