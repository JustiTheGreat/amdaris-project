using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Dtos.ResponseDTOs.CompetitorResponseDTOs;
using AmdarisProject.Domain.Models.CompetitorModels;
using Mapster;
using MapsterMapper;
using MediatR;

namespace AmdarisProject.Application.Handlers.CompetitorHandlers
{
    public record GetTeams() : IRequest<IEnumerable<TeamResponseDTO>>;
    public class GetTeamsHandler(IUnitOfWork unitOfWork, IMapper mapper)
        : IRequestHandler<GetTeams, IEnumerable<TeamResponseDTO>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<IEnumerable<TeamResponseDTO>> Handle(GetTeams request, CancellationToken cancellationToken)
        {
            IEnumerable<Team> teams = await _unitOfWork.CompetitorRepository.GetAllTeams();
            IEnumerable<TeamResponseDTO> response = teams.Select(_mapper.Map<TeamResponseDTO>).ToList();
            return response;
        }
    }
}
