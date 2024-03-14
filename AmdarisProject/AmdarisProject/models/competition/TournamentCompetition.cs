using AmdarisProject.models.competitor;
using AmdarisProject.utils;
using AmdarisProject.utils.exceptions;

namespace AmdarisProject.models.competition
{
    public class TournamentCompetition(string name, string location, DateTime startTime, Game game) : Competition(name, location, startTime, game)
    {
        public IEnumerable<Stage> Stages { get; set; } = [];

        protected override void CheckCompetitorNumber()
        {
            int competitorNumber = Competitors.Count();

            if (competitorNumber < 2)
                throw new CompetitorNumberException(MessageFormatter.Format(nameof(TournamentCompetition), nameof(CheckCompetitorNumber), Competitors.Count().ToString()));

            while (competitorNumber != 1)
            {
                if (competitorNumber % 2 == 1)
                    throw new CompetitorNumberException(MessageFormatter.Format(nameof(TournamentCompetition), nameof(CheckCompetitorNumber), Competitors.Count().ToString()));
                competitorNumber /= 2;
            }
        }

        protected override void CreateMatches()
        {
            throw new NotImplementedException();
        }

        public override Competitor GetWinner()
        {
            throw new NotImplementedException();
        }
    }
}
