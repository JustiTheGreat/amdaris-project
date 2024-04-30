using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Dtos.ResponseDTOs.CompetitionResponseDTOs;
using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models.CompetitionModels;
using MapsterMapper;
using MediatR;

namespace AmdarisProject.handlers.competition
{
    public record CancelCompetition(Guid CompetitionId) : IRequest<CompetitionGetDTO>;
    public class CancelCompetitionHandler(IUnitOfWork unitOfWork, IMapper mapper)
        : IRequestHandler<CancelCompetition, CompetitionGetDTO>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<CompetitionGetDTO> Handle(CancelCompetition request, CancellationToken cancellationToken)
        {
            Competition competition = await _unitOfWork.CompetitionRepository.GetById(request.CompetitionId)
                ?? throw new APNotFoundException(Tuple.Create(nameof(request.CompetitionId), request.CompetitionId));

            if (competition.Status is not CompetitionStatus.ORGANIZING
                && competition.Status is not CompetitionStatus.NOT_STARTED
                && competition.Status is not CompetitionStatus.STARTED)
                throw new APIllegalStatusException(competition.Status);

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

            CompetitionGetDTO response = _mapper.Map<CompetitionGetDTO>(updated);
            return response;
        }
    }
}
