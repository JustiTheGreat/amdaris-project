using AmdarisProject.models.competitor;
using AmdarisProject.repositories.abstractions;
using MediatR;

namespace AmdarisProject.handlers.competitor
{
    public record CreateTeam(string Name, ushort TeamSize) : IRequest<Team>;
    public class CreateTeamHandler(ICompetitorRepository competitorRepository)
        : IRequestHandler<CreateTeam, Team>
    {
        private readonly ICompetitorRepository _competitorRepository = competitorRepository;

        public Task<Team> Handle(CreateTeam request, CancellationToken cancellationToken)
        {
            Team team = new(request.Name, [], [], request.TeamSize, []);
            Team created = (Team)_competitorRepository.Create(team);
            return Task.FromResult(created);
        }
    }
}

