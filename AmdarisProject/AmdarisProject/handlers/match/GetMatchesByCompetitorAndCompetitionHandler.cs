using AmdarisProject.models;
using AmdarisProject.repositories.abstractions;
using MediatR;

namespace AmdarisProject.handlers.match
{
    public record GetMatchesByCompetitorAndCompetition(ulong CompetitorId, ulong CompetitionId) : IRequest<IEnumerable<Match>>;
    public class GetMatchesByCompetitorAndCompetitionHandler(IMatchRepository matchRepository)
        : IRequestHandler<GetMatchesByCompetitorAndCompetition, IEnumerable<Match>>
    {
        private readonly IMatchRepository _matchRepository = matchRepository;

        public Task<IEnumerable<Match>> Handle(GetMatchesByCompetitorAndCompetition request, CancellationToken cancellationToken)
        {
            IEnumerable<Match> matches = _matchRepository.GetAllByCompetitorAndCompetition(request.CompetitorId, request.CompetitionId);
            return Task.FromResult(matches);
        }
    }
}
