using AmdarisProject.Application.Abstractions;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AmdarisProject.Application.Handlers.TeamPlayerHandlers
{
    public record RemovePlayerFromTeam(Guid TeamId, Guid PlayerId) : IRequest<bool>;
    public class RemovePlayerFromTeamHandler(IUnitOfWork unitOfWork, ILogger<RemovePlayerFromTeamHandler> logger)
        : IRequestHandler<RemovePlayerFromTeam, bool>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ILogger<RemovePlayerFromTeamHandler> _logger = logger;

        public async Task<bool> Handle(RemovePlayerFromTeam request, CancellationToken cancellationToken)
        {
            TeamPlayer teamPlayer = await _unitOfWork.TeamPlayerRepository.GetByTeamAndPlayer(request.TeamId, request.PlayerId)
                ?? throw new APNotFoundException(
                    [Tuple.Create(nameof(request.TeamId), request.TeamId), Tuple.Create(nameof(request.PlayerId), request.PlayerId)]);

            if (teamPlayer.Team.IsInAStartedMatch())
                throw new APException($"Team {request.TeamId} is in a started match!");

            try
            {
                await _unitOfWork.BeginTransactionAsync();
                await _unitOfWork.TeamPlayerRepository.Delete(teamPlayer.Id);
                await _unitOfWork.SaveAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }

            _logger.LogInformation("Player {PlayerName} was removed from the team {TeamName}!",
                [teamPlayer.Player.Name, teamPlayer.Team.Name]);

            return true;
        }
    }
}
