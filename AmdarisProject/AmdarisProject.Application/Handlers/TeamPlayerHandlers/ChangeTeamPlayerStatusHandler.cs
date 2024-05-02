using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Dtos.ResponseDTOs.GetDTOs;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models.CompetitorModels;
using MapsterMapper;
using MediatR;

namespace AmdarisProject.Application.Handlers.TeamPlayerHandlers
{
    public record ChangeTeamPlayerStatus(Guid TeamId, Guid PlayerId, bool Active) : IRequest<TeamPlayerGetDTO>;
    public class ChangeTeamPlayerStatusHandler(IUnitOfWork unitOfWork, IMapper mapper)
        : IRequestHandler<ChangeTeamPlayerStatus, TeamPlayerGetDTO>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<TeamPlayerGetDTO> Handle(ChangeTeamPlayerStatus request, CancellationToken cancellationToken)
        {
            TeamPlayer teamPlayer = await _unitOfWork.TeamPlayerRepository.GetByTeamAndPlayer(request.TeamId, request.PlayerId)
                ?? throw new APNotFoundException(
                    [Tuple.Create(nameof(request.TeamId), request.TeamId), Tuple.Create(nameof(request.PlayerId), request.PlayerId)]);

            if (await _unitOfWork.MatchRepository.TeamIsInAStartedMatch(request.TeamId))
                throw new AmdarisProjectException($"Team {request.TeamId} is in a started match!");

            TeamPlayer updated;

            try
            {
                await _unitOfWork.BeginTransactionAsync();
                teamPlayer.IsActive = request.Active;
                updated = await _unitOfWork.TeamPlayerRepository.Update(teamPlayer);
                await _unitOfWork.SaveAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }

            TeamPlayerGetDTO response = _mapper.Map<TeamPlayerGetDTO>(updated);
            return response;
        }
    }
}
