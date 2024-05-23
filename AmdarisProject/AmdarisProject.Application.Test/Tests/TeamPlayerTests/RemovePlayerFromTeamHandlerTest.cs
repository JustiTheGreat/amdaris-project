using AmdarisProject.Application.Handlers.TeamPlayerHandlers;
using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models;
using AmdarisProject.TestUtils.ModelBuilders;
using Moq;

namespace AmdarisProject.Application.Test.Tests.TeamPlayerTests
{
    public class RemovePlayerFromTeamHandlerTest : ApplicationTestBase
    {
        [Fact]
        public async Task Test_RemovePlayerFromTeamHandlerTest_Success()
        {
            TeamPlayer teamPlayer = APBuilder.CreateBasicTeamPlayer().Get();
            _teamPlayerRepositoryMock.Setup(o => o.GetByTeamAndPlayer(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .Returns(Task.FromResult((TeamPlayer?)teamPlayer));
            RemovePlayerFromTeam command = new(teamPlayer.Team.Id, teamPlayer.Player.Id);
            RemovePlayerFromTeamHandler handler = new(_unitOfWorkMock.Object, GetLogger<RemovePlayerFromTeamHandler>());

            bool response = await handler.Handle(command, default);

            Assert.True(response);
            _teamPlayerRepositoryMock.Verify(x => x.Delete(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async Task Test_RemovePlayerFromTeamHandlerTest_TeamPlayerNotFound_throws_APNotFoundException()
        {
            TeamPlayer teamPlayer = APBuilder.CreateBasicTeamPlayer().Get();
            _teamPlayerRepositoryMock.Setup(o => o.GetByTeamAndPlayer(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .Returns(Task.FromResult((TeamPlayer?)null));
            RemovePlayerFromTeam command = new(teamPlayer.Team.Id, teamPlayer.Player.Id);
            RemovePlayerFromTeamHandler handler = new(_unitOfWorkMock.Object, GetLogger<RemovePlayerFromTeamHandler>());

            await Assert.ThrowsAsync<APNotFoundException>(async () => await handler.Handle(command, default));
        }

        [Fact]
        public async Task Test_RemovePlayerFromTeamHandlerTest_TeamIsInAStartedMatch_throws_AmdarisProjectException()
        {
            TeamPlayer teamPlayer = APBuilder.CreateBasicTeamPlayer().Get();
            MatchBuilder matchBuilder = APBuilder.CreateBasicMatch().SetCompetitorOne(teamPlayer.Team).SetStatus(MatchStatus.STARTED);
            _teamPlayerRepositoryMock.Setup(o => o.GetByTeamAndPlayer(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .Returns(Task.FromResult((TeamPlayer?)teamPlayer));
            RemovePlayerFromTeam command = new(teamPlayer.Team.Id, teamPlayer.Player.Id);
            RemovePlayerFromTeamHandler handler = new(_unitOfWorkMock.Object, GetLogger<RemovePlayerFromTeamHandler>());

            await Assert.ThrowsAsync<APException>(async () => await handler.Handle(command, default));
        }

        [Fact]
        public async Task Test_RemovePlayerFromTeamHandlerTest_RollbackIsCalled_throws_Exception()
        {
            TeamPlayer teamPlayer = APBuilder.CreateBasicTeamPlayer().Get();
            _teamPlayerRepositoryMock.Setup(o => o.GetByTeamAndPlayer(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .Returns(Task.FromResult((TeamPlayer?)teamPlayer));
            _teamPlayerRepositoryMock.Setup(o => o.Delete(It.IsAny<Guid>())).Throws<Exception>();
            RemovePlayerFromTeam command = new(teamPlayer.Team.Id, teamPlayer.Player.Id);
            RemovePlayerFromTeamHandler handler = new(_unitOfWorkMock.Object, GetLogger<RemovePlayerFromTeamHandler>());

            await Assert.ThrowsAsync<Exception>(async () => await handler.Handle(command, default));

            _unitOfWorkMock.Verify(x => x.RollbackTransactionAsync(), Times.Once());
        }
    }
}
