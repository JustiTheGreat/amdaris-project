namespace AmdarisProject.Application.Dtos.ResponseDTOs.CompetitorResponseDTOs
{
    public class TeamResponseDTO : CompetitorResponseDTO
    {
        public ushort? TeamSize { get; set; }
        public List<ulong> Players { get; set; } = [];
    }
}
