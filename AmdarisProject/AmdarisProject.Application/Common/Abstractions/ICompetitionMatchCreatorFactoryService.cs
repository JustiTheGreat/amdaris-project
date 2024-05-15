namespace AmdarisProject.Application.Common.Abstractions
{
    public interface ICompetitionMatchCreatorFactoryService
    {
        ICompetitionMatchCreatorService GetCompetitionMatchCreator(Type type);
    }
}