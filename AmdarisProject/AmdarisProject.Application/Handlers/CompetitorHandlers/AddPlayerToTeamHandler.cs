using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Dtos.ResponseDTOs.CompetitorResponseDTOs;
using AmdarisProject.Application.Utils;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models.CompetitorModels;
using MapsterMapper;
using MediatR;

namespace AmdarisProject.Application.Handlers.CompetitorHandlers
{
    public record AddPlayerToTeam(ulong PlayerId, ulong TeamId) : IRequest<TeamResponseDTO>;
    public class AddPlayerToTeamHandler(IUnitOfWork unitOfWork, IMapper mapper)
        : IRequestHandler<AddPlayerToTeam, TeamResponseDTO>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<TeamResponseDTO> Handle(AddPlayerToTeam request, CancellationToken cancellationToken)
        {
            //TODO what kind of persistence a team should have (per competition?, or forever?)
            Team team = (Team)(await _unitOfWork.CompetitorRepository.GetById(request.TeamId)
                ?? throw new APNotFoundException(Tuple.Create(nameof(request.TeamId), request.TeamId)));

            Player player = (Player)(await _unitOfWork.CompetitorRepository.GetById(request.PlayerId)
                ?? throw new APNotFoundException(Tuple.Create(nameof(request.PlayerId), request.PlayerId)));

            if (team.Players.Count == team.TeamSize)
                throw new AmdarisProjectException($"Team {team.Id} is full!");

            if (HandlerUtils.TeamContainsPlayer(team, request.PlayerId))
                throw new AmdarisProjectException($"Player {player.Id} is already a member of team {team.Id}!");

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

            TeamResponseDTO response = _mapper.Map<TeamResponseDTO>(team);
            return response;
        }
    }
}
