using AmdarisProject.models.competition;
using AmdarisProject.models.competitor;
using AmdarisProject.repositories.abstractions;
using AmdarisProject.utils;
using AmdarisProject.utils.enums;
using AmdarisProject.utils.exceptions;
using AmdarisProject.utils.Exceptions;
using MediatR;

namespace AmdarisProject.handlers.competitor
{
    public record AddCompetitorToCompetition(ulong CompetitorId, ulong CompetitionId) : IRequest<Competition>;
    public class AddCompetitorToCompetitionHandler(ICompetitorRepository competitorRepository, ICompetitionRepository competitionRepository)
        : IRequestHandler<AddCompetitorToCompetition, Competition>
    {
        private readonly ICompetitorRepository _competitorRepository = competitorRepository;
        private readonly ICompetitionRepository _competitionRepository = competitionRepository;

        public Task<Competition> Handle(AddCompetitorToCompetition request, CancellationToken cancellationToken)
        {
            Competition competition = _competitionRepository.GetById(request.CompetitionId);

            if (competition.Status is not CompetitionStatus.ORGANIZING)
                throw new APIllegalStatusException(nameof(AddCompetitorToCompetitionHandler), nameof(Handle), competition.Status.ToString());

            if (Utils.CompetitionContainsCompetitor(competition, request.CompetitorId))
                throw new APCompetitorException(nameof(AddCompetitorToCompetitionHandler), nameof(Handle),
                    $"Competitor {request.CompetitorId} is already registered to {competition.Id}!");

            //TODO check if competitor is team and player from team is in another team from competition

            Competitor competitor = _competitorRepository.GetById(request.CompetitorId);
            competition.Competitors.Add(competitor);
            Competition updated = _competitionRepository.Update(competition);

            Console.WriteLine($"Competitor {competitor.Name} has registered to competition {competition.Name}!");

            return Task.FromResult(updated);
        }
    }
}
