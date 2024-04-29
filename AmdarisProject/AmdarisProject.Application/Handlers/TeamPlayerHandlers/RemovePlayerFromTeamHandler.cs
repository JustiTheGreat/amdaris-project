using AmdarisProject.Application.Abstractions;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models.CompetitorModels;
using MediatR;

namespace AmdarisProject.Application.Handlers.TeamPlayerHandlers
{
    public record RemovePlayerFromTeam(Guid PlayerId, Guid TeamId) : IRequest<bool>;
    public class RemovePlayerFromTeamHandler(IUnitOfWork unitOfWork)
        : IRequestHandler<RemovePlayerFromTeam, bool>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<bool> Handle(RemovePlayerFromTeam request, CancellationToken cancellationToken)
        {
            TeamPlayer teamPlayer = await _unitOfWork.TeamPlayerRepository.GetByTeamAndPlayer(request.TeamId, request.PlayerId)
                ?? throw new APNotFoundException(
                    [Tuple.Create(nameof(request.TeamId), request.TeamId), Tuple.Create(nameof(request.PlayerId), request.PlayerId)]);

            if (await _unitOfWork.MatchRepository.TeamIsInAStartedMatch(request.TeamId))
                throw new AmdarisProjectException($"Team {request.TeamId} is in a started match!");

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

            return true;
        }
    }
}
