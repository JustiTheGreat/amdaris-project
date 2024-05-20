using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Abstractions.RepositoryAbstractions;
using AmdarisProject.Domain.Models;
using AmdarisProject.Domain.Models.CompetitionModels;
using Microsoft.Extensions.Logging;

namespace AmdarisProject.Application.Services.CompetitionMatchCreatorFactoryService.MatchCreatorService
{
    public class OneVsAllCompetitionMatchCreatorService(IUnitOfWork unitOfWork,
        ILogger<CompetitionMatchCreatorService<OneVSAllCompetition>> logger)
        : CompetitionMatchCreatorService<OneVSAllCompetition>(unitOfWork, logger), IOneVsAllCompetitionMatchCreatorService
    {
        protected override async Task<IEnumerable<Match>> CreateMatches(OneVSAllCompetition oneVSAllCompetition)
        {
            List<Match> matches = [];
            int numberOfCompetitors = oneVSAllCompetition.Competitors.Count;

            for (int i = 0; i < numberOfCompetitors; i++)
            {
                for (int j = i + 1; j < numberOfCompetitors; j++)
                {
                    Match created = await CreateMatch(oneVSAllCompetition.Location,
                        oneVSAllCompetition.Competitors[i], oneVSAllCompetition.Competitors[j],
                        oneVSAllCompetition, null, null);
                    matches.Add(created);
                }
            }

            return matches;
        }
    }
}
