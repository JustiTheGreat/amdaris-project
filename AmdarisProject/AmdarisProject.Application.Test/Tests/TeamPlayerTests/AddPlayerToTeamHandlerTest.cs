using AmdarisProject.Application.Dtos.ResponseDTOs.DisplayDTOs;
using AmdarisProject.Application.Handlers.TeamPlayerHandlers;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models;
using AmdarisProject.Domain.Models.CompetitorModels;
using AmdarisProject.TestUtils.ModelBuilders;
using AmdarisProject.TestUtils.ModelBuilders.CompetitorBuilders;
using Moq;

namespace AmdarisProject.Application.Test.Tests.TeamPlayerTests
{
    public class AddPlayerToTeamHandlerTest : ApplicationTestBase
    {
        [Fact]
        public async Task Test_AddPlayerToTeamHandler_Success()
        {
            TeamPlayer teamPlayer = APBuilder.CreateBasicTeamPlayer().Get();
            _competitorRepositoryMock.Setup(o => o.GetTeamById(It.IsAny<Guid>()))
                .Returns(Task.FromResult((Team?)teamPlayer.Team));
            _competitorRepositoryMock.Setup(o => o.GetPlayerById(It.IsAny<Guid>()))
                .Returns(Task.FromResult((Player?)teamPlayer.Player));
            _teamPlayerRepositoryMock.Setup(o => o.Create(It.IsAny<TeamPlayer>())).Returns(Task.FromResult(teamPlayer));
            _mapperMock.Setup(o => o.Map<TeamPlayerDisplayDTO>(It.IsAny<TeamPlayer>()))
                .Returns(_mapper.Map<TeamPlayerDisplayDTO>(teamPlayer));
            AddPlayerToTeam command = new(teamPlayer.Team.Id, teamPlayer.Player.Id);
            AddPlayerToTeamHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object,
                GetLogger<AddPlayerToTeamHandler>());

            TeamPlayerDisplayDTO response = await handler.Handle(command, default);

            Assert.Equal(teamPlayer.Team.Id, response.TeamId);
            Assert.Equal(teamPlayer.Player.Id, response.PlayerId);
            Assert.Equal(teamPlayer.IsActive, response.IsActive);
        }

        [Fact]
        public async Task Test_AddPlayerToTeamHandler_TeamNotFound_throws_APNotFoundException()
        {
            _competitorRepositoryMock.Setup(o => o.GetTeamById(It.IsAny<Guid>())).Returns(Task.FromResult((Team?)null));
            AddPlayerToTeam command = new(It.IsAny<Guid>(), It.IsAny<Guid>());
            AddPlayerToTeamHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object,
                GetLogger<AddPlayerToTeamHandler>());

            await Assert.ThrowsAsync<APNotFoundException>(async () => await handler.Handle(command, default));
        }

        [Fact]
        public async Task Test_AddPlayerToTeamHandler_PlayerNotFound_throws_APNotFoundException()
        {
            Team team = APBuilder.CreateBasicTeam().Get();
            _competitorRepositoryMock.Setup(o => o.GetTeamById(It.IsAny<Guid>()))
                .Returns(Task.FromResult((Team?)team));
            _competitorRepositoryMock.Setup(o => o.GetPlayerById(It.IsAny<Guid>()))
                .Returns(Task.FromResult((Player?)null));
            AddPlayerToTeam command = new(It.IsAny<Guid>(), It.IsAny<Guid>());
            AddPlayerToTeamHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object, GetLogger<AddPlayerToTeamHandler>());

            await Assert.ThrowsAsync<APNotFoundException>(async () => await handler.Handle(command, default));
        }

        [Fact]
        public async Task Test_AddPlayerToTeamHandler_PlayerIsAlreadyInATeam_throws_AmdarisProjectException()
        {
            TeamPlayer teamPlayer = APBuilder.CreateBasicTeamPlayer().Get();
            TeamBuilder teamBuilder = APBuilder.CreateBasicTeam().AddPlayer(teamPlayer.Player);
            _competitorRepositoryMock.Setup(o => o.GetTeamById(It.IsAny<Guid>()))
                .Returns(Task.FromResult((Team?)teamPlayer.Team));
            _competitorRepositoryMock.Setup(o => o.GetPlayerById(It.IsAny<Guid>()))
                .Returns(Task.FromResult((Player?)teamPlayer.Player));
            AddPlayerToTeam command = new(teamPlayer.Team.Id, teamPlayer.Player.Id);
            AddPlayerToTeamHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object,
                GetLogger<AddPlayerToTeamHandler>());

            await Assert.ThrowsAsync<APException>(async () => await handler.Handle(command, default));
        }

        [Fact]
        public async Task Test_AddPlayerToTeamHandler_RollbackIsCalled_throws_Exception()
        {
            TeamPlayer teamPlayer = APBuilder.CreateBasicTeamPlayer().Get();
            _competitorRepositoryMock.Setup(o => o.GetTeamById(It.IsAny<Guid>()))
                .Returns(Task.FromResult((Team?)teamPlayer.Team));
            _competitorRepositoryMock.Setup(o => o.GetPlayerById(It.IsAny<Guid>()))
                .Returns(Task.FromResult((Player?)teamPlayer.Player));
            _teamPlayerRepositoryMock.Setup(o => o.Create(It.IsAny<TeamPlayer>())).Throws<Exception>();
            AddPlayerToTeam command = new(teamPlayer.Team.Id, teamPlayer.Player.Id);
            AddPlayerToTeamHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object,
                GetLogger<AddPlayerToTeamHandler>());

            await Assert.ThrowsAsync<Exception>(async () => await handler.Handle(command, default));

            _unitOfWorkMock.Verify(x => x.RollbackTransactionAsync(), Times.Once);
        }
    }
}
