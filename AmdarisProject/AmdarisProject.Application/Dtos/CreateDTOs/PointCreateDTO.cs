namespace AmdarisProject.Application.Dtos.CreateDTOs
{
    public class PointCreateDTO : CreateDTO
    {
        public required uint Value { get; set; }
        public required Guid Match { get; set; }
        public required Guid Player { get; set; }
    }
}
