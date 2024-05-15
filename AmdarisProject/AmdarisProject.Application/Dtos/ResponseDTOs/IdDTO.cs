namespace AmdarisProject.Application.Dtos.ResponseDTOs
{
    public abstract class IdDTO : ResponseDTO
    {
        public required Guid Id { get; set; }
    }
}
