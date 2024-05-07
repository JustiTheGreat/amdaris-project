using AmdarisProject.Application.Services.CompetitionMatchCreatorFactoryService.MatchCreators;

namespace AmdarisProject.Application.Services.CompetitionMatchCreatorServices
{
    public interface ICompetitionMatchCreatorFactoryService
    {
        ICompetitionMatchCreator GetCompetitionMatchCreator(Type type);
    }
}