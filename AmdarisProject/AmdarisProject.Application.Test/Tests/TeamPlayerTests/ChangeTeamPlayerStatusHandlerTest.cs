using AmdarisProject.Application.Dtos.ResponseDTOs.DisplayDTOs;
using AmdarisProject.Application.Handlers.TeamPlayerHandlers;
using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models;
using AmdarisProject.TestUtils.ModelBuilders;
using Moq;

namespace AmdarisProject.Application.Test.Tests.TeamPlayerTests
{
    public class ChangeTeamPlayerStatusHandlerTest : ApplicationTestBase
    {
        [Fact]
        public async Task Test_ChangeTeamPlayerStatusHandler_Success()
        {
            TeamPlayerBuilder teamPlayerBuilder = APBuilder.CreateBasicTeamPlayer();
            TeamPlayer teamPlayer = teamPlayerBuilder.Get();
            _teamPlayerRepositoryMock.Setup(o => o.GetByTeamAndPlayer(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .Returns(Task.FromResult((TeamPlayer?)teamPlayer));
            teamPlayer = teamPlayerBuilder.SetIsActive(!teamPlayer.IsActive).Get();
            _teamPlayerRepositoryMock.Setup(o => o.Update(It.IsAny<TeamPlayer>())).Returns(Task.FromResult(teamPlayer));
            _mapperMock.Setup(o => o.Map<TeamPlayerDisplayDTO>(It.IsAny<TeamPlayer>()))
                .Returns(_mapper.Map<TeamPlayerDisplayDTO>(teamPlayer));
            ChangeTeamPlayerStatus command = new(teamPlayer.Team.Id, teamPlayer.Player.Id);
            ChangeTeamPlayerStatusHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object,
                GetLogger<ChangeTeamPlayerStatusHandler>());

            TeamPlayerDisplayDTO response = await handler.Handle(command, default);

            Assert.Equal(teamPlayer.Team.Id, response.TeamId);
            Assert.Equal(teamPlayer.Player.Id, response.PlayerId);
            Assert.Equal(teamPlayer.IsActive, response.IsActive);
        }

        [Fact]
        public async Task Test_ChangeTeamPlayerStatusHandler_TeamPlayerNotFound_throws_APNotFoundException()
        {
            _teamPlayerRepositoryMock.Setup(o => o.GetByTeamAndPlayer(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .Returns(Task.FromResult((TeamPlayer?)null));
            ChangeTeamPlayerStatus command = new(It.IsAny<Guid>(), It.IsAny<Guid>());
            ChangeTeamPlayerStatusHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object,
                GetLogger<ChangeTeamPlayerStatusHandler>());

            await Assert.ThrowsAsync<APNotFoundException>(async () => await handler.Handle(command, default));
        }

        [Fact]
        public async Task Test_ChangeTeamPlayerStatusHandler_TeamIsInAStartedMatch_throws_AmdarisProjectException()
        {
            TeamPlayer teamPlayer = APBuilder.CreateBasicTeamPlayer().Get();
            MatchBuilder matchBuilder = APBuilder.CreateBasicMatch().SetCompetitorOne(teamPlayer.Team).SetStatus(MatchStatus.STARTED);
            _teamPlayerRepositoryMock.Setup(o => o.GetByTeamAndPlayer(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .Returns(Task.FromResult((TeamPlayer?)teamPlayer));
            ChangeTeamPlayerStatus command = new(teamPlayer.Team.Id, teamPlayer.Player.Id);
            ChangeTeamPlayerStatusHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object,
                GetLogger<ChangeTeamPlayerStatusHandler>());

            await Assert.ThrowsAsync<APException>(async () => await handler.Handle(command, default));
        }

        [Fact]
        public async Task Test_ChangeTeamPlayerStatusHandler_RollbackIsCalled_throws_Exception()
        {
            TeamPlayer teamPlayer = APBuilder.CreateBasicTeamPlayer().Get();
            _teamPlayerRepositoryMock.Setup(o => o.GetByTeamAndPlayer(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .Returns(Task.FromResult((TeamPlayer?)teamPlayer));
            _teamPlayerRepositoryMock.Setup(o => o.Update(It.IsAny<TeamPlayer>())).Throws<Exception>();
            ChangeTeamPlayerStatus command = new(teamPlayer.Team.Id, teamPlayer.Player.Id);
            ChangeTeamPlayerStatusHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object,
                GetLogger<ChangeTeamPlayerStatusHandler>());

            await Assert.ThrowsAsync<Exception>(async () => await handler.Handle(command, default));

            _unitOfWorkMock.Verify(x => x.RollbackTransactionAsync(), Times.Once());
        }
    }
}
