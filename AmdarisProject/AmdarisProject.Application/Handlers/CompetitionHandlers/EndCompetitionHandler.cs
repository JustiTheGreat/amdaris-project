using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Dtos.ResponseDTOs.CompetitionResponseDTOs;
using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models.CompetitionModels;
using Mapster;
using MediatR;

namespace AmdarisProject.handlers.competition
{
    public record EndCompetition(ulong CompetitionId) : IRequest<CompetitionResponseDTO>;
    public class EndCompetitionHandler(IUnitOfWork unitOfWork)
        : IRequestHandler<EndCompetition, CompetitionResponseDTO>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<CompetitionResponseDTO> Handle(EndCompetition request, CancellationToken cancellationToken)
        {
            Competition competition = await _unitOfWork.CompetitionRepository.GetById(request.CompetitionId)
                ?? throw new APNotFoundException(nameof(GetCompetitionByIdHandler), nameof(Handle),
                    [Tuple.Create(nameof(request.CompetitionId), request.CompetitionId)]);

            if (competition.Status is not CompetitionStatus.STARTED)
                throw new APIllegalStatusException(nameof(EndCompetitionHandler), nameof(Handle), competition.Status);

            bool allMatchesEnded = !(await _unitOfWork.MatchRepository.GetUnfinishedByCompetition(competition.Id)).Any();

            if (!allMatchesEnded)
                throw new AmdarisProjectException(nameof(EndCompetitionHandler), nameof(Handle),
                    $"Competition {competition.Id} still has unfinished matches!");

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
            Console.WriteLine($"Competition {competition.Name} ended!");
            //
            CompetitionResponseDTO response = updated.Adapt<CompetitionResponseDTO>();
            return response;
        }
    }
}
