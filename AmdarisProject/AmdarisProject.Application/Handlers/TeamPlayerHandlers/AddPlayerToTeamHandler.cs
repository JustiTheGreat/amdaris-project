using AmdarisProject.Application.Abstractions;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Extensions;
using AmdarisProject.Domain.Models.CompetitorModels;
using MediatR;

namespace AmdarisProject.Application.Handlers.TeamPlayerHandlers
{
    public record AddPlayerToTeam(Guid PlayerId, Guid TeamId) : IRequest<TeamPlayer>;
    public class AddPlayerToTeamHandler(IUnitOfWork unitOfWork)
        : IRequestHandler<AddPlayerToTeam, TeamPlayer>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<TeamPlayer> Handle(AddPlayerToTeam request, CancellationToken cancellationToken)
        {
            Team team = (Team)(await _unitOfWork.CompetitorRepository.GetById(request.TeamId)
                ?? throw new APNotFoundException(Tuple.Create(nameof(request.TeamId), request.TeamId)));

            Player player = (Player)(await _unitOfWork.CompetitorRepository.GetById(request.PlayerId)
                ?? throw new APNotFoundException(Tuple.Create(nameof(request.PlayerId), request.PlayerId)));

            if (team.ContainsPlayer(request.PlayerId))
                throw new AmdarisProjectException($"Player {player.Id} is already a member of team {team.Id}!");

            if (await _unitOfWork.CompetitorRepository.PlayerIsInATeam(player.Id))
                throw new AmdarisProjectException($"Player {player.Id} is already a member of a team!");

            TeamPlayer created;

            try
            {
                await _unitOfWork.BeginTransactionAsync();
                TeamPlayer teamPlayer = new() { Team = team, Player = player, IsActive = false };
                created = await _unitOfWork.TeamPlayerRepository.Create(teamPlayer);
                await _unitOfWork.SaveAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }

            return created;
        }
    }
}
