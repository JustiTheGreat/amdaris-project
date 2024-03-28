using AmdarisProject.models.competition;
using AmdarisProject.repositories.abstractions;
using AmdarisProject.utils.enums;
using AmdarisProject.utils.Exceptions;
using MediatR;

namespace AmdarisProject.handlers.competition
{
    public record StartCompetition(ulong CompetitionId) : IRequest<Competition>;
    public class StartCompetitionHandler(ICompetitionRepository competitionRepository)
        : IRequestHandler<StartCompetition, Competition>
    {
        private readonly ICompetitionRepository _competitionRepository = competitionRepository;

        public Task<Competition> Handle(StartCompetition request, CancellationToken cancellationToken)
        {
            Competition competition = _competitionRepository.GetById(request.CompetitionId);

            if (competition.Status is not CompetitionStatus.NOT_STARTED)
                throw new APIllegalStatusException(nameof(StartCompetitionHandler), nameof(Handle), competition.Status.ToString());

            competition.Status = CompetitionStatus.STARTED;
            Competition updated = _competitionRepository.Update(competition);

            Console.WriteLine($"Competition {updated.Name} started!");

            return Task.FromResult(updated);
        }
    }
}
