using AmdarisProject.Application.Dtos.RequestDTOs.CreateDTOs;
using AmdarisProject.Application.Dtos.ResponseDTOs.CompetitorResponseDTOs;
using AmdarisProject.Application.Handlers.CompetitorHandlers;
using AmdarisProject.Domain.Models.CompetitorModels;
using AmdarisProject.TestUtils;
using AmdarisProject.TestUtils.ModelBuilders;
using Moq;

namespace AmdarisProject.Application.Test.Tests.CompetitorTests
{
    public class CreateTeamHandlerTest : ApplicationTestBase
    {
        [Fact]
        public async Task Test_CreateTeamHandler_Success()
        {
            Team team = APBuilder.CreateBasicTeam().Get();
            _mapperMock.Setup(o => o.Map<Team>(It.IsAny<CompetitorCreateDTO>())).Returns(team);
            _competitorRepositoryMock.Setup(o => o.Create(It.IsAny<Team>())).Returns(Task.FromResult((Competitor)team));
            _mapperMock.Setup(o => o.Map<TeamGetDTO>(It.IsAny<Team>())).Returns(_mapper.Map<TeamGetDTO>(team));
            CreateTeam command = new(_mapper.Map<CompetitorCreateDTO>(team));
            CreateTeamHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object, GetLogger<CreateTeamHandler>());

            TeamGetDTO response = await handler.Handle(command, default);

            AssertResponse.TeamGetDTO(team, response, createTeam: true);
        }

        [Fact]
        public async Task Test_CreateTeamHandler_RollbackIsCalled_throws_Exception()
        {
            _competitorRepositoryMock.Setup(o => o.Create(It.IsAny<Competitor>())).Throws<Exception>();
            CreateTeam command = new(It.IsAny<CompetitorCreateDTO>());
            CreateTeamHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object, GetLogger<CreateTeamHandler>());

            await Assert.ThrowsAsync<Exception>(async () => await handler.Handle(command, default));

            _unitOfWorkMock.Verify(o => o.RollbackTransactionAsync(), Times.Once);
        }
    }
}
