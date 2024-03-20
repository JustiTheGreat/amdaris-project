using AmdarisProject.models.competitor;
using AmdarisProject.utils;
using AmdarisProject.utils.enums;
using AmdarisProject.utils.exceptions;
using AmdarisProject.utils.Exceptions;

namespace AmdarisProject.models.competition
{
    public class TournamentCompetition(string name, string location, DateTime startTime, GameRules game)
        : Competition(name, location, startTime, game)
    {
        public IEnumerable<Stage> Stages { get; set; } = [];

        protected override void CheckCompetitorNumber()
        {
            int competitorNumber = Competitors.Count();

            if (competitorNumber < 2)
                throw new CompetitorNumberException(MessageFormatter.Format(nameof(TournamentCompetition), nameof(CheckCompetitorNumber),
                    Competitors.Count().ToString()));

            while (competitorNumber != 1)
            {
                if (competitorNumber % 2 == 1)
                    throw new CompetitorNumberException(MessageFormatter.Format(nameof(TournamentCompetition), nameof(CheckCompetitorNumber),
                        Competitors.Count().ToString()));
                competitorNumber /= 2;
            }
        }

        protected override void CreateMatches(IEnumerable<Competitor> competitors)
        {
            IEnumerable<KeyValuePair<Competitor, double>> competitorsByRating = competitors
                .ToDictionary(competitor => competitor, competitor => competitor.GetRating(GameRules.Type))
                .OrderByDescending(entry => entry.Value).ToList();

            List<Match> stageMatches = [];
            for (int i = 0; i < competitors.Count(); i += 2)
            {
                Match match = CreateMatch(competitors.ElementAt(i), competitors.ElementAt(i + 1));
                stageMatches.Add(match);
                Matches.Add(match);
            }

            Stage stage = new(stageMatches);
            Stages = Stages.Append(stage);
        }

        public override Competitor GetWinner()
        {
            if (Status is not CompetitionStatus.FINISHED)
                throw new IllegalStatusException(MessageFormatter.Format(nameof(TournamentCompetition), nameof(GetWinner), Status.ToString()));

            //TODO find better criteria
            return GetRanking().ElementAt(0).Key;
        }
    }
}
