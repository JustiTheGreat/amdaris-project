using AmdarisProject.Application.Dtos.ResponseDTOs.GetDTOs;
using AmdarisProject.Application.Handlers.TeamPlayerHandlers;
using AmdarisProject.Application.Test.ModelBuilders;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models.CompetitorModels;
using Mapster;
using Moq;

namespace AmdarisProject.Application.Test.Tests.TeamPlayerTests
{
    public class AddPlayerToTeamHandlerTest : MockObjectUser
    {
        [Fact]
        public async Task Test_AddPlayerToTeamHandler_Success()
        {
            TeamPlayer teamPlayer = Builder.CreateBasicTeamPlayer().Get();
            _unitOfWorkMock.Setup(o => o.CompetitorRepository).Returns(_competitorRepositoryMock.Object);
            _unitOfWorkMock.Setup(o => o.TeamPlayerRepository).Returns(_teamPlayerRepositoryMock.Object);
            _competitorRepositoryMock.Setup(o => o.GetTeamById(It.IsAny<Guid>()))
                .Returns(Task.FromResult((Team?)teamPlayer.Team));
            _competitorRepositoryMock.Setup(o => o.GetPlayerById(It.IsAny<Guid>()))
                .Returns(Task.FromResult((Player?)teamPlayer.Player));
            _competitorRepositoryMock.Setup(o => o.PlayerIsInATeam(It.IsAny<Guid>())).Returns(Task.FromResult(false));
            _teamPlayerRepositoryMock.Setup(o => o.Create(It.IsAny<TeamPlayer>())).Returns(Task.FromResult(teamPlayer));
            _mapperMock.Setup(o => o.Map<TeamPlayerGetDTO>(It.IsAny<TeamPlayer>())).Returns(teamPlayer.Adapt<TeamPlayerGetDTO>());
            AddPlayerToTeam command = new(teamPlayer.Player.Id, teamPlayer.Team.Id);
            AddPlayerToTeamHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object,
                GetLogger<AddPlayerToTeamHandler>());

            TeamPlayerGetDTO response = await handler.Handle(command, default);

            Assert.Equal(teamPlayer.Team.Id, response.TeamId);
            Assert.Equal(teamPlayer.Player.Id, response.PlayerId);
            Assert.Equal(teamPlayer.IsActive, response.IsActive);
        }

        [Fact]
        public async Task Test_AddPlayerToTeamHandler_TeamNotFound_throws_APNotFoundException()
        {
            _unitOfWorkMock.Setup(o => o.CompetitorRepository).Returns(_competitorRepositoryMock.Object);
            _competitorRepositoryMock.Setup(o => o.GetTeamById(It.IsAny<Guid>())).Returns(Task.FromResult((Team?)null));
            AddPlayerToTeam command = new(It.IsAny<Guid>(), It.IsAny<Guid>());
            AddPlayerToTeamHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object,
                GetLogger<AddPlayerToTeamHandler>());

            await Assert.ThrowsAsync<APNotFoundException>(async () => await handler.Handle(command, default));
        }

        [Fact]
        public async Task Test_AddPlayerToTeamHandler_PlayerNotFound_throws_APNotFoundException()
        {
            Team team = Builder.CreateBasicTeam().Get();
            _unitOfWorkMock.Setup(o => o.CompetitorRepository).Returns(_competitorRepositoryMock.Object);
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
            TeamPlayer teamPlayer = Builder.CreateBasicTeamPlayer().Get();
            _unitOfWorkMock.Setup(o => o.CompetitorRepository).Returns(_competitorRepositoryMock.Object);
            _unitOfWorkMock.Setup(o => o.TeamPlayerRepository).Returns(_teamPlayerRepositoryMock.Object);
            _competitorRepositoryMock.Setup(o => o.GetTeamById(It.IsAny<Guid>()))
                .Returns(Task.FromResult((Team?)teamPlayer.Team));
            _competitorRepositoryMock.Setup(o => o.GetPlayerById(It.IsAny<Guid>()))
                .Returns(Task.FromResult((Player?)teamPlayer.Player));
            _competitorRepositoryMock.Setup(o => o.PlayerIsInATeam(It.IsAny<Guid>())).Returns(Task.FromResult(true));
            AddPlayerToTeam command = new(teamPlayer.Player.Id, teamPlayer.Team.Id);
            AddPlayerToTeamHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object,
                GetLogger<AddPlayerToTeamHandler>());

            await Assert.ThrowsAsync<AmdarisProjectException>(async () => await handler.Handle(command, default));
        }

        [Fact]
        public async Task Test_AddPlayerToTeamHandler_RollbackIsCalled_throws_Exception()
        {
            TeamPlayer teamPlayer = Builder.CreateBasicTeamPlayer().Get();
            _unitOfWorkMock.Setup(o => o.CompetitorRepository).Returns(_competitorRepositoryMock.Object);
            _unitOfWorkMock.Setup(o => o.TeamPlayerRepository).Returns(_teamPlayerRepositoryMock.Object);
            _competitorRepositoryMock.Setup(o => o.GetTeamById(It.IsAny<Guid>()))
                .Returns(Task.FromResult((Team?)teamPlayer.Team));
            _competitorRepositoryMock.Setup(o => o.GetPlayerById(It.IsAny<Guid>()))
                .Returns(Task.FromResult((Player?)teamPlayer.Player));
            _competitorRepositoryMock.Setup(o => o.PlayerIsInATeam(It.IsAny<Guid>())).Returns(Task.FromResult(false));
            _teamPlayerRepositoryMock.Setup(o => o.Create(It.IsAny<TeamPlayer>())).Throws<Exception>();
            AddPlayerToTeam command = new(teamPlayer.Player.Id, teamPlayer.Team.Id);
            AddPlayerToTeamHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object,
                GetLogger<AddPlayerToTeamHandler>());

            await Assert.ThrowsAsync<Exception>(async () => await handler.Handle(command, default));

            _unitOfWorkMock.Verify(x => x.RollbackTransactionAsync(), Times.Once);
        }
    }
}
