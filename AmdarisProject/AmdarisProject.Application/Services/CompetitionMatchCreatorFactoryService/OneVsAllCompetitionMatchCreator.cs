using AmdarisProject.Application.Abstractions;
using AmdarisProject.Domain.Models;
using AmdarisProject.Domain.Models.CompetitionModels;
using AmdarisProject.Domain.Models.CompetitorModels;

namespace AmdarisProject.Application.Services.CompetitionMatchCreatorServices
{
    internal class OneVsAllCompetitionMatchCreator(IUnitOfWork unitOfWork) : CompetitionMatchCreator(unitOfWork)
    {
        public override async Task<IEnumerable<Match>> CreateMatches(Competition competiton, IEnumerable<Competitor> competitors)
        {
            List<Match> matches = [];

            for (int i = 0; i < competitors.Count(); i++)
            {
                for (int j = i + 1; j < competitors.Count(); j++)
                {
                    Match created = await CreateMatch(competiton.Location, competitors.ElementAt(i).Id,
                        competitors.ElementAt(j).Id, competiton.Id, null, null);
                    matches.Add(created);
                }
            }

            return matches;
        }
    }
}
