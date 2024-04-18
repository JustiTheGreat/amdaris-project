namespace AmdarisProject.Application.Dtos.ResponseDTOs
{
    public class PointResponseDTO : ResponseDTO
    {
        public uint Value { get; set; }
        public Guid? Match { get; set; }
        public Guid? Player { get; set; }
    }
}
