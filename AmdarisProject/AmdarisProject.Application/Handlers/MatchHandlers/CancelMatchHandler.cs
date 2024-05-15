using AmdarisProject.Application.Common.Abstractions;
using AmdarisProject.Application.Dtos.ResponseDTOs;
using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models;
using MapsterMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AmdarisProject.Application.Handlers.MatchHandlers
{
    public record CancelMatch(Guid MatchId) : IRequest<MatchGetDTO>;
    public class CancelMatchHandler(IUnitOfWork unitOfWork, IMapper mapper,
        ICompetitionMatchCreatorFactoryService competitionMatchCreatorFactoryService,
        ILogger<CancelMatchHandler> logger)
        : IRequestHandler<CancelMatch, MatchGetDTO>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ICompetitionMatchCreatorFactoryService _competitionMatchCreatorFactoryService = competitionMatchCreatorFactoryService;
        private readonly ILogger<CancelMatchHandler> _logger = logger;

        public async Task<MatchGetDTO> Handle(CancelMatch request, CancellationToken cancellationToken)
        {
            Match match = await _unitOfWork.MatchRepository.GetById(request.MatchId)
                ?? throw new APNotFoundException(Tuple.Create(nameof(request.MatchId), request.MatchId));

            if (match.Status is not MatchStatus.NOT_STARTED && match.Status is not MatchStatus.STARTED)
                throw new APIllegalStatusException(match.Status);

            Match updated;

            try
            {
                await _unitOfWork.BeginTransactionAsync();
                match.Status = MatchStatus.CANCELED;
                updated = await _unitOfWork.MatchRepository.Update(match);

                await _competitionMatchCreatorFactoryService
                    .GetCompetitionMatchCreator(updated.Competition.GetType())
                    .CreateCompetitionMatches(updated.Competition.Id);

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

            _logger.LogInformation("Match {CompetitorOneName}-{CompetitorTwoName} was cancelled!",
                [match.CompetitorOne.Name, match.CompetitorTwo.Name]);

            MatchGetDTO response = _mapper.Map<MatchGetDTO>(updated);
            return response;
        }
    }
}
