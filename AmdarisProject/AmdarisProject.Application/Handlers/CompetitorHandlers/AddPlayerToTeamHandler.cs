using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Dtos.ResponseDTOs.CompetitorResponseDTOs;
using AmdarisProject.Application.Utils;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models.CompetitorModels;
using Mapster;
using MediatR;

namespace AmdarisProject.Application.Handlers.CompetitorHandlers
{
    public record AddPlayerToTeam(ulong PlayerId, ulong TeamId) : IRequest<TeamResponseDTO>;
    public class AddPlayerToTeamHandler(IUnitOfWork unitOfWork)
        : IRequestHandler<AddPlayerToTeam, TeamResponseDTO>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<TeamResponseDTO> Handle(AddPlayerToTeam request, CancellationToken cancellationToken)
        {
            //TODO what kind of persistence a team should have (per competition?, or forever?)
            Team team = (Team)(await _unitOfWork.CompetitorRepository.GetById(request.TeamId)
                ?? throw new APNotFoundException(nameof(AddPlayerToTeamHandler), nameof(Handle),
                    [Tuple.Create(nameof(request.TeamId), request.TeamId)]));

            Player player = (Player)(await _unitOfWork.CompetitorRepository.GetById(request.PlayerId)
                ?? throw new APNotFoundException(nameof(AddPlayerToTeamHandler), nameof(Handle),
                    [Tuple.Create(nameof(request.PlayerId), request.PlayerId)]));

            if (team.Players.Count == team.TeamSize)
                throw new APCompetitorNumberException(nameof(AddPlayerToTeamHandler), nameof(Handle), $"Team {team.Id} is full!");

            if (HandlerUtils.TeamContainsPlayer(team, request.PlayerId))
                throw new APCompetitorException(nameof(AddPlayerToTeamHandler), nameof(Handle),
                    $"Player {request.PlayerId} is already a member of team {team.Id}!");

            Team updated;

            try
            {
                await _unitOfWork.BeginTransactionAsync();
                team.Players = [.. team.Players, player];
                updated = (Team)await _unitOfWork.CompetitorRepository.Update(team);
                await _unitOfWork.SaveAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }

            TeamResponseDTO response = updated.Adapt<TeamResponseDTO>();
            return response;
        }
    }
}
