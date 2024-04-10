using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Dtos.ResponseDTOs.CompetitionResponseDTOs;
using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models.CompetitionModels;
using Mapster;
using MediatR;

namespace AmdarisProject.handlers.competition
{
    public record StartCompetition(ulong CompetitionId) : IRequest<CompetitionResponseDTO>;
    public class StartCompetitionHandler(IUnitOfWork unitOfWork)
        : IRequestHandler<StartCompetition, CompetitionResponseDTO>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<CompetitionResponseDTO> Handle(StartCompetition request, CancellationToken cancellationToken)
        {
            Competition competition = await _unitOfWork.CompetitionRepository.GetById(request.CompetitionId)
                ?? throw new APNotFoundException(Tuple.Create(nameof(request.CompetitionId), request.CompetitionId));

            if (competition.Status is not CompetitionStatus.NOT_STARTED)
                throw new APIllegalStatusException(competition.Status);

            Competition updated;

            try
            {
                await _unitOfWork.BeginTransactionAsync();
                competition.Status = CompetitionStatus.NOT_STARTED;
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
            Console.WriteLine($"Competition {updated.Name} started!");
            //
            CompetitionResponseDTO response = updated.Adapt<CompetitionResponseDTO>();
            return response;
        }
    }
}
