namespace AmdarisProject.Application.Dtos.ResponseDTOs.DisplayDTOs
{
    public class PointDisplayDTO : DisplayDTO
    {
        public required int Value { get; set; }
        public required Guid MatchId { get; set; }
        public required Guid PlayerId { get; set; }
        public required string Player { get; set; }
    }
}
