using AmdarisProject.Application.Test.ModelBuilder.ModelBuilder;
using AmdarisProject.Domain.Models;
using AmdarisProject.Domain.Models.CompetitorModels;

namespace AmdarisProject.Application.Test.ModelBuilder
{
    internal class PointBuilder : ModelBuilder<Point, PointBuilder>
    {
        private PointBuilder(Point point) : base(point) { }

        public static PointBuilder CreateBasic()
            => new(new Point()
            {
                Id = Guid.NewGuid(),
                Value = 0,
                Match = Builders.CreateBasicMatch().Get(),
                Player = Builders.CreateBasicPlayer().Get(),
            });

        public PointBuilder SetValue(uint value)
        {
            _model.Value = value;
            return this;
        }

        public PointBuilder SetMatch(Match match)
        {
            _model.Match = match;
            return this;
        }

        public PointBuilder SetPlayer(Player player)
        {
            _model.Player = player;
            return this;
        }
    }
}
