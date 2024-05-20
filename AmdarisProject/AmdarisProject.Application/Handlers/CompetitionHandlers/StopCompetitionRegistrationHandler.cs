using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Dtos.ResponseDTOs.CompetitionResponseDTOs;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models.CompetitionModels;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AmdarisProject.handlers.competition
{
    public record StopCompetitionRegistration(Guid CompetitionId) : IRequest<CompetitionGetDTO>;
    public class StopCompetitionRegistrationHandler(IUnitOfWork unitOfWork, IMapper mapper,
        ICompetitionMatchCreatorFactoryService competitionMatchCreatorFactoryService,
        ILogger<StopCompetitionRegistrationHandler> logger)
        : IRequestHandler<StopCompetitionRegistration, CompetitionGetDTO>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ICompetitionMatchCreatorFactoryService _competitionMatchCreatorFactoryService = competitionMatchCreatorFactoryService;
        private readonly ILogger<StopCompetitionRegistrationHandler> _logger = logger;

        public async Task<CompetitionGetDTO> Handle(StopCompetitionRegistration request, CancellationToken cancellationToken)
        {
            Competition competition = await _unitOfWork.CompetitionRepository.GetById(request.CompetitionId)
                ?? throw new APNotFoundException(Tuple.Create(nameof(request.CompetitionId), request.CompetitionId));

            competition.StopRegistrations();

            Competition updated;

            try
            {
                await _unitOfWork.BeginTransactionAsync();
                updated = await _unitOfWork.CompetitionRepository.Update(competition);
                await _competitionMatchCreatorFactoryService
                    .GetCompetitionMatchCreator(competition.GetType())
                    .CreateCompetitionMatches(updated.Id);
                await _unitOfWork.SaveAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }

            _logger.LogInformation("Registrations for competition {CompetitionName} have been stopped!",
                [competition.Name]);

            CompetitionGetDTO response = updated is OneVSAllCompetition ? _mapper.Map<OneVSAllCompetitionGetDTO>(updated)
                : updated is TournamentCompetition ? _mapper.Map<TournamentCompetitionGetDTO>(updated)
                : throw new AmdarisProjectException("Unexpected competition type!");
            return response;
        }
    }
}
