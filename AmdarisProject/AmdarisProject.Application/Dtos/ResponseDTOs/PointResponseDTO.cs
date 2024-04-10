namespace AmdarisProject.Application.Dtos.ResponseDTOs
{
    public class PointResponseDTO : ResponseDTO
    {
        public uint? Value { get; set; }
        public ulong? Match { get; set; }
        public ulong? Player { get; set; }
    }
}
