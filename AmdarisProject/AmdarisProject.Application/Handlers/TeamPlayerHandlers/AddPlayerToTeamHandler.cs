using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Dtos.ResponseDTOs.DisplayDTOs;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models;
using AmdarisProject.Domain.Models.CompetitorModels;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AmdarisProject.Application.Handlers.TeamPlayerHandlers
{
    public record AddPlayerToTeam(Guid TeamId, Guid PlayerId) : IRequest<TeamPlayerDisplayDTO>;
    public class AddPlayerToTeamHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<AddPlayerToTeamHandler> logger)
        : IRequestHandler<AddPlayerToTeam, TeamPlayerDisplayDTO>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<AddPlayerToTeamHandler> _logger = logger;

        public async Task<TeamPlayerDisplayDTO> Handle(AddPlayerToTeam request, CancellationToken cancellationToken)
        {
            Team team = await _unitOfWork.CompetitorRepository.GetTeamById(request.TeamId)
                ?? throw new APNotFoundException(Tuple.Create(nameof(request.TeamId), request.TeamId));

            Player player = await _unitOfWork.CompetitorRepository.GetPlayerById(request.PlayerId)
                ?? throw new APNotFoundException(Tuple.Create(nameof(request.PlayerId), request.PlayerId));

            if (team.ContainsPlayer(player.Id))
                throw new APException($"Player {player.Name} is already a member of team {team.Name}!");

            if (player.Teams.Any())
                throw new APException($"Player {player.Name} is already a member of another team!");

            TeamPlayer created;

            try
            {
                await _unitOfWork.BeginTransactionAsync();
                created = await _unitOfWork.TeamPlayerRepository.Create(
                    new() { Team = team, Player = player, IsActive = false });
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

            TeamPlayerDisplayDTO response = _mapper.Map<TeamPlayerDisplayDTO>(created);
            return response;
        }
    }
}
