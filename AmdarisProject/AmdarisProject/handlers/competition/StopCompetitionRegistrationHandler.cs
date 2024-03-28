using AmdarisProject.models.competition;
using AmdarisProject.repositories.abstractions;
using AmdarisProject.utils;
using AmdarisProject.utils.enums;
using AmdarisProject.utils.Exceptions;
using MediatR;

namespace AmdarisProject.handlers.competition
{
    public record StopCompetitionRegistration(ulong CompetitionId) : IRequest<Competition>;
    public class StopCompetitionRegistrationHandler(ICompetitionRepository competitionRepository, IMatchRepository matchRepository,
        ICompetitorRepository competitorRepository, IPointRepository pointRepository, IStageRepository stageRepository)
        : IRequestHandler<StopCompetitionRegistration, Competition>
    {
        private readonly ICompetitionRepository _competitionRepository = competitionRepository;
        private readonly IMatchRepository _matchRepository = matchRepository;
        private readonly ICompetitorRepository _competitorRepository = competitorRepository;
        private readonly IPointRepository _pointRepository = pointRepository;
        private readonly IStageRepository _stageRepository = stageRepository;

        public Task<Competition> Handle(StopCompetitionRegistration request, CancellationToken cancellationToken)
        {
            Competition competition = _competitionRepository.GetById(request.CompetitionId);

            if (competition.Status is not CompetitionStatus.ORGANIZING)
                throw new APIllegalStatusException(nameof(StopCompetitionRegistrationHandler), nameof(Handle), competition.Status.ToString());

            Utils.CheckCompetitionCompetitorNumber(competition);
            //TODO CreateBonusMatches
            Utils.CreateCompetitionMatchesUtil(_competitionRepository, _matchRepository, _competitorRepository, _pointRepository,
                _stageRepository, request.CompetitionId);
            competition.Status = CompetitionStatus.NOT_STARTED;
            Competition updated = _competitionRepository.Update(competition);

            Console.WriteLine($"Registrations for competition {updated.Name} have stopped!");

            return Task.FromResult(updated);
        }
    }
}
