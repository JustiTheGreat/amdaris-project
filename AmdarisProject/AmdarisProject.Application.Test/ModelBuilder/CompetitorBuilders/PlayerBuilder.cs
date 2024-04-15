﻿using AmdarisProject.Domain.Models.CompetitorModels;

namespace AmdarisProject.Application.Test.ModelBuilder.CompetitorBuilders
{
    internal class PlayerBuilder : CompetitiorBuilder<Player, PlayerBuilder>
    {
        private PlayerBuilder(Player player) : base(player) { }

        public static PlayerBuilder CreateBasic()
            => new(new Player()
            {
                Id = ++_instances,
                Name = "Test",
            });
    }
}
