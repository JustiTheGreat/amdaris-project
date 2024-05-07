using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Services.CompetitionMatchCreatorFactoryService.MatchCreators;
using AmdarisProject.Domain.Models.CompetitionModels;

namespace AmdarisProject.Application.Services.CompetitionMatchCreatorServices
{
    public class CompetitionMatchCreatorFactoryService : ICompetitionMatchCreatorFactoryService
    {
        private readonly Dictionary<Type, ICompetitionMatchCreator> competitionMatchCreatorServices = [];

        public CompetitionMatchCreatorFactoryService(IUnitOfWork unitOfWork)
        {
            competitionMatchCreatorServices.Add(typeof(OneVSAllCompetition), new OneVsAllCompetitionMatchCreator(unitOfWork));
            competitionMatchCreatorServices.Add(typeof(TournamentCompetition), new TournamentCompetitionMatchCreator(unitOfWork));
        }

        public ICompetitionMatchCreator GetCompetitionMatchCreator(Type type)
            => competitionMatchCreatorServices[type];
    }
}
