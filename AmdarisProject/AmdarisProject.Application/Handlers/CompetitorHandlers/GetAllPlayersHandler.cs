using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Dtos.DisplayDTOs.CompetitorDisplayDTOs;
using AmdarisProject.Domain.Models.CompetitorModels;
using MapsterMapper;
using MediatR;

namespace AmdarisProject.Application.Handlers.CompetitorHandlers
{
    public record GetAllPlayers() : IRequest<IEnumerable<PlayerDisplayDTO>>;
    public class GetAllPlayersHandler(IUnitOfWork unitOfWork, IMapper mapper)
        : IRequestHandler<GetAllPlayers, IEnumerable<PlayerDisplayDTO>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<IEnumerable<PlayerDisplayDTO>> Handle(GetAllPlayers request, CancellationToken cancellationToken)
        {
            IEnumerable<Player> players = await _unitOfWork.CompetitorRepository.GetAllPlayers();
            IEnumerable<PlayerDisplayDTO> response = _mapper.Map<IEnumerable<PlayerDisplayDTO>>(players);
            return response;
        }
    }
}
