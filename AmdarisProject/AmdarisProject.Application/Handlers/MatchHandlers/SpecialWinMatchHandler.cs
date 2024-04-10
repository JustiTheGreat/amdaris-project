using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Dtos.CreateDTOs;
using AmdarisProject.Application.Dtos.ResponseDTOs;
using AmdarisProject.Application.Utils;
using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models;
using Mapster;
using MediatR;

namespace AmdarisProject.Application.Handlers.MatchHandlers
{
    public record SpecialWinMatch(ulong MatchId, MatchCompetitor MatchCompetitor) : IRequest<MatchResponseDTO>;
    public class SpecialWinMatchHandler(IUnitOfWork unitOfWork)
        : IRequestHandler<SpecialWinMatch, MatchResponseDTO>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<MatchResponseDTO> Handle(SpecialWinMatch request, CancellationToken cancellationToken)
        {
            Match match = await _unitOfWork.MatchRepository.GetById(request.MatchId)
                ?? throw new APNotFoundException(Tuple.Create(nameof(request.MatchId), request.MatchId));

            if (match.Status is not MatchStatus.STARTED)
                throw new APIllegalStatusException(match.Status);

            match.Status = request.MatchCompetitor is MatchCompetitor.COMPETITOR_ONE
                    ? MatchStatus.SPECIAL_WIN_COMPETITOR_ONE
                    : MatchStatus.SPECIAL_WIN_COMPETITOR_TWO;
            match.EndTime = DateTime.Now;

            Match updated;

            try
            {
                await _unitOfWork.BeginTransactionAsync();
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

            MatchResponseDTO response = updated.Adapt<MatchResponseDTO>();
            return response;
        }
    }
}
