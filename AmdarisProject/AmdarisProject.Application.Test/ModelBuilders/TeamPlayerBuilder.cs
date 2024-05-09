using AmdarisProject.Domain.Models.CompetitorModels;

namespace AmdarisProject.Application.Test.ModelBuilders
{
    public class TeamPlayerBuilder : ModelBuilder<TeamPlayer, TeamPlayerBuilder>
    {
        public TeamPlayerBuilder() : base(new TeamPlayer()
        {
            Id = Guid.NewGuid(),
            Team = Builder.CreateBasicTeam().Get(),
            Player = Builder.CreateBasicPlayer().Get(),
            IsActive = true
        })
        { }

        public override TeamPlayerBuilder Clone()
            => new TeamPlayerBuilder()
            .SetId(_model.Id)
            .SetTeam(_model.Team)
            .SetPlayer(_model.Player)
            .SetIsActive(_model.IsActive);

        public TeamPlayerBuilder SetTeam(Team team)
        {
            _model.Team = team;
            return this;
        }

        public TeamPlayerBuilder SetPlayer(Player player)
        {
            _model.Player = player;
            return this;
        }

        public TeamPlayerBuilder SetIsActive(bool isActive)
        {
            _model.IsActive = isActive;
            return this;
        }
    }
}
