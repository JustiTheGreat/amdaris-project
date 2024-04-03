using AmdarisProject.repositories.abstractions;
using AmdarisProject.utils;
using Domain.Enums;
using MediatR;

namespace AmdarisProject.handlers.competitor
{
    public record GetCompetitorWins(ulong CompetitorId, GameType GameType) : IRequest<uint>;
    public class GetCompetitorWinsHandler(IMatchRepository matchRepository,
            ICompetitorRepository competitorRepository, IPointRepository pointRepository)
        : IRequestHandler<GetCompetitorWins, uint>
    {
        private readonly IMatchRepository _matchRepository = matchRepository;
        private readonly ICompetitorRepository _competitorRepository = competitorRepository;
        private readonly IPointRepository _pointRepository = pointRepository;

        public Task<uint> Handle(GetCompetitorWins request, CancellationToken cancellationToken)
        {
            uint wins = (uint)_matchRepository.GetAllByCompetitorAndGameType(request.CompetitorId, request.GameType)
                .Count(match => Utils.CompetitorIsPartOfTheWinningSideUtil(_matchRepository, _competitorRepository,
                    _pointRepository, match.Id, request.CompetitorId));
            return Task.FromResult(wins);
        }
    }
}
