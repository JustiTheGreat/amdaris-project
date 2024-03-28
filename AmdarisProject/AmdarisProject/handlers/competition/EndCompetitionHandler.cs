using AmdarisProject.models.competition;
using AmdarisProject.repositories.abstractions;
using AmdarisProject.utils.enums;
using AmdarisProject.utils.Exceptions;
using MediatR;

namespace AmdarisProject.handlers.competition
{
    public record EndCompetition(ulong CompetitionId) : IRequest<Competition>;
    public class EndCompetitionHandler(ICompetitionRepository competitionRepository, IMatchRepository matchRepository)
        : IRequestHandler<EndCompetition, Competition>
    {
        private readonly ICompetitionRepository _competitionRepository = competitionRepository;
        private readonly IMatchRepository _matchRepository = matchRepository;

        public Task<Competition> Handle(EndCompetition request, CancellationToken cancellationToken)
        {
            Competition competition = _competitionRepository.GetById(request.CompetitionId);

            if (competition.Status is not CompetitionStatus.STARTED)
                throw new APIllegalStatusException(nameof(EndCompetitionHandler), nameof(Handle), competition.Status.ToString());

            bool allMatchesEnded = !_matchRepository.GetUnfinishedByCompetition(competition.Id).Any();

            if (!allMatchesEnded)
                throw new AmdarisProjectException(nameof(EndCompetitionHandler), nameof(Handle),
                    $"Competition {competition.Id} still has unfinished matches!");

            competition.Status = CompetitionStatus.FINISHED;
            Competition updated = _competitionRepository.Update(competition);

            Console.WriteLine($"Competition {updated.Name} ended!");

            return Task.FromResult(updated);
        }
    }
}
