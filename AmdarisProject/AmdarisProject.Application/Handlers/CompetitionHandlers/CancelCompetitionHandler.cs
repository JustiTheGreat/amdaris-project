using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Dtos.ResponseDTOs.CompetitionResponseDTOs;
using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models.CompetitionModels;
using Mapster;
using MediatR;

namespace AmdarisProject.handlers.competition
{
    public record CancelCompetition(ulong CompetitionId) : IRequest<CompetitionResponseDTO>;
    public class CancelCompetitionHandler(IUnitOfWork unitOfWork)
        : IRequestHandler<CancelCompetition, CompetitionResponseDTO>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<CompetitionResponseDTO> Handle(CancelCompetition request, CancellationToken cancellationToken)
        {
            Competition competition = await _unitOfWork.CompetitionRepository.GetById(request.CompetitionId)
                ?? throw new APNotFoundException(nameof(GetCompetitionByIdHandler), nameof(Handle),
                    [Tuple.Create(nameof(request.CompetitionId), request.CompetitionId)]);

            if (competition.Status is not CompetitionStatus.ORGANIZING
                && competition.Status is not CompetitionStatus.NOT_STARTED
                && competition.Status is not CompetitionStatus.STARTED)
                throw new APIllegalStatusException(nameof(CancelCompetitionHandler), nameof(Handle), competition.Status);

            Competition updated;

            try
            {
                await _unitOfWork.BeginTransactionAsync();
                competition.Status = CompetitionStatus.FINISHED;
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
            Console.WriteLine($"Competition {updated.Name} ended!");
            //
            CompetitionResponseDTO response = updated.Adapt<CompetitionResponseDTO>();
            return response;
        }
    }
}
