using AmdarisProject.Application.Dtos.CreateDTOs;
using AmdarisProject.Application.Dtos.ResponseDTOs.CompetitorResponseDTOs;
using AmdarisProject.Application.Handlers.CompetitorHandlers;
using AmdarisProject.Application.Test.ModelBuilders;
using AmdarisProject.Domain.Models.CompetitorModels;
using Mapster;
using Moq;

namespace AmdarisProject.Application.Test.Tests.CompetitorTests
{
    public class CreateTeamHandlerTest : MockObjectUser
    {
        [Fact]
        public async Task Test_CreateTeamHandler_Success()
        {
            Team team = APBuilder.CreateBasicTeam().Get();
            CompetitorCreateDTO createDTO = team.Adapt<CompetitorCreateDTO>();
            _mapperMock.Setup(o => o.Map<Team>(It.IsAny<CompetitorCreateDTO>())).Returns(team);
            _unitOfWorkMock.Setup(o => o.CompetitorRepository).Returns(_competitorRepositoryMock.Object);
            _competitorRepositoryMock.Setup(o => o.Create(It.IsAny<Team>())).Returns(Task.FromResult((Competitor)team));
            _mapperMock.Setup(o => o.Map<TeamGetDTO>(It.IsAny<Team>())).Returns(team.Adapt<TeamGetDTO>());
            CreateTeam command = new(createDTO);
            CreateTeamHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object, GetLogger<CreateTeamHandler>());

            TeamGetDTO response = await handler.Handle(command, default);

            Assert.Equal(createDTO.Name, response.Name);
            Assert.Empty(response.Matches);
            Assert.Empty(response.WonMatches);
            Assert.Empty(response.Competitions);
            Assert.Empty(response.Players);
        }

        [Fact]
        public async Task Test_CreateTeamHandler_RollbackIsCalled_throws_Exception()
        {
            _unitOfWorkMock.Setup(o => o.CompetitorRepository).Returns(_competitorRepositoryMock.Object);
            _competitorRepositoryMock.Setup(o => o.Create(It.IsAny<Competitor>())).Throws<Exception>();
            CreateTeam command = new(It.IsAny<CompetitorCreateDTO>());
            CreateTeamHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object, GetLogger<CreateTeamHandler>());

            await Assert.ThrowsAsync<Exception>(async () => await handler.Handle(command, default));

            _unitOfWorkMock.Verify(o => o.RollbackTransactionAsync(), Times.Once);
        }
    }
}
