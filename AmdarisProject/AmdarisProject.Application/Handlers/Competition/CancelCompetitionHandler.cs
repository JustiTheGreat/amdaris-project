using AmdarisProject.models.competition;
using AmdarisProject.repositories.abstractions;
using Domain.Enums;
using Domain.Exceptions;
using MediatR;

namespace AmdarisProject.handlers.competition
{
    public record CancelCompetition(ulong CompetitionId) : IRequest<Competition>;
    public class CancelCompetitionHandler(ICompetitionRepository competitionRepository)
        : IRequestHandler<CancelCompetition, Competition>
    {
        private readonly ICompetitionRepository _competitionRepository = competitionRepository;

        public Task<Competition> Handle(CancelCompetition request, CancellationToken cancellationToken)
        {
            Competition competition = _competitionRepository.GetById(request.CompetitionId);

            if (competition.Status is not CompetitionStatus.ORGANIZING
                && competition.Status is not CompetitionStatus.NOT_STARTED
                && competition.Status is not CompetitionStatus.STARTED)
                throw new APIllegalStatusException(nameof(CancelCompetitionHandler), nameof(Handle), competition.Status.ToString());

            competition.Status = CompetitionStatus.CANCELED;
            Competition updated = _competitionRepository.Update(competition);

            Console.WriteLine($"Competition {updated.Name} was canceled!");

            return Task.FromResult(updated);
        }
    }
}
