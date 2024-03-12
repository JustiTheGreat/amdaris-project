using AmdarisProject.models.competitor;
using AmdarisProject.utils;
using AmdarisProject.utils.exceptions;
using AmdarisProject.utils.Exceptions;

namespace AmdarisProject.models.competition
{
    public class OneVSAllCompetition(string name, string location, DateTime startTime, Game game) : Competition(name, location, startTime, game)
    {
        protected override void CheckCompetitorNumber()
        {
            if (Competitors.Count() < 2)
                throw new CompetitorNumberException(MessageFormatter.Format(nameof(OneVSAllCompetition), nameof(CheckCompetitorNumber), Competitors.Count().ToString()));
        }

        protected override void CreateMatches()
        {
            foreach (Competitor competitor1 in Competitors)
            {
                foreach (Competitor competitor2 in Competitors)
                {
                    if (!competitor1.Equals(competitor2))
                    {
                        Matches = Matches.Append(new Match(Location, DateTime.Now, Game, competitor1, competitor2));
                    }
                }
            }
        }

        protected override Competitor GetWinner()
        {
            IEnumerable<Competitor?> winners = Matches.Select(match =>
            {
                try
                {
                    return match.GetWinner();
                }
                catch (AmdarisProjectException)
                {
                    return null;
                }
            }).ToList();
            Competitor winner = Competitors.ToDictionary(
                competitor => competitor,
                competitor => winners.Where(winner => winner is not null && winner.Equals(competitor)).Count())
                .OrderByDescending(entry => entry.Value).First().Key;
            return winner;
        }
    }
}
