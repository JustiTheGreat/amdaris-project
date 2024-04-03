using AmdarisProject.models.competitor;
using AmdarisProject.repositories.abstractions;
using MediatR;

namespace AmdarisProject.handlers.competitor
{
    public record GetTeams() : IRequest<IEnumerable<Team>>;
    public class GetTeamsHandler(ICompetitorRepository competitorRepository)
        : IRequestHandler<GetTeams, IEnumerable<Team>>
    {
        private readonly ICompetitorRepository _competitorRepository = competitorRepository;

        public Task<IEnumerable<Team>> Handle(GetTeams request, CancellationToken cancellationToken)
        {
            IEnumerable<Team> teams = _competitorRepository.GetAllTeams();
            return Task.FromResult(teams);
        }
    }
}
