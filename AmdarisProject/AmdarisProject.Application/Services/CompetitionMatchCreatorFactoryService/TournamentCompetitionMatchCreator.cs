using AmdarisProject.Application.Abstractions;
using AmdarisProject.Domain.Models;
using AmdarisProject.Domain.Models.CompetitionModels;
using AmdarisProject.Domain.Models.CompetitorModels;

namespace AmdarisProject.Application.Services.CompetitionMatchCreatorServices
{
    internal class TournamentCompetitionMatchCreator(IUnitOfWork unitOfWork) : CompetitionMatchCreator(unitOfWork)
    {
        public override async Task<IEnumerable<Match>> CreateMatches(Competition competiton, IEnumerable<Competitor> competitors)
        {
            List<Match> matches = [];

            IEnumerable<Competitor> competitorsByRating = [.. competitors
                    .OrderByDescending(competitor =>
                        _unitOfWork.MatchRepository
                            .GetCompetitorWinRatingForGameType(competitor.Id, competiton.GameType).Result)];

            //TODO tournament competition cancel advancement logic

            int competitorsCount = competitors.Count();
            int stageLevel = 0;

            while (competitorsCount != 1)
            {
                stageLevel++;
                competitorsCount /= 2;
            }

            for (int i = 0; i < competitors.Count(); i += 2)
            {
                Match created = await CreateMatch(competiton.Location, competitors.ElementAt(i).Id,
                    competitors.ElementAt(i + 1).Id, competiton.Id, (ushort?)stageLevel, (ushort?)i);
                matches.Add(created);
            }

            return matches;
        }
    }
}
