using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Dtos.ResponseDTOs.CompetitionResponseDTOs;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models.CompetitionModels;
using AmdarisProject.Domain.Models.CompetitorModels;
using AutoMapper;
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

            Competitor competitor = await _unitOfWork.CompetitorRepository.GetById(request.CompetitorId)
                    ?? throw new APNotFoundException(Tuple.Create(nameof(request.CompetitorId), request.CompetitorId));

            competition.AddCompetitor(competitor);

            Competition updated;

            try
            {
                await _unitOfWork.BeginTransactionAsync();
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
                : throw new APException(nameof(updated));
            return response;
        }
    }
}
