using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Dtos.ResponseDTOs.CompetitionResponseDTOs;
using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Extensions;
using AmdarisProject.Domain.Models.CompetitionModels;
using AmdarisProject.Domain.Models.CompetitorModels;
using MapsterMapper;
using MediatR;

namespace AmdarisProject.Application.Handlers.CompetitionHandlers
{
    public record RemoveCompetitorFromCompetition(Guid CompetitorId, Guid CompetitionId) : IRequest<CompetitionResponseDTO>;
    public class RemoveCompetitorFromCompetitionHandler(IUnitOfWork unitOfWork, IMapper mapper)
        : IRequestHandler<RemoveCompetitorFromCompetition, CompetitionResponseDTO>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<CompetitionResponseDTO> Handle(RemoveCompetitorFromCompetition request, CancellationToken cancellationToken)
        {
            Competition competition = await _unitOfWork.CompetitionRepository.GetById(request.CompetitionId)
                ?? throw new APNotFoundException(Tuple.Create(nameof(request.CompetitionId), request.CompetitionId));

            if (competition.Status is not CompetitionStatus.STARTED)
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

            CompetitionResponseDTO response = updated is OneVSAllCompetition ? _mapper.Map<OneVSAllCompetitionResponseDTO>(updated)
                : updated is TournamentCompetition ? _mapper.Map<TournamentCompetitionResponseDTO>(updated)
                : throw new AmdarisProjectException(nameof(updated));

            return response;
        }
    }
}
