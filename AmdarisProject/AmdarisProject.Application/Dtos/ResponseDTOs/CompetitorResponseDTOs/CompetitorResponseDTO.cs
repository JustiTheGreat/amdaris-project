using AmdarisProject.Application.Dtos.DisplayDTOs;

namespace AmdarisProject.Application.Dtos.ResponseDTOs.CompetitorResponseDTOs
{
    public abstract class CompetitorResponseDTO : ResponseDTO
    {
        public string Name { get; set; }
        public List<MatchDisplayDTO> Matches { get; set; } = [];
        public List<Guid> WonMatches { get; set; } = [];
        public List<CompetitionDisplayDTO> Competitions { get; set; } = [];
    }
}
