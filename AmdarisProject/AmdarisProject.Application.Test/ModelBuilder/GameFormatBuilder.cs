using AmdarisProject.Application.Test.ModelBuilder.ModelBuilder;
using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Models;

namespace AmdarisProject.Application.Test.ModelBuilder
{
    internal class GameFormatBuilder : ModelBuilder<GameFormat, GameFormatBuilder>
    {
        private GameFormatBuilder(GameFormat gameFormat) : base(gameFormat) { }

        public static GameFormatBuilder CreateBasic()
            => new(new GameFormat()
            {
                Id = Guid.NewGuid(),
                Name = "PingPongPlayerWinAt11",
                GameType = GameType.PING_PONG,
                CompetitorType = CompetitorType.PLAYER,
                TeamSize = null,
                WinAt = 11,
                DurationInSeconds = null,
            });

        public GameFormatBuilder SetCompetitorType(CompetitorType competitorType)
        {
            _model.CompetitorType = competitorType;
            return this;
        }

        public GameFormatBuilder SetTeamSize(ushort? teamSize)
        {
            _model.TeamSize = teamSize;
            return this;
        }

        public GameFormatBuilder SetWinAt(ushort? winAt)
        {
            _model.WinAt = winAt;
            return this;
        }

        public GameFormatBuilder SetDurationInSeconds(ulong? durationInSeconds)
        {
            _model.DurationInSeconds = durationInSeconds;
            return this;
        }
    }
}
