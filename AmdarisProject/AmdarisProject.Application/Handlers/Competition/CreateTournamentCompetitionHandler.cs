using AmdarisProject.models.competition;
using AmdarisProject.repositories.abstractions;
using AmdarisProject.utils;
using Domain.Enums;
using MediatR;

namespace AmdarisProject.handlers.competition
{
    public record CreateTournamentCompetition(string Name, string Location, DateTime StartTime, GameRules GameRules)
        : IRequest<TournamentCompetition>;
    public class CreateTournamentCompetitionHandler(ICompetitionRepository competitionRepository)
        : IRequestHandler<CreateTournamentCompetition, TournamentCompetition>
    {
        private readonly ICompetitionRepository _competitionRepository = competitionRepository;

        public Task<TournamentCompetition> Handle(CreateTournamentCompetition request, CancellationToken cancellationToken)
        {
            TournamentCompetition tournamentCompetition = new(request.Name, request.Location, request.StartTime, CompetitionStatus.ORGANIZING,
                request.GameRules.WinAt, request.GameRules.DurationInSeconds, request.GameRules.BreakInSeconds, request.GameRules.Type,
                request.GameRules.CompetitorType, request.GameRules.TeamSize, [], [], []);
            TournamentCompetition created = (TournamentCompetition)_competitionRepository.Create(tournamentCompetition);
            return Task.FromResult(created);
        }
    }
}
