using AmdarisProject.Application.Dtos.DisplayDTOs;

namespace AmdarisProject.Application.Dtos.ResponseDTOs.CompetitorResponseDTOs
{
    public abstract class CompetitorGetDTO : GetDTO
    {
        public required string Name { get; set; }
        public required List<MatchDisplayDTO> Matches { get; set; } = [];
        public required List<Guid> WonMatches { get; set; } = [];
        public required List<CompetitionDisplayDTO> Competitions { get; set; } = [];
    }
}
