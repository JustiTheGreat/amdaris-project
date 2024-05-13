using AmdarisProject.Application.Dtos.ResponseDTOs.GetDTOs;
using AmdarisProject.Application.Handlers.TeamPlayerHandlers;
using AmdarisProject.Application.Test.ModelBuilders;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models.CompetitorModels;
using Mapster;
using Moq;

namespace AmdarisProject.Application.Test.Tests.TeamPlayerTests
{
    public class ChangeTeamPlayerStatusHandlerTest : MockObjectUser
    {
        [Fact]
        public async Task Test_ChangeTeamPlayerStatusHandler_Success()
        {
            bool newPlayerActivityStatus = true;
            TeamPlayerBuilder teamPlayerBuilder = APBuilder.CreateBasicTeamPlayer();
            TeamPlayer teamPlayer = teamPlayerBuilder.Get();
            TeamPlayer updated = teamPlayerBuilder.SetIsActive(newPlayerActivityStatus).Get();
            _unitOfWorkMock.Setup(o => o.TeamPlayerRepository).Returns(_teamPlayerRepositoryMock.Object);
            _unitOfWorkMock.Setup(o => o.MatchRepository).Returns(_matchRepositoryMock.Object);
            _teamPlayerRepositoryMock.Setup(o => o.GetByTeamAndPlayer(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(Task.FromResult((TeamPlayer?)teamPlayer));
            _matchRepositoryMock.Setup(o => o.CompetitorIsInAStartedMatch(It.IsAny<Guid>())).Returns(Task.FromResult(false));
            _teamPlayerRepositoryMock.Setup(o => o.Update(It.IsAny<TeamPlayer>())).Returns(Task.FromResult(updated));
            _mapperMock.Setup(o => o.Map<TeamPlayerGetDTO>(It.IsAny<TeamPlayer>())).Returns(updated.Adapt<TeamPlayerGetDTO>());
            ChangeTeamPlayerStatus command = new(teamPlayer.Team.Id, teamPlayer.Player.Id, newPlayerActivityStatus);
            ChangeTeamPlayerStatusHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object,
                GetLogger<ChangeTeamPlayerStatusHandler>());

            TeamPlayerGetDTO response = await handler.Handle(command, default);

            Assert.Equal(updated.Team.Id, response.TeamId);
            Assert.Equal(updated.Player.Id, response.PlayerId);
            Assert.Equal(updated.IsActive, response.IsActive);
        }

        [Fact]
        public async Task Test_ChangeTeamPlayerStatusHandler_TeamPlayerNotFound_throws_APNotFoundException()
        {
            _unitOfWorkMock.Setup(o => o.TeamPlayerRepository).Returns(_teamPlayerRepositoryMock.Object);
            _teamPlayerRepositoryMock.Setup(o => o.GetByTeamAndPlayer(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .Returns(Task.FromResult((TeamPlayer?)null));
            ChangeTeamPlayerStatus command = new(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<bool>());
            ChangeTeamPlayerStatusHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object,
                GetLogger<ChangeTeamPlayerStatusHandler>());

            await Assert.ThrowsAsync<APNotFoundException>(async () => await handler.Handle(command, default));
        }

        [Fact]
        public async Task Test_ChangeTeamPlayerStatusHandler_TeamIsInAStartedMatch_throws_AmdarisProjectException()
        {
            TeamPlayer teamPlayer = APBuilder.CreateBasicTeamPlayer().Get();
            _unitOfWorkMock.Setup(o => o.TeamPlayerRepository).Returns(_teamPlayerRepositoryMock.Object);
            _unitOfWorkMock.Setup(o => o.MatchRepository).Returns(_matchRepositoryMock.Object);
            _teamPlayerRepositoryMock.Setup(o => o.GetByTeamAndPlayer(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .Returns(Task.FromResult((TeamPlayer?)teamPlayer));
            _matchRepositoryMock.Setup(o => o.CompetitorIsInAStartedMatch(It.IsAny<Guid>())).Returns(Task.FromResult(true));
            ChangeTeamPlayerStatus command = new(teamPlayer.Team.Id, teamPlayer.Player.Id, It.IsAny<bool>());
            ChangeTeamPlayerStatusHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object,
                GetLogger<ChangeTeamPlayerStatusHandler>());

            await Assert.ThrowsAsync<AmdarisProjectException>(async () => await handler.Handle(command, default));
        }

        [Fact]
        public async Task Test_ChangeTeamPlayerStatusHandler_RollbackIsCalled_throws_Exception()
        {
            TeamPlayer teamPlayer = APBuilder.CreateBasicTeamPlayer().Get();
            _unitOfWorkMock.Setup(o => o.TeamPlayerRepository).Returns(_teamPlayerRepositoryMock.Object);
            _unitOfWorkMock.Setup(o => o.MatchRepository).Returns(_matchRepositoryMock.Object);
            _teamPlayerRepositoryMock.Setup(o => o.GetByTeamAndPlayer(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .Returns(Task.FromResult((TeamPlayer?)teamPlayer));
            _matchRepositoryMock.Setup(o => o.CompetitorIsInAStartedMatch(It.IsAny<Guid>())).Returns(Task.FromResult(false));
            _teamPlayerRepositoryMock.Setup(o => o.Update(It.IsAny<TeamPlayer>())).Throws<Exception>();
            ChangeTeamPlayerStatus command = new(teamPlayer.Team.Id, teamPlayer.Player.Id, It.IsAny<bool>());
            ChangeTeamPlayerStatusHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object,
                GetLogger<ChangeTeamPlayerStatusHandler>());

            await Assert.ThrowsAsync<Exception>(async () => await handler.Handle(command, default));

            _unitOfWorkMock.Verify(x => x.RollbackTransactionAsync(), Times.Once());
        }
    }
}
