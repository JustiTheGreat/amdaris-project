using AmdarisProject.Application.Test.ModelBuilder.CompetitionBuilders;
using AmdarisProject.Application.Test.ModelBuilder.CompetitorBuilders;

namespace AmdarisProject.Application.Test.ModelBuilder
{
    internal class Builders
    {
        public static PlayerBuilder CreateBasicPlayer()
        {
            return PlayerBuilder.CreateBasic();
        }

        public static TeamBuilder CreateBasicTeam()
        {
            return TeamBuilder.CreateBasic();
        }

        public static GameFormatBuilder CreateBasicGameFormat()
        {
            return GameFormatBuilder.CreateBasic();
        }

        public static OneVSAllCompetitionBuilder CreateBasicOneVSAllCompetition()
        {
            return OneVSAllCompetitionBuilder.CreateBasic();
        }

        public static TournamentCompetitionBuilder CreateBasicTournamentCompetition()
        {
            return TournamentCompetitionBuilder.CreateBasic();
        }

        public static MatchBuilder CreateBasicMatch()
        {
            return MatchBuilder.CreateBasic();
        }

        public static PointBuilder CreateBasicPoint()
        {
            return PointBuilder.CreateBasic();
        }
    }
}
