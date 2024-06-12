using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Dtos.ResponseDTOs.DisplayDTOs;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AmdarisProject.Application.Handlers.TeamPlayerHandlers
{
    public record ChangeTeamPlayerStatus(Guid TeamId, Guid PlayerId) : IRequest<TeamPlayerDisplayDTO>;
    public class ChangeTeamPlayerStatusHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<ChangeTeamPlayerStatusHandler> logger)
        : IRequestHandler<ChangeTeamPlayerStatus, TeamPlayerDisplayDTO>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<ChangeTeamPlayerStatusHandler> _logger = logger;

        public async Task<TeamPlayerDisplayDTO> Handle(ChangeTeamPlayerStatus request, CancellationToken cancellationToken)
        {
            TeamPlayer teamPlayer = await _unitOfWork.TeamPlayerRepository.GetByTeamAndPlayer(request.TeamId, request.PlayerId)
                ?? throw new APNotFoundException(
                    [Tuple.Create(nameof(request.TeamId), request.TeamId), Tuple.Create(nameof(request.PlayerId), request.PlayerId)]);

            teamPlayer.ChangeStatus();

            TeamPlayer updated;

            try
            {
                await _unitOfWork.BeginTransactionAsync();
                updated = await _unitOfWork.TeamPlayerRepository.Update(teamPlayer);
                await _unitOfWork.SaveAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }

            _logger.LogInformation("Changed activity status of player {PlayerName} from the team {TeamName} to {Status}!",
                [updated.Player.Name, updated.Team.Name, updated.IsActive ? "active" : "incative"]);

            TeamPlayerDisplayDTO response = _mapper.Map<TeamPlayerDisplayDTO>(updated);
            return response;
        }
    }
}
