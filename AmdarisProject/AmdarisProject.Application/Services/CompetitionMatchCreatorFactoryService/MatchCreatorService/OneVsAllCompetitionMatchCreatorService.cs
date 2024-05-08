using AmdarisProject.Application.Abstractions;
using AmdarisProject.Domain.Models;
using AmdarisProject.Domain.Models.CompetitionModels;
using Microsoft.Extensions.Logging;

namespace AmdarisProject.Application.Services.CompetitionMatchCreatorFactoryService.MatchCreatorService
{
    public class OneVsAllCompetitionMatchCreatorService(IUnitOfWork unitOfWork,
        ILogger<CompetitionMatchCreatorService<OneVSAllCompetition>> logger)
        : CompetitionMatchCreatorService<OneVSAllCompetition>(unitOfWork, logger), IOneVsAllCompetitionMatchCreatorService
    {
        protected override bool ShouldCreateMatches(OneVSAllCompetition competition)
            => competition.Matches.Count == 0;

        protected override async Task<IEnumerable<Match>> CreateMatches(OneVSAllCompetition competiton)
        {
            List<Match> matches = [];
            int numberOfCompetitors = competiton.Competitors.Count;

            for (int i = 0; i < numberOfCompetitors; i++)
            {
                for (int j = i + 1; j < numberOfCompetitors; j++)
                {
                    Match created = await CreateMatch(competiton.Location, competiton.Competitors.ElementAt(i).Id,
                        competiton.Competitors.ElementAt(j).Id, competiton.Id, null, null);
                    matches.Add(created);
                }
            }

            return matches;
        }
    }
}
