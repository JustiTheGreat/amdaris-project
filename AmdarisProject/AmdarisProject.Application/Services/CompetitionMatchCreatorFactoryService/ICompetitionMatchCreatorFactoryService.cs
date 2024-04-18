
namespace AmdarisProject.Application.Services.CompetitionMatchCreatorServices
{
    public interface ICompetitionMatchCreatorFactoryService
    {
        CompetitionMatchCreator GetCompetitionMatchCreatorService(Type type);
    }
}