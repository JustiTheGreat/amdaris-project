using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Dtos.ResponseDTOs.CompetitionResponseDTOs;
using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models.CompetitionModels;
using MapsterMapper;
using MediatR;

namespace AmdarisProject.handlers.competition
{
    public record EndCompetition(Guid CompetitionId) : IRequest<CompetitionResponseDTO>;
    public class EndCompetitionHandler(IUnitOfWork unitOfWork, IMapper mapper)
        : IRequestHandler<EndCompetition, CompetitionResponseDTO>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<CompetitionResponseDTO> Handle(EndCompetition request, CancellationToken cancellationToken)
        {
            Competition competition = await _unitOfWork.CompetitionRepository.GetById(request.CompetitionId)
                ?? throw new APNotFoundException(Tuple.Create(nameof(request.CompetitionId), request.CompetitionId));

            if (competition.Status is not CompetitionStatus.STARTED)
                throw new APIllegalStatusException(competition.Status);

            if (!await _unitOfWork.MatchRepository.AllMatchesOfCompetitonAreFinished(competition.Id))
                throw new AmdarisProjectException($"Competition {competition.Id} still has unfinished matches!");

            Competition updated;

            try
            {
                await _unitOfWork.BeginTransactionAsync();
                competition.Status = CompetitionStatus.FINISHED;
                //TODO move set winner here
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

            CompetitionResponseDTO response = updated is OneVSAllCompetition ? _mapper.Map<OneVSAllCompetitionResponseDTO>(updated)
                : updated is TournamentCompetition ? _mapper.Map<TournamentCompetitionResponseDTO>(updated)
                : throw new AmdarisProjectException(nameof(updated));
            return response;
        }
    }
}
