using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Dtos.DisplayDTOs.CompetitorDisplayDTOs;
using AmdarisProject.Domain.Models.CompetitorModels;
using MapsterMapper;
using MediatR;

namespace AmdarisProject.Application.Handlers.CompetitorHandlers
{
    public record GetAllTeams() : IRequest<IEnumerable<TeamDisplayDTO>>;
    public class GetAllTeamsHandler(IUnitOfWork unitOfWork, IMapper mapper)
        : IRequestHandler<GetAllTeams, IEnumerable<TeamDisplayDTO>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<IEnumerable<TeamDisplayDTO>> Handle(GetAllTeams request, CancellationToken cancellationToken)
        {
            IEnumerable<Team> teams = await _unitOfWork.CompetitorRepository.GetAllTeams();
            IEnumerable<TeamDisplayDTO> response = _mapper.Map<List<TeamDisplayDTO>>(teams);
            return response;
        }
    }
}
