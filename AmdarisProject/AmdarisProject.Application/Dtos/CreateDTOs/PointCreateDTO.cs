namespace AmdarisProject.Application.Dtos.CreateDTOs
{
    public class PointCreateDTO : CreateDTO
    {
        public required uint Value { get; init; }
        public required ulong Match { get; init; }
        public required ulong Player { get; init; }
    }
}
