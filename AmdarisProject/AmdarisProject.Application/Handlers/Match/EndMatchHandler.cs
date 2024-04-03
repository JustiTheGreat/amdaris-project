using AmdarisProject.models;
using AmdarisProject.repositories.abstractions;
using AmdarisProject.utils;
using Domain.Enums;
using Domain.Exceptions;
using MediatR;

namespace AmdarisProject.handlers.match
{
    public record EndMatch(ulong MatchId) : IRequest<Match>;
    public class EndMatchHandler(IMatchRepository matchRepository, ICompetitorRepository competitorRepository,
        IPointRepository pointRepository)
        : IRequestHandler<EndMatch, Match>
    {
        private readonly IMatchRepository _matchRepository = matchRepository;
        private readonly ICompetitorRepository _competitorRepository = competitorRepository;
        private readonly IPointRepository _pointRepository = pointRepository;

        public Task<Match> Handle(EndMatch request, CancellationToken cancellationToken)
        {
            Match match = _matchRepository.GetById(request.MatchId);

            if (match.Status is not MatchStatus.STARTED)
                throw new APIllegalStatusException(nameof(EndMatchHandler), nameof(Handle), match.Status.ToString());

            if (!Utils.MatchHasACompetitorWithTheWinningScoreUtil(_matchRepository, _competitorRepository, _pointRepository, match.Id))
                throw new APPointsException(nameof(EndMatchHandler), nameof(Handle), "Not enough points to end the match!");

            match.Status = MatchStatus.FINISHED;
            match.EndTime = DateTime.Now;

            Match updated = _matchRepository.Update(match);

            uint competitorOneMatchPoints = Utils.GetCompetitorMatchPointsUtil(_matchRepository, _competitorRepository, _pointRepository,
                updated.Id, updated.CompetitorOne.Id);
            uint competitorTwoMatchPoints = Utils.GetCompetitorMatchPointsUtil(_matchRepository, _competitorRepository, _pointRepository,
               updated.Id, updated.CompetitorTwo.Id);
            Console.WriteLine($"Competition {match.Competition.Name}: Match between " +
                $"{match.CompetitorOne.Name} and {match.CompetitorTwo.Name} has ended with score " +
                $"{competitorOneMatchPoints}-{competitorTwoMatchPoints}!");

            //TODO CreateBonusMatches
            //match.Competition.CreateBonusMatches();

            return Task.FromResult(updated);
        }
    }
}
