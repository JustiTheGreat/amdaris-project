using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Dtos.DisplayDTOs.CompetitorDisplayDTOs;
using AmdarisProject.Domain.Models.CompetitorModels;
using MapsterMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AmdarisProject.Application.Handlers.CompetitorHandlers
{
    public record GetAllPlayers() : IRequest<IEnumerable<PlayerDisplayDTO>>;
    public class GetAllPlayersHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetAllPlayersHandler> logger)
        : IRequestHandler<GetAllPlayers, IEnumerable<PlayerDisplayDTO>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<GetAllPlayersHandler> _logger = logger;

        public async Task<IEnumerable<PlayerDisplayDTO>> Handle(GetAllPlayers request, CancellationToken cancellationToken)
        {
            IEnumerable<Player> players = await _unitOfWork.CompetitorRepository.GetAllPlayers();
            _logger.LogInformation("Got all players (Count = {Count})!", [players.Count()]);
            IEnumerable<PlayerDisplayDTO> response = _mapper.Map<IEnumerable<PlayerDisplayDTO>>(players);
            return response;
        }
    }
}
