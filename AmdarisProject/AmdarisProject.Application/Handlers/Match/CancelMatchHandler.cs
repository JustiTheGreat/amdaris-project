using AmdarisProject.models;
using AmdarisProject.repositories.abstractions;
using Domain.Enums;
using Domain.Exceptions;
using MediatR;

namespace AmdarisProject.handlers.match
{
    public record CancelMatch(ulong MatchId) : IRequest<Match>;
    public class CancelMatchHandler(IMatchRepository matchRepository)
        : IRequestHandler<CancelMatch, Match>
    {
        private readonly IMatchRepository _matchRepository = matchRepository;

        public Task<Match> Handle(CancelMatch request, CancellationToken cancellationToken)
        {
            Match match = _matchRepository.GetById(request.MatchId);

            if (match.Status is not MatchStatus.NOT_STARTED or MatchStatus.STARTED)
                throw new APIllegalStatusException(nameof(CancelMatchHandler), nameof(Handle), match.Status.ToString());

            match.Status = MatchStatus.CANCELED;

            Match updated = _matchRepository.Update(match);

            //TODO CreateBonusMatches
            //match.Competition.CreateBonusMatches();

            return Task.FromResult(updated);
        }
    }
}
