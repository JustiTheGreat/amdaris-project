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
    public record AddCompetitorToCompetition(Guid CompetitorId, Guid CompetitionId) : IRequest<CompetitionGetDTO>;
    public class AddCompetitorToCompetitionHandler(IUnitOfWork unitOfWork, IMapper mapper,
        ILogger<AddCompetitorToCompetitionHandler> logger)
        : IRequestHandler<AddCompetitorToCompetition, CompetitionGetDTO>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<AddCompetitorToCompetitionHandler> _logger = logger;

        public async Task<CompetitionGetDTO> Handle(AddCompetitorToCompetition request, CancellationToken cancellationToken)
        {
            Competition competition = await _unitOfWork.CompetitionRepository.GetById(request.CompetitionId)
                ?? throw new APNotFoundException(Tuple.Create(nameof(request.CompetitionId), request.CompetitionId));

            if (competition.Status is not CompetitionStatus.ORGANIZING)
                throw new APIllegalStatusException(competition.Status);

            Competitor competitor = await _unitOfWork.CompetitorRepository.GetById(request.CompetitorId)
                    ?? throw new APNotFoundException(Tuple.Create(nameof(request.CompetitorId), request.CompetitorId));

            if (competitor is Player && competition.GameFormat.CompetitorType is not CompetitorType.PLAYER
                || competitor is Team && competition.GameFormat.CompetitorType is not CompetitorType.TEAM)
                throw new AmdarisProjectException($"Tried to add {competitor.GetType().Name} " +
                    $"to competition with {competition.GameFormat.CompetitorType} competitor type!");

            if (competition.ContainsCompetitor(request.CompetitorId))
                throw new AmdarisProjectException($"Competitor {competitor.Id} is already registered to competition {competition.Id}!");

            if (competition.GameFormat.CompetitorType is CompetitorType.TEAM
                && !await _unitOfWork.TeamPlayerRepository.TeamHasTheRequiredNumberOfActivePlayers(
                    competitor.Id, (uint)competition.GameFormat.TeamSize!))
                throw new AmdarisProjectException($"Team {competitor.Id} doesn't have the required number of active competitors!");

            bool teamContainsAPlayerPartOfAnotherTeamFromCompetition = competitor is Team team
                    && team.Players.Any(player => competition.Competitors.Any(competitor => ((Team)competitor).ContainsPlayer(player.Id)));

            if (teamContainsAPlayerPartOfAnotherTeamFromCompetition)
                throw new AmdarisProjectException($"Team {competitor.Id} is contains a player part of another team from competition {competition.Id}!");

            Competition updated;

            try
            {
                await _unitOfWork.BeginTransactionAsync();
                competition.Competitors = [.. competition.Competitors, competitor];
                updated = await _unitOfWork.CompetitionRepository.Update(competition);
                await _unitOfWork.SaveAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }

            _logger.LogInformation("Competitor {CompetitorName} has been registered to competition {CompetitionName}!",
                [competitor.Name, competition.Name]);

            CompetitionGetDTO response = updated is OneVSAllCompetition ? _mapper.Map<OneVSAllCompetitionGetDTO>(updated)
                : updated is TournamentCompetition ? _mapper.Map<TournamentCompetitionGetDTO>(updated)
                : throw new AmdarisProjectException(nameof(updated));

            return response;
        }
    }
}
