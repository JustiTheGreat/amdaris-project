using AmdarisProject.models.competition;
using AmdarisProject.repositories.abstractions;
using AmdarisProject.utils;
using Domain.Enums;
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
            OneVSAllCompetition oneVSAllCompetition = new(request.Name, request.Location, request.StartTime, CompetitionStatus.ORGANIZING,
                request.GameRules.WinAt, request.GameRules.DurationInSeconds, request.GameRules.BreakInSeconds, request.GameRules.Type,
                request.GameRules.CompetitorType, request.GameRules.TeamSize, [], []);
            OneVSAllCompetition created = (OneVSAllCompetition)_competitionRepository.Create(oneVSAllCompetition);
            return Task.FromResult(created);
        }
    }
}
