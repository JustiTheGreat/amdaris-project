using AmdarisProject.models.competitor;
using AmdarisProject.repositories.abstractions;
using MediatR;

namespace AmdarisProject.handlers.competitor
{
    public record GetCompetitorById(ulong CompetitorId) : IRequest<Competitor>;
    public class GetCompetitorByIdHandler(ICompetitorRepository competitorRepository)
        : IRequestHandler<GetCompetitorById, Competitor>
    {
        private readonly ICompetitorRepository _competitorRepository = competitorRepository;

        public Task<Competitor> Handle(GetCompetitorById request, CancellationToken cancellationToken)
        {
            Competitor competitor = _competitorRepository.GetById(request.CompetitorId);
            return Task.FromResult(competitor);
        }
    }
}
