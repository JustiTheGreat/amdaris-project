using AmdarisProject.Application.Dtos.ResponseDTOs.GetDTOs;
using AmdarisProject.Application.Handlers.TeamPlayerHandlers;
using AmdarisProject.Application.Test.ModelBuilders;
using AmdarisProject.Domain.Models.CompetitorModels;
using AmdarisProject.Presentation.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace AmdarisProject.Presentation.Test.Tests
{
    public class TeamPlayerControllerTests : PresentationTestBase<TeamPlayerController>
    {
        [Fact]
        public async Task Test_AddPlayerToTeam_OkStatus()
        {
            Setup<AddPlayerToTeam, TeamPlayerGetDTO, AddPlayerToTeamHandler>();
            Seed_AddPlayerToTeam(out Team team, out Player player);

            var requestResult = await _controller.AddPlayerToTeam(team.Id, player.Id);

            var result = requestResult as OkObjectResult;
            var response = result?.Value as TeamPlayerGetDTO;
            Assert.Equal((int)HttpStatusCode.OK, result!.StatusCode);
            Assert.NotNull(response);
            Assert.Equal(team.Id, response.TeamId);
            Assert.Equal(player.Id, response.PlayerId);
            Assert.False(response.IsActive);
        }

        private void Seed_AddPlayerToTeam(out Team team, out Player player)
        {
            team = APBuilder.CreateBasicTeam().Get();
            _dbContext.Add(team);
            player = APBuilder.CreateBasicPlayer().Get();
            _dbContext.Add(player);
            _dbContext.SaveChanges();
            Detach(team);
            Detach(player);
        }

        [Fact]
        public async Task Test_ChangeTeamPlayerStatus_OkStatus()
        {
            Setup<AddPlayerToTeam, TeamPlayerGetDTO, AddPlayerToTeamHandler>();
            bool newStatus = true;
            Seed_ChangeTeamPlayerStatus(out TeamPlayer teamPlayer);

            var requestResult =
                await _controller.ChangeTeamPlayerStatus(teamPlayer.Team.Id, teamPlayer.Player.Id, newStatus);

            var result = requestResult as OkObjectResult;
            var response = result?.Value as TeamPlayerGetDTO;
            Assert.Equal((int)HttpStatusCode.OK, result!.StatusCode);
            Assert.NotNull(response);
            Assert.Equal(teamPlayer.Team.Id, response.TeamId);
            Assert.Equal(teamPlayer.Player.Id, response.PlayerId);
            Assert.Equal(newStatus, response.IsActive);
        }

        private void Seed_ChangeTeamPlayerStatus(out TeamPlayer teamPlayer)
        {
            teamPlayer = APBuilder.CreateBasicTeamPlayer().Get();
            _dbContext.Add(teamPlayer);
            _dbContext.SaveChanges();
            Detach(teamPlayer);
        }

        [Fact]
        public async Task Test_RemovePlayerFromTeam_OkStatus()
        {
            Setup<RemovePlayerFromTeam, bool, RemovePlayerFromTeamHandler>();
            Seed_RemovePlayerFromTeam(out TeamPlayer teamPlayer);

            var requestResult =
                await _controller.RemovePlayerFromTeam(teamPlayer.Team.Id, teamPlayer.Player.Id);

            var result = requestResult as OkResult;
            Assert.Equal((int)HttpStatusCode.OK, result!.StatusCode);
        }

        private void Seed_RemovePlayerFromTeam(out TeamPlayer teamPlayer)
        {
            teamPlayer = APBuilder.CreateBasicTeamPlayer().Get();
            _dbContext.Add(teamPlayer);
            _dbContext.SaveChanges();
        }
    }
}
