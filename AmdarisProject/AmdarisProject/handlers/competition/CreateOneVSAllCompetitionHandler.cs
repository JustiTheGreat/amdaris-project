using AmdarisProject.models.competition;
using AmdarisProject.repositories.abstractions;
using AmdarisProject.utils;
using AmdarisProject.utils.enums;
using MediatR;

namespace AmdarisProject.handlers.competition
{
    public record CreateOneVSAllCompetition(string Name, string Location, DateTime StartTime, GameRules GameRules)
        : IRequest<OneVSAllCompetition>;
    public class CreateOneVSAllCompetitionHandler(ICompetitionRepository competitionRepository)
        : IRequestHandler<CreateOneVSAllCompetition, OneVSAllCompetition>
    {
        private readonly ICompetitionRepository _competitionRepository = competitionRepository;

        public Task<OneVSAllCompetition> Handle(CreateOneVSAllCompetition request, CancellationToken cancellationToken)
        {
            OneVSAllCompetition tournamentCompetition = new(request.Name, request.Location, request.StartTime, request.GameRules,
                CompetitionStatus.ORGANIZING, [], []);
            OneVSAllCompetition created = (OneVSAllCompetition)_competitionRepository.Create(tournamentCompetition);
            return Task.FromResult(created);
        }
    }
}
