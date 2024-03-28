using AmdarisProject.models;
using AmdarisProject.repositories.abstractions;
using MediatR;

namespace AmdarisProject.handlers.match
{
    public record UpdateMatch(Match Match) : IRequest<Match>;
    public class UpdateMatchHandler(IMatchRepository matchRepository)
        : IRequestHandler<UpdateMatch, Match>
    {
        private readonly IMatchRepository _matchRepository = matchRepository;

        public Task<Match> Handle(UpdateMatch request, CancellationToken cancellationToken)
        {
            Match updated = _matchRepository.Update(request.Match);
            return Task.FromResult(updated);
        }
    }
}
