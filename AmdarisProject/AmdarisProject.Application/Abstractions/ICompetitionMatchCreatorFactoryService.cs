namespace AmdarisProject.Application.Abstractions
{
    public interface ICompetitionMatchCreatorFactoryService
    {
        ICompetitionMatchCreatorService GetCompetitionMatchCreator(Type type);
    }
}