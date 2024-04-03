using AmdarisProject.models.competitor;
using AmdarisProject.repositories.abstractions;
using MediatR;

namespace AmdarisProject.handlers.competitor
{
    public record GetPlayers() : IRequest<IEnumerable<Player>>;
    public class GetPlayersHandler(ICompetitorRepository competitorRepository)
        : IRequestHandler<GetPlayers, IEnumerable<Player>>
    {
        private readonly ICompetitorRepository _competitorRepository = competitorRepository;

        public Task<IEnumerable<Player>> Handle(GetPlayers request, CancellationToken cancellationToken)
        {
            IEnumerable<Player> players = _competitorRepository.GetAllPlayers();
            return Task.FromResult(players);
        }
    }
}
