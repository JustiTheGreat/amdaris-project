using AmdarisProject.Application.Services.CompetitionMatchCreatorFactoryService.MatchCreatorService;

namespace AmdarisProject.Application.Services.CompetitionMatchCreatorServices
{
    public interface ICompetitionMatchCreatorFactoryService
    {
        ICompetitionMatchCreatorService GetCompetitionMatchCreator(Type type);
    }
}