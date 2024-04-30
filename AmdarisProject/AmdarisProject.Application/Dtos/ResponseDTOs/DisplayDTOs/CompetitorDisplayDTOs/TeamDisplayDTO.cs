namespace AmdarisProject.Application.Dtos.DisplayDTOs.CompetitorDisplayDTOs
{
    public class TeamDisplayDTO : CompetitorDisplayDTO
    {
        public required List<string> PlayerNames { get; set; } = [];
    }
}
