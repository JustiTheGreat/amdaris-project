using AmdarisProject.Application.Test.ModelBuilder.CompetitionBuilders;
using AmdarisProject.Application.Test.ModelBuilder.CompetitorBuilders;

namespace AmdarisProject.Application.Test.ModelBuilder
{
    internal class Builders
    {
        public static PlayerBuilder CreateBasicPlayer() => PlayerBuilder.CreateBasic();

        public static TeamBuilder CreateBasicTeam() => TeamBuilder.CreateBasic();

        public static GameFormatBuilder CreateBasicGameFormat() => GameFormatBuilder.CreateBasic();

        public static OneVSAllCompetitionBuilder CreateBasicOneVSAllCompetition() => OneVSAllCompetitionBuilder.CreateBasic();

        public static TournamentCompetitionBuilder CreateBasicTournamentCompetition() => TournamentCompetitionBuilder.CreateBasic();

        public static MatchBuilder CreateBasicMatch() => MatchBuilder.CreateBasic();

        public static PointBuilder CreateBasicPoint() => PointBuilder.CreateBasic();

        public static TeamPlayerBuilder CreateBasicTeamPlayer() => TeamPlayerBuilder.CreateBasic();
    }
}
