using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Dtos.ResponseDTOs.CompetitorResponseDTOs;
using AmdarisProject.Application.Handlers.CompetitorHandlers;
using AmdarisProject.Application.Test.ModelBuilder;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models.CompetitorModels;
using AmdarisProject.Presentation;
using Mapster;
using MapsterMapper;
using Moq;

namespace AmdarisProject.Application.Test.Tests.CompetitorTests
{
    public class AddPlayerToTeamHandlerTest
    {
        private readonly Mock<ICompetitorRepository> _competitorRepositoryMock = new();
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly Mock<IMapper> _mapperMock = new();

        public AddPlayerToTeamHandlerTest() => MapsterConfiguration.ConfigureMapster();

        [Fact]
        public async Task Test_AddPlayerToTeamHandler_Success()
        {
            Team team = Builders.CreateBasicTeam().Get();
            Player player = Builders.CreateBasicPlayer().Get();
            Team teamWithPlayer = Builders.CreateBasicTeam().SetId(team.Id).AddPlayer(player).Get();
            _unitOfWorkMock.Setup(o => o.CompetitorRepository).Returns(_competitorRepositoryMock.Object);
            _competitorRepositoryMock.SetupSequence(o => o.GetById(It.IsAny<Guid>()))
                .Returns(Task.FromResult((Competitor?)team)).Returns(Task.FromResult((Competitor?)player));
            _competitorRepositoryMock.Setup(o => o.Update(It.IsAny<Team>())).Returns(Task.FromResult((Competitor)team));
            _mapperMock.Setup(o => o.Map<TeamResponseDTO>(It.IsAny<Team>())).Returns(teamWithPlayer.Adapt<TeamResponseDTO>());
            AddPlayerToTeam command = new(player.Id, team.Id);
            AddPlayerToTeamHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object);

            TeamResponseDTO response = await handler.Handle(command, default);

            _competitorRepositoryMock.Verify(o => o.Update(It.Is<Team>(t => t.Players.Any(p => p.Id.Equals(player.Id)))), Times.Once);
            Assert.Equal(team.Id, response.Id);
            Assert.Equal(player.Id, response.Players[^1].Id);
        }

        [Fact]
        public async Task Test_AddPlayerToTeamHandler_TeamNotFound_Throws_APNotFoundException()
        {
            _unitOfWorkMock.Setup(o => o.CompetitorRepository).Returns(_competitorRepositoryMock.Object);
            _competitorRepositoryMock.Setup(o => o.GetById(It.IsAny<Guid>())).Returns(Task.FromResult((Competitor?)null));
            AddPlayerToTeam command = new(It.IsAny<Guid>(), It.IsAny<Guid>());
            AddPlayerToTeamHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object);

            await Assert.ThrowsAsync<APNotFoundException>(async () => await handler.Handle(command, default));
        }

        [Fact]
        public async Task Test_AddPlayerToTeamHandler_PlayerNotFound_Throws_APNotFoundException()
        {
            Team team = Builders.CreateBasicTeam().Get();
            _unitOfWorkMock.Setup(o => o.CompetitorRepository).Returns(_competitorRepositoryMock.Object);
            _competitorRepositoryMock.SetupSequence(o => o.GetById(It.IsAny<Guid>()))
                .Returns(Task.FromResult((Competitor?)team))
                .Returns(Task.FromResult((Competitor?)null));
            AddPlayerToTeam command = new(It.IsAny<Guid>(), It.IsAny<Guid>());
            AddPlayerToTeamHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object);

            await Assert.ThrowsAsync<APNotFoundException>(async () => await handler.Handle(command, default));
        }

        [Fact]
        public async Task Test_AddPlayerToTeamHandler_TeamIsFull_Throws_AmdarisProjectException()
        {
            Team fullTeam = Builders.CreateBasicTeam().FillTeamWithPlayers().Get();
            Player player = Builders.CreateBasicPlayer().Get();
            _unitOfWorkMock.Setup(o => o.CompetitorRepository).Returns(_competitorRepositoryMock.Object);
            _competitorRepositoryMock.SetupSequence(o => o.GetById(It.IsAny<Guid>()))
                .Returns(Task.FromResult((Competitor?)fullTeam)).Returns(Task.FromResult((Competitor?)player));
            AddPlayerToTeam command = new(player.Id, fullTeam.Id);
            AddPlayerToTeamHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object);

            await Assert.ThrowsAsync<AmdarisProjectException>(async () => await handler.Handle(command, default));
        }

        [Fact]
        public async Task Test_AddPlayerToTeamHandler_TeamContainsPlayer_Throws_AmdarisProjectException()
        {
            Player player = Builders.CreateBasicPlayer().Get();
            Team team = Builders.CreateBasicTeam().AddPlayer(player).Get();
            _unitOfWorkMock.Setup(o => o.CompetitorRepository).Returns(_competitorRepositoryMock.Object);
            _competitorRepositoryMock.SetupSequence(o => o.GetById(It.IsAny<Guid>()))
                .Returns(Task.FromResult((Competitor?)team)).Returns(Task.FromResult((Competitor?)player));
            AddPlayerToTeam command = new(player.Id, team.Id);
            AddPlayerToTeamHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object);

            await Assert.ThrowsAsync<AmdarisProjectException>(async () => await handler.Handle(command, default));
        }

        [Fact]
        public async Task Test_AddPlayerToTeamHandler_Transaction_Throws_Exception()
        {
            Team team = Builders.CreateBasicTeam().Get();
            Player player = Builders.CreateBasicPlayer().Get();
            _unitOfWorkMock.Setup(o => o.CompetitorRepository).Returns(_competitorRepositoryMock.Object);
            _competitorRepositoryMock.SetupSequence(o => o.GetById(It.IsAny<Guid>()))
                .Returns(Task.FromResult((Competitor?)team)).Returns(Task.FromResult((Competitor?)player));
            _competitorRepositoryMock.Setup(o => o.Update(It.IsAny<Team>())).Throws<Exception>();
            AddPlayerToTeam command = new(It.IsAny<Guid>(), It.IsAny<Guid>());
            AddPlayerToTeamHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object);

            await Assert.ThrowsAsync<Exception>(async () => await handler.Handle(command, default));

            _unitOfWorkMock.Verify(o => o.RollbackTransactionAsync(), Times.Once);
        }
    }
}
