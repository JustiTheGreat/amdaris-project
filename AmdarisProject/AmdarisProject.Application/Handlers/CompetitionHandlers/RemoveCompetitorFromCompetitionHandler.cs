using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Dtos.ResponseDTOs.CompetitionResponseDTOs;
using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Extensions;
using AmdarisProject.Domain.Models.CompetitionModels;
using AmdarisProject.Domain.Models.CompetitorModels;
using MapsterMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AmdarisProject.Application.Handlers.CompetitionHandlers
{
    public record RemoveCompetitorFromCompetition(Guid CompetitorId, Guid CompetitionId) : IRequest<CompetitionGetDTO>;
    public class RemoveCompetitorFromCompetitionHandler(IUnitOfWork unitOfWork, IMapper mapper,
        ILogger<RemoveCompetitorFromCompetitionHandler> logger)
        : IRequestHandler<RemoveCompetitorFromCompetition, CompetitionGetDTO>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<RemoveCompetitorFromCompetitionHandler> _logger = logger;

        public async Task<CompetitionGetDTO> Handle(RemoveCompetitorFromCompetition request, CancellationToken cancellationToken)
        {
            Competition competition = await _unitOfWork.CompetitionRepository.GetById(request.CompetitionId)
                ?? throw new APNotFoundException(Tuple.Create(nameof(request.CompetitionId), request.CompetitionId));

            if (competition.Status is not CompetitionStatus.ORGANIZING)
                throw new APIllegalStatusException(competition.Status);

            Competitor competitor = await _unitOfWork.CompetitorRepository.GetById(request.CompetitorId)
                    ?? throw new APNotFoundException(Tuple.Create(nameof(request.CompetitorId), request.CompetitorId));

            if (competitor is Player && competition.GameFormat.CompetitorType is not CompetitorType.PLAYER
                || competitor is Team && competition.GameFormat.CompetitorType is not CompetitorType.TEAM)
                throw new AmdarisProjectException($"Tried to remove {competitor.GetType().Name} " +
                    $"from competition with {competition.GameFormat.CompetitorType} competitor type!");

            if (!competition.ContainsCompetitor(request.CompetitorId))
                throw new AmdarisProjectException($"Competitor {competitor.Id} is not registered to competition {competition.Id}!");

            Competition updated;

            try
            {
                await _unitOfWork.BeginTransactionAsync();
                competition.Competitors = competition.Competitors.Where(c => !c.Id.Equals(competitor.Id)).ToList();
                updated = await _unitOfWork.CompetitionRepository.Update(competition);
                await _unitOfWork.SaveAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }

            _logger.LogInformation("Competitor {CompetitorName} has been removed from competition {CompetitionName}!",
                [competitor.Name, competition.Name]);

            CompetitionGetDTO response = updated is OneVSAllCompetition ? _mapper.Map<OneVSAllCompetitionGetDTO>(updated)
                : updated is TournamentCompetition ? _mapper.Map<TournamentCompetitionGetDTO>(updated)
                : throw new AmdarisProjectException(nameof(updated));

            return response;
        }
    }
}
