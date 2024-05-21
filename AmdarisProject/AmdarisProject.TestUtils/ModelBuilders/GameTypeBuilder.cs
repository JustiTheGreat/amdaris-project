using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Models;

namespace AmdarisProject.TestUtils.ModelBuilders
{
    public class GameTypeBuilder : ModelBuilder<GameType, GameTypeBuilder>
    {
        public GameTypeBuilder() : base(new GameType()
        {
            Id = Guid.NewGuid(),
            Name = "Test",
        })
        { }

        public override GameTypeBuilder Clone()
            => new GameTypeBuilder()
            .SetId(_model.Id)
            .SetName(_model.Name);

        public GameTypeBuilder SetName(string name)
        {
            _model.Name = name;
            return this;
        }
    }
}
