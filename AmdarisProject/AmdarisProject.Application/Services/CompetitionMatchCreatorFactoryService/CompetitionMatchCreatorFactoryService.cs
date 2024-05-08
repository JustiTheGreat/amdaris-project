using AmdarisProject.Application.Services.CompetitionMatchCreatorFactoryService.MatchCreatorService;
using AmdarisProject.Domain.Models.CompetitionModels;

namespace AmdarisProject.Application.Services.CompetitionMatchCreatorServices
{
    public class CompetitionMatchCreatorFactoryService : ICompetitionMatchCreatorFactoryService
    {
        private readonly Dictionary<Type, ICompetitionMatchCreatorService> competitionMatchCreatorServices = [];

        public CompetitionMatchCreatorFactoryService(IOneVsAllCompetitionMatchCreatorService oneVsAllCompetitionMatchCreatorService,
            ITournamentCompetitionMatchCreatorService tournamentCompetitionMatchCreatorService)
        {
            competitionMatchCreatorServices.Add(typeof(OneVSAllCompetition), oneVsAllCompetitionMatchCreatorService);
            competitionMatchCreatorServices.Add(typeof(TournamentCompetition), tournamentCompetitionMatchCreatorService);
        }

        public ICompetitionMatchCreatorService GetCompetitionMatchCreator(Type type)
            => competitionMatchCreatorServices[type];
    }
}
