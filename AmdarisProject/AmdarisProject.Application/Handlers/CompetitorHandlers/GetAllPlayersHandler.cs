using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Dtos.DisplayDTOs.CompetitorDisplayDTOs;
using AmdarisProject.Domain.Models.CompetitorModels;
using MapsterMapper;
using MediatR;

namespace AmdarisProject.Application.Handlers.CompetitorHandlers
{
    public record GetPlayers() : IRequest<IEnumerable<PlayerDisplayDTO>>;
    public class GetAllPlayersHandler(IUnitOfWork unitOfWork, IMapper mapper)
        : IRequestHandler<GetPlayers, IEnumerable<PlayerDisplayDTO>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<IEnumerable<PlayerDisplayDTO>> Handle(GetPlayers request, CancellationToken cancellationToken)
        {
            IEnumerable<Player> players = await _unitOfWork.CompetitorRepository.GetAllPlayers();
            IEnumerable<PlayerDisplayDTO> response = _mapper.Map<List<PlayerDisplayDTO>>(players);
            return response;
        }
    }
}
