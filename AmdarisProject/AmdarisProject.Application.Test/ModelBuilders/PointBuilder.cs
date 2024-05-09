﻿using AmdarisProject.Domain.Models;
using AmdarisProject.Domain.Models.CompetitorModels;

namespace AmdarisProject.Application.Test.ModelBuilders
{
    public class PointBuilder : ModelBuilder<Point, PointBuilder>
    {
        public PointBuilder() : base(new Point()
        {
            Id = Guid.NewGuid(),
            Value = 0,
            Match = Builder.CreateBasicMatch().Get(),
            Player = Builder.CreateBasicPlayer().Get(),
        })
        { }

        public override PointBuilder Clone()
            => new PointBuilder()
            .SetId(_model.Id)
            .SetValue(_model.Value)
            .SetMatch(_model.Match)
            .SetPlayer(_model.Player);

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
