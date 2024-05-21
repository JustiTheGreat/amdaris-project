using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Models;

namespace AmdarisProject.TestUtils.ModelBuilders
{
    public class GameFormatBuilder : ModelBuilder<GameFormat, GameFormatBuilder>
    {
        public GameFormatBuilder() : base(new GameFormat()
        {
            Id = Guid.NewGuid(),
            Name = "Test",
            GameType = APBuilder.CreateBasicGameType().Get(),
            CompetitorType = CompetitorType.PLAYER,
            TeamSize = null,
            WinAt = 3,
            DurationInMinutes = null,
        })
        { }

        public override GameFormatBuilder Clone()
            => new GameFormatBuilder()
            .SetId(_model.Id)
            .SetName(_model.Name)
            .SetGameType(_model.GameType)
            .SetCompetitorType(_model.CompetitorType)
            .SetTeamSize(_model.TeamSize)
            .SetWinAt(_model.WinAt)
            .SetDurationInMinutes(_model.DurationInMinutes);

        public GameFormatBuilder SetName(string name)
        {
            _model.Name = name;
            return this;
        }

        public GameFormatBuilder SetGameType(GameType gameType)
        {
            _model.GameType = gameType;
            return this;
        }

        public GameFormatBuilder SetCompetitorType(CompetitorType competitorType)
        {
            _model.CompetitorType = competitorType;
            return this;
        }

        public GameFormatBuilder SetTeamSize(uint? teamSize)
        {
            _model.TeamSize = teamSize;
            return this;
        }

        public GameFormatBuilder SetWinAt(uint? winAt)
        {
            _model.WinAt = winAt;
            return this;
        }

        public GameFormatBuilder SetDurationInMinutes(ulong? durationInMinutes)
        {
            _model.DurationInMinutes = durationInMinutes;
            return this;
        }
    }
}
