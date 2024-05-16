using AmdarisProject.TestUtils.ModelBuilders.CompetitionBuilders;
using AmdarisProject.TestUtils.ModelBuilders.CompetitorBuilders;

namespace AmdarisProject.TestUtils.ModelBuilders
{
    public class APBuilder
    {
        public static PlayerBuilder CreateBasicPlayer() => new();

        public static TeamBuilder CreateBasicTeam() => new();

        public static GameFormatBuilder CreateBasicGameFormat() => new();

        public static OneVSAllCompetitionBuilder CreateBasicOneVSAllCompetition() => new();

        public static TournamentCompetitionBuilder CreateBasicTournamentCompetition() => new();

        public static MatchBuilder CreateBasicMatch() => new();

        public static PointBuilder CreateBasicPoint() => new();

        public static TeamPlayerBuilder CreateBasicTeamPlayer() => new();
    }
}
