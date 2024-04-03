using AmdarisProject.models.competitor;
using AmdarisProject.repositories.abstractions;
using MediatR;

namespace AmdarisProject.handlers.competitor
{
    public record CreatePlayer(string Name) : IRequest<Player>;
    public class CreatePlayerHandler(ICompetitorRepository competitorRepository)
        : IRequestHandler<CreatePlayer, Player>
    {
        private readonly ICompetitorRepository _competitorRepository = competitorRepository;

        public Task<Player> Handle(CreatePlayer request, CancellationToken cancellationToken)
        {
            Player player = new(request.Name, [], [], [], []);
            Player created = (Player)_competitorRepository.Create(player);
            return Task.FromResult(created);
        }
    }
}
