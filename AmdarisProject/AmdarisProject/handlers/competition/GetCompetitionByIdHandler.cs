using AmdarisProject.models.competition;
using AmdarisProject.repositories.abstractions;
using MediatR;

namespace AmdarisProject.handlers.competition
{
    public record GetCompetitionById(ulong CompetitionId) : IRequest<Competition>;
    public class GetCompetitionByIdHandler(ICompetitionRepository competitionRepository)
        : IRequestHandler<GetCompetitionById, Competition>
    {
        private readonly ICompetitionRepository _competitionRepository = competitionRepository;

        public Task<Competition> Handle(GetCompetitionById request, CancellationToken cancellationToken)
        {
            Competition competition = _competitionRepository.GetById(request.CompetitionId);
            return Task.FromResult(competition);
        }
    }
}
