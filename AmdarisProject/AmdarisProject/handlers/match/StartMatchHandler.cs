using AmdarisProject.models;
using AmdarisProject.repositories.abstractions;
using AmdarisProject.utils.enums;
using AmdarisProject.utils.Exceptions;
using MediatR;

namespace AmdarisProject.handlers.match
{
    public record StartMatch(ulong MatchId) : IRequest<Match>;
    public class StartMatchHandler(IMatchRepository matchRepository)
        : IRequestHandler<StartMatch, Match>
    {
        private readonly IMatchRepository _matchRepository = matchRepository;

        public Task<Match> Handle(StartMatch request, CancellationToken cancellationToken)
        {
            Match match = _matchRepository.GetById(request.MatchId);

            if (match.Competition.Status is not CompetitionStatus.STARTED)
                throw new APIllegalStatusException(nameof(StartMatchHandler), nameof(Handle), match.Competition.Status.ToString());

            bool anotherMatchIsBeingPlayed = match.Competition.Matches.Any(match => match.Status is MatchStatus.STARTED);

            if (anotherMatchIsBeingPlayed)
                throw new AmdarisProjectException(nameof(StartMatchHandler), nameof(Handle),
                    "Cannot start a match while another one is being played!");

            if (match.Status is not MatchStatus.NOT_STARTED)
                throw new APIllegalStatusException(nameof(StartMatchHandler), nameof(Handle), match.Status.ToString());

            DateTime now = DateTime.Now;
            bool lateStart = match.StartTime is not null && now > match.StartTime;
            match.StartTime = now;

            //TODO extract to method in repository
            if (lateStart)
            {
                int i = 0;
                match.Competition.Matches
                    .Where(match => match.Status is MatchStatus.NOT_STARTED)
                    .OrderBy(match => match.StartTime)
                    .ToList()
                    .ForEach(match => match.StartTime = now.AddSeconds(
                        (double)(((ulong)++i) * (match.Competition.GameRules.DurationInSeconds! + match.Competition.GameRules.BreakInSeconds!)))
                    );
            }

            match.Status = MatchStatus.STARTED;

            Match updated = _matchRepository.Update(match);

            Console.WriteLine($"Competition {match.Competition.Name}: Match between {match.CompetitorOne.Name} and {match.CompetitorTwo.Name} has started!");

            return Task.FromResult(updated);
        }
    }
}
