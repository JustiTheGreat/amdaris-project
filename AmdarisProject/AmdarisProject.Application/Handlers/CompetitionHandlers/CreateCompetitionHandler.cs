using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Dtos.CreateDTOs.CompetitionCreateDTOs;
using AmdarisProject.Application.Dtos.ResponseDTOs.CompetitionResponseDTOs;
using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models.CompetitionModels;
using MapsterMapper;
using MediatR;

namespace AmdarisProject.Application.Handlers.CompetitionHandlers
{
    public record CreateCompetition(CompetitionCreateDTO CompetitionCreateDTO)
        : IRequest<CompetitionResponseDTO>;
    public class CreateCompetitionHandler(IUnitOfWork unitOfWork, IMapper mapper)
        : IRequestHandler<CreateCompetition, CompetitionResponseDTO>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<CompetitionResponseDTO> Handle(CreateCompetition request, CancellationToken cancellationToken)
        {
            if (request.CompetitionCreateDTO.Status is not CompetitionStatus.ORGANIZING)
                throw new APIllegalStatusException(request.CompetitionCreateDTO.Status);

            bool winningConditionsAreValid = (request.CompetitionCreateDTO.WinAt is null
                    || request.CompetitionCreateDTO.DurationInSeconds is not null || request.CompetitionCreateDTO.BreakInSeconds is not null)
                && (request.CompetitionCreateDTO.DurationInSeconds is null || request.CompetitionCreateDTO.BreakInSeconds is null);

            bool competitorRequirementsAreValid =
                request.CompetitionCreateDTO.CompetitorType is CompetitorType.PLAYER && request.CompetitionCreateDTO.TeamSize is not null
                || request.CompetitionCreateDTO.CompetitorType is CompetitorType.TEAM
                    && (request.CompetitionCreateDTO.TeamSize is null || request.CompetitionCreateDTO.TeamSize < 2);

            if (winningConditionsAreValid || competitorRequirementsAreValid)
                throw new APArgumentException(nameof(request.CompetitionCreateDTO));

            Competition mapped =
                request.CompetitionCreateDTO is OneVSAllCompetitionCreateDTO ? _mapper.Map<OneVSAllCompetition>(request.CompetitionCreateDTO)
                : request.CompetitionCreateDTO is TournamentCompetitionCreateDTO ? _mapper.Map<TournamentCompetition>(request.CompetitionCreateDTO)
                : throw new AmdarisProjectException(nameof(request.CompetitionCreateDTO));

            Competition competition;

            try
            {
                await _unitOfWork.BeginTransactionAsync();
                competition = await _unitOfWork.CompetitionRepository.Create(mapped);
                await _unitOfWork.SaveAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }

            CompetitionResponseDTO response =
                competition is OneVSAllCompetition ? _mapper.Map<OneVSAllCompetitionResponseDTO>(competition)
                : competition is TournamentCompetition ? _mapper.Map<TournamentCompetitionResponseDTO>(competition)
                : throw new AmdarisProjectException(nameof(competition));

            return response;
        }
    }
}
