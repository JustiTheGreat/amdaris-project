using AmdarisProject.Application.Test.ModelBuilders.CompetitionBuilders;
using AmdarisProject.Application.Test.ModelBuilders.CompetitorBuilders;

namespace AmdarisProject.Application.Test.ModelBuilders
{
    public class Builder
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
