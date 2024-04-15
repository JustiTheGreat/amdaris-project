namespace AmdarisProject.Application.Dtos.CreateDTOs
{
    public class PointCreateDTO : CreateDTO
    {
        public required uint Value { get; set; }
        public required ulong Match { get; set; }
        public required ulong Player { get; set; }
    }
}
