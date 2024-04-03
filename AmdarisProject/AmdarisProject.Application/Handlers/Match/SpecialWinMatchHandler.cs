using AmdarisProject.models;
using AmdarisProject.repositories.abstractions;
using Domain.Enums;
using Domain.Exceptions;
using MediatR;

namespace AmdarisProject.handlers.match
{
    public record SpecialWinMatch(ulong MatchId, MatchCompetitor MatchCompetitor) : IRequest<Match>;
    public class SpecialWinMatchHandler(IMatchRepository matchRepository)
        : IRequestHandler<SpecialWinMatch, Match>
    {
        private readonly IMatchRepository _matchRepository = matchRepository;

        public Task<Match> Handle(SpecialWinMatch request, CancellationToken cancellationToken)
        {
            Match match = _matchRepository.GetById(request.MatchId);

            if (match.Status is not MatchStatus.STARTED)
                throw new APIllegalStatusException(nameof(Match), nameof(Handle), match.Status.ToString());

            match.Status = request.MatchCompetitor is MatchCompetitor.COMPETITOR_ONE ? MatchStatus.SPECIAL_WIN_COMPETITOR_ONE
                : MatchStatus.SPECIAL_WIN_COMPETITOR_TWO;
            match.EndTime = DateTime.Now;
            Match updated = _matchRepository.Update(match);

            //TODO CreateBonusMatches
            //updated.Competition.CreateBonusMatches();

            return Task.FromResult(updated);
        }
    }
}
