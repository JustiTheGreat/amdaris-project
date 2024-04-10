using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Dtos.ResponseDTOs;
using AmdarisProject.Application.Utils;
using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models;
using Mapster;
using MediatR;

namespace AmdarisProject.Application.Handlers.MatchHandlers
{
    public record EndMatch(ulong MatchId) : IRequest<MatchResponseDTO>;
    public class EndMatchHandler(IUnitOfWork unitOfWork)
        : IRequestHandler<EndMatch, MatchResponseDTO>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<MatchResponseDTO> Handle(EndMatch request, CancellationToken cancellationToken)
        {
            Match match = await _unitOfWork.MatchRepository.GetById(request.MatchId)
                ?? throw new APNotFoundException(Tuple.Create(nameof(request.MatchId), request.MatchId));

            if (match.Status is not MatchStatus.STARTED)
                throw new APIllegalStatusException(match.Status);

            if (!await HandlerUtils.MatchHasACompetitorWithTheWinningScoreUtil(_unitOfWork, match.Id))
                throw new AmdarisProjectException($"Match {match.Id} doesn't have a competitor with the winning number of points!");

            Match updated;

            try
            {
                await _unitOfWork.BeginTransactionAsync();
                match.Status = MatchStatus.FINISHED;
                match.EndTime = DateTime.Now;
                updated = await _unitOfWork.MatchRepository.Update(match);

                //TODO CreateBonusMatches
                await HandlerUtils.CreateCompetitionMatchesUtil(_unitOfWork, updated.Competition.Id);

                await _unitOfWork.SaveAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }

            updated = await _unitOfWork.MatchRepository.GetById(updated.Id)
                ?? throw new APNotFoundException(Tuple.Create(nameof(updated.Id), updated.Id));

            //TODO remove
            uint competitorOneMatchPoints = await HandlerUtils.GetCompetitorMatchPointsUtil(_unitOfWork, updated.Id, updated.CompetitorOne.Id);
            uint competitorTwoMatchPoints = await HandlerUtils.GetCompetitorMatchPointsUtil(_unitOfWork, updated.Id, updated.CompetitorTwo.Id);
            Console.WriteLine($"Competition {match.Competition.Name}: Match between " +
                $"{match.CompetitorOne.Name} and {match.CompetitorTwo.Name} has ended with score " +
                $"{competitorOneMatchPoints}-{competitorTwoMatchPoints}!");
            //

            MatchResponseDTO response = updated.Adapt<MatchResponseDTO>();
            return response;
        }
    }
}
