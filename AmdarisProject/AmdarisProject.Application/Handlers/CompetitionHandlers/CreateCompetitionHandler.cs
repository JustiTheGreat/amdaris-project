using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Dtos.CreateDTOs.CompetitionCreateDTOs;
using AmdarisProject.Application.Dtos.ResponseDTOs.CompetitionResponseDTOs;
using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models.CompetitionModels;
using Mapster;
using MediatR;

namespace AmdarisProject.Application.Handlers.CompetitionHandlers
{
    public record CreateCompetition(CompetitionCreateDTO CompetitionCreateDTO)
        : IRequest<CompetitionResponseDTO>;
    public class CreateCompetitionHandler(IUnitOfWork unitOfWork)
        : IRequestHandler<CreateCompetition, CompetitionResponseDTO>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<CompetitionResponseDTO> Handle(CreateCompetition request, CancellationToken cancellationToken)
        {
            if (request.CompetitionCreateDTO.Status is not CompetitionStatus.ORGANIZING)
                throw new APIllegalStatusException(nameof(CreateCompetitionHandler), nameof(Handle), request.CompetitionCreateDTO.Status);

            if (request.CompetitionCreateDTO.WinAt is null
                && request.CompetitionCreateDTO.DurationInSeconds is null && request.CompetitionCreateDTO.BreakInSeconds is null
                || request.CompetitionCreateDTO.DurationInSeconds is null && request.CompetitionCreateDTO.BreakInSeconds is not null
                || request.CompetitionCreateDTO.DurationInSeconds is not null && request.CompetitionCreateDTO.BreakInSeconds is null
                || request.CompetitionCreateDTO.CompetitorType is CompetitorType.PLAYER && request.CompetitionCreateDTO.TeamSize is not null
                || request.CompetitionCreateDTO.CompetitorType is CompetitorType.TEAM && (request.CompetitionCreateDTO.TeamSize is null || request.CompetitionCreateDTO.TeamSize < 2))
                throw new APArgumentException(nameof(CreateCompetitionHandler), nameof(Handle), nameof(request.CompetitionCreateDTO));

            Competition mapped =
                request.CompetitionCreateDTO is OneVSAllCompetitionCreateDTO ? request.CompetitionCreateDTO.Adapt<OneVSAllCompetition>()
                : request.CompetitionCreateDTO is TournamentCompetitionCreateDTO ? request.CompetitionCreateDTO.Adapt<TournamentCompetition>()
                : throw new AmdarisProjectException(nameof(CreateCompetitionHandler), nameof(Handle), nameof(request.CompetitionCreateDTO));

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
                competition is OneVSAllCompetition ? competition.Adapt<OneVSAllCompetitionResponseDTO>()
                : competition is TournamentCompetition ? competition.Adapt<TournamentCompetitionResponseDTO>()
                : throw new AmdarisProjectException(nameof(CreateCompetitionHandler), nameof(Handle), nameof(competition));

            return response;
        }
    }
}
