using AmdarisProject.models.competitor;
using AmdarisProject.utils;
using AmdarisProject.utils.enums;
using AmdarisProject.utils.exceptions;
using AmdarisProject.utils.Exceptions;

namespace AmdarisProject.models.competition
{
    public class OneVSAllCompetition(string name, string location, DateTime startTime, Game game)
        : Competition(name, location, startTime, game)
    {
        protected override void CheckCompetitorNumber()
        {
            if (Competitors.Count() < 2)
                throw new CompetitorNumberException(MessageFormatter.Format(nameof(OneVSAllCompetition), nameof(CheckCompetitorNumber),
                    Competitors.Count().ToString()));
        }

        protected override void CreateMatches(IEnumerable<Competitor> competitors)
        {
            for (int i = 0; i < competitors.Count(); i++)
                for (int j = i + 1; j < competitors.Count(); j++)
                    Matches = Matches.Append(new Match(Location, DateTime.Now, Game, competitors.ElementAt(i), competitors.ElementAt(j), this));
        }

        public override Competitor GetWinner()
        {
            if (Status is not CompetitionStatus.FINISHED)
                throw new IllegalStatusException(MessageFormatter.Format(nameof(OneVSAllCompetition), nameof(GetWinner), Status.ToString()));

            //TODO find better criteria
            return GetRanking().ElementAt(0).Key;
        }
    }
}
