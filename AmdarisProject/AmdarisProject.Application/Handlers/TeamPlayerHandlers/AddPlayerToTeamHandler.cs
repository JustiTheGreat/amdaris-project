using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Dtos.ResponseDTOs.GetDTOs;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Extensions;
using AmdarisProject.Domain.Models.CompetitorModels;
using MapsterMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace AmdarisProject.Application.Handlers.TeamPlayerHandlers
{
    public record AddPlayerToTeam(Guid PlayerId, Guid TeamId) : IRequest<TeamPlayerGetDTO>;
    public class AddPlayerToTeamHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<AddPlayerToTeamHandler> logger)
        : IRequestHandler<AddPlayerToTeam, TeamPlayerGetDTO>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<AddPlayerToTeamHandler> _logger = logger;

        public async Task<TeamPlayerGetDTO> Handle(AddPlayerToTeam request, CancellationToken cancellationToken)
        {
            Team team = await _unitOfWork.CompetitorRepository.GetTeamById(request.TeamId)
                ?? throw new APNotFoundException(Tuple.Create(nameof(request.TeamId), request.TeamId));

            Player player = await _unitOfWork.CompetitorRepository.GetPlayerById(request.PlayerId)
                ?? throw new APNotFoundException(Tuple.Create(nameof(request.PlayerId), request.PlayerId));

            if (team.ContainsPlayer(request.PlayerId))
                throw new AmdarisProjectException($"Player {player.Id} is already a member of team {team.Id}!");

            if (await _unitOfWork.CompetitorRepository.PlayerIsInATeam(player.Id))
                throw new AmdarisProjectException($"Player {player.Id} is already a member of a team!");

            TeamPlayer created;

            try
            {
                await _unitOfWork.BeginTransactionAsync();
                //TODO use create DTO to respect convention?
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

            _logger.LogInformation("Added an {Status} player {PlayerName} to the team {TeamName}!",
                [created.IsActive ? "active" : "inactive", created.Player.Name, created.Team.Name]);

            TeamPlayerGetDTO response = _mapper.Map<TeamPlayerGetDTO>(created);
            return response;
        }
    }
}
