using AmdarisProject.models;
using AmdarisProject.repositories.abstractions;
using MediatR;

namespace AmdarisProject.handlers.match
{
    public record GetMatchById(ulong MatchId) : IRequest<Match>;
    public class GetMatchByIdHandler(IMatchRepository matchRepository)
        : IRequestHandler<GetMatchById, Match>
    {
        private readonly IMatchRepository _matchRepository = matchRepository;

        public Task<Match> Handle(GetMatchById request, CancellationToken cancellationToken)
        {
            Match match = _matchRepository.GetById(request.MatchId);
            return Task.FromResult(match);
        }
    }
}
