using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Dtos.ResponseDTOs.CompetitionResponseDTOs;
using AmdarisProject.Application.ExtensionMethods;
using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models.CompetitionModels;
using AmdarisProject.Domain.Models.CompetitorModels;
using MapsterMapper;
using MediatR;

namespace AmdarisProject.Application.Handlers.CompetitionHandlers
{
    public record AddCompetitorToCompetition(Guid CompetitorId, Guid CompetitionId) : IRequest<CompetitionResponseDTO>;
    public class AddCompetitorToCompetitionHandler(IUnitOfWork unitOfWork, IMapper mapper)
        : IRequestHandler<AddCompetitorToCompetition, CompetitionResponseDTO>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<CompetitionResponseDTO> Handle(AddCompetitorToCompetition request, CancellationToken cancellationToken)
        {
            Competitor competitor = await _unitOfWork.CompetitorRepository.GetById(request.CompetitorId)
                    ?? throw new APNotFoundException(Tuple.Create(nameof(request.CompetitorId), request.CompetitorId));

            Competition competition = await _unitOfWork.CompetitionRepository.GetById(request.CompetitionId)
                ?? throw new APNotFoundException(Tuple.Create(nameof(request.CompetitionId), request.CompetitionId));

            if (competition.Status is not CompetitionStatus.ORGANIZING)
                throw new APIllegalStatusException(competition.Status);

            if (competition.ContainsCompetitor(request.CompetitorId))
                throw new AmdarisProjectException($"Competitor {competitor.Id} is already registered to {competition.Id}!");

            //TODO check if competitor is team and player from team is in another team from competition

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

            //TODO remove
            Console.WriteLine($"Competitor {competitor.Name} has registered to competition {competition.Name}!");
            //

            CompetitionResponseDTO response = competition is OneVSAllCompetition ? _mapper.Map<OneVSAllCompetitionResponseDTO>(competition)
                : competition is TournamentCompetition ? _mapper.Map<TournamentCompetitionResponseDTO>(competition)
                : throw new AmdarisProjectException(nameof(competition));

            return response;
        }
    }
}
