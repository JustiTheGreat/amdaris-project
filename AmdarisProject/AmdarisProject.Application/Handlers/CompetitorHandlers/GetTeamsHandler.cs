using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Dtos.ResponseDTOs.CompetitorResponseDTOs;
using AmdarisProject.Domain.Models.CompetitorModels;
using Mapster;
using MediatR;

namespace AmdarisProject.Application.Handlers.CompetitorHandlers
{
    public record GetTeams() : IRequest<IEnumerable<TeamResponseDTO>>;
    public class GetTeamsHandler(IUnitOfWork unitOfWork)
        : IRequestHandler<GetTeams, IEnumerable<TeamResponseDTO>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<IEnumerable<TeamResponseDTO>> Handle(GetTeams request, CancellationToken cancellationToken)
        {
            IEnumerable<Team> teams = await _unitOfWork.CompetitorRepository.GetAllTeams();
            IEnumerable<TeamResponseDTO> response = teams.Select(team => team.Adapt<TeamResponseDTO>()).ToList();
            return response;
        }
    }
}
