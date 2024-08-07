﻿using AmdarisProject.TestUtils.ModelBuilders.CompetitionBuilders;
using AmdarisProject.TestUtils.ModelBuilders.CompetitorBuilders;

namespace AmdarisProject.TestUtils.ModelBuilders
{
    public class APBuilder
    {
        public static PlayerBuilder CreateBasicPlayer() => new();

        public static TeamBuilder CreateBasicTeam() => new();

        public static GameFormatBuilder CreateBasicGameFormat() => new();

        public static GameTypeBuilder CreateBasicGameType() => new();

        public static OneVSAllCompetitionBuilder CreateBasicOneVSAllCompetition() => new(DateTimeOffset.UtcNow);

        public static TournamentCompetitionBuilder CreateBasicTournamentCompetition() => new(DateTimeOffset.UtcNow);

        public static MatchBuilder CreateBasicMatch() => new();

        public static PointBuilder CreateBasicPoint() => new();

        public static TeamPlayerBuilder CreateBasicTeamPlayer() => new();
    }
}
