using AmdarisProject.models;
using AmdarisProject.repositories.abstractions;
using AmdarisProject.utils.enums;
using MediatR;

namespace AmdarisProject.handlers.match
{
    public record GetMatchesByCompetitorAndGameType(ulong CompetitorId, GameType GameType) : IRequest<IEnumerable<Match>>;
    public class GetMatchesByCompetitorAndGameTypeHandler(IMatchRepository matchRepository)
        : IRequestHandler<GetMatchesByCompetitorAndGameType, IEnumerable<Match>>
    {
        private readonly IMatchRepository _matchRepository = matchRepository;

        public Task<IEnumerable<Match>> Handle(GetMatchesByCompetitorAndGameType request, CancellationToken cancellationToken)
        {
            IEnumerable<Match> matches = _matchRepository.GetAllByCompetitorAndGameType(request.CompetitorId, request.GameType);
            return Task.FromResult(matches);
        }
    }
}
