using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Dtos.ResponseDTOs.CompetitorResponseDTOs;
using AmdarisProject.Domain.Models.CompetitorModels;
using Mapster;
using MediatR;

namespace AmdarisProject.Application.Handlers.CompetitorHandlers
{
    public record GetPlayers() : IRequest<IEnumerable<PlayerResponseDTO>>;
    public class GetPlayersHandler(IUnitOfWork unitOfWork)
        : IRequestHandler<GetPlayers, IEnumerable<PlayerResponseDTO>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<IEnumerable<PlayerResponseDTO>> Handle(GetPlayers request, CancellationToken cancellationToken)
        {
            IEnumerable<Player> players = await _unitOfWork.CompetitorRepository.GetAllPlayers();
            IEnumerable<PlayerResponseDTO> response = players.Select(player => player.Adapt<PlayerResponseDTO>()).ToList();
            return response;
        }
    }
}
