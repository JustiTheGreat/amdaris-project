using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Dtos.ResponseDTOs;
using AmdarisProject.Application.Utils;
using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models;
using MapsterMapper;
using MediatR;

namespace AmdarisProject.Application.Handlers.MatchHandlers
{
    public record CancelMatch(Guid MatchId) : IRequest<MatchResponseDTO>;
    public class CancelMatchHandler(IUnitOfWork unitOfWork, IMapper mapper)
        : IRequestHandler<CancelMatch, MatchResponseDTO>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<MatchResponseDTO> Handle(CancelMatch request, CancellationToken cancellationToken)
        {
            Match match = await _unitOfWork.MatchRepository.GetById(request.MatchId)
                ?? throw new APNotFoundException(Tuple.Create(nameof(request.MatchId), request.MatchId));

            if (match.Status is not MatchStatus.NOT_STARTED or MatchStatus.STARTED)
                throw new APIllegalStatusException(match.Status);

            Match updated;

            try
            {
                await _unitOfWork.BeginTransactionAsync();
                match.Status = MatchStatus.CANCELED;
                updated = await _unitOfWork.MatchRepository.Update(match);

                await HandlerUtils.CreateCompetitionMatchesUtil(_unitOfWork, _mapper, updated.Competition.Id);

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

            MatchResponseDTO response = _mapper.Map<MatchResponseDTO>(updated);
            return response;
        }
    }
}
