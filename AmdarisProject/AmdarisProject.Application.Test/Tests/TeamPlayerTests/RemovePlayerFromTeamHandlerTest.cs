using AmdarisProject.Application.Handlers.TeamPlayerHandlers;
using AmdarisProject.Application.Test.ModelBuilders;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models.CompetitorModels;
using Moq;

namespace AmdarisProject.Application.Test.Tests.TeamPlayerTests
{
    public class RemovePlayerFromTeamHandlerTest : ApplicationTestBase
    {
        [Fact]
        public async Task Test_RemovePlayerFromTeamHandlerTest_Success()
        {
            TeamPlayer teamPlayer = APBuilder.CreateBasicTeamPlayer().Get();
            _unitOfWorkMock.Setup(o => o.TeamPlayerRepository).Returns(_teamPlayerRepositoryMock.Object);
            _unitOfWorkMock.Setup(o => o.MatchRepository).Returns(_matchRepositoryMock.Object);
            _teamPlayerRepositoryMock.Setup(o => o.GetByTeamAndPlayer(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .Returns(Task.FromResult((TeamPlayer?)teamPlayer));
            _matchRepositoryMock.Setup(o => o.CompetitorIsInAStartedMatch(It.IsAny<Guid>())).Returns(Task.FromResult(false));
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
            _unitOfWorkMock.Setup(o => o.TeamPlayerRepository).Returns(_teamPlayerRepositoryMock.Object);
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
            _unitOfWorkMock.Setup(o => o.TeamPlayerRepository).Returns(_teamPlayerRepositoryMock.Object);
            _unitOfWorkMock.Setup(o => o.MatchRepository).Returns(_matchRepositoryMock.Object);
            _teamPlayerRepositoryMock.Setup(o => o.GetByTeamAndPlayer(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .Returns(Task.FromResult((TeamPlayer?)teamPlayer));
            _matchRepositoryMock.Setup(o => o.CompetitorIsInAStartedMatch(It.IsAny<Guid>())).Returns(Task.FromResult(true));
            RemovePlayerFromTeam command = new(teamPlayer.Team.Id, teamPlayer.Player.Id);
            RemovePlayerFromTeamHandler handler = new(_unitOfWorkMock.Object, GetLogger<RemovePlayerFromTeamHandler>());

            await Assert.ThrowsAsync<AmdarisProjectException>(async () => await handler.Handle(command, default));
        }

        [Fact]
        public async Task Test_RemovePlayerFromTeamHandlerTest_RollbackIsCalled_throws_Exception()
        {
            TeamPlayer teamPlayer = APBuilder.CreateBasicTeamPlayer().Get();
            _unitOfWorkMock.Setup(o => o.TeamPlayerRepository).Returns(_teamPlayerRepositoryMock.Object);
            _unitOfWorkMock.Setup(o => o.MatchRepository).Returns(_matchRepositoryMock.Object);
            _teamPlayerRepositoryMock.Setup(o => o.GetByTeamAndPlayer(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .Returns(Task.FromResult((TeamPlayer?)teamPlayer));
            _matchRepositoryMock.Setup(o => o.CompetitorIsInAStartedMatch(It.IsAny<Guid>())).Returns(Task.FromResult(false));
            _teamPlayerRepositoryMock.Setup(o => o.Delete(It.IsAny<Guid>())).Throws<Exception>();
            RemovePlayerFromTeam command = new(teamPlayer.Team.Id, teamPlayer.Player.Id);
            RemovePlayerFromTeamHandler handler = new(_unitOfWorkMock.Object, GetLogger<RemovePlayerFromTeamHandler>());

            await Assert.ThrowsAsync<Exception>(async () => await handler.Handle(command, default));

            _unitOfWorkMock.Verify(x => x.RollbackTransactionAsync(), Times.Once());
        }
    }
}
