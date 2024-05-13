using AmdarisProject.Application.Dtos.DisplayDTOs.CompetitorDisplayDTOs;
using AmdarisProject.Application.Handlers.CompetitorHandlers;
using AmdarisProject.Application.Test.ModelBuilders;
using AmdarisProject.Domain.Models.CompetitorModels;
using Mapster;
using Moq;

namespace AmdarisProject.Application.Test.Tests.CompetitorTests
{
    public class GetAllTeamsHandlerTest : MockObjectUser
    {
        [Fact]
        public async Task Test_GetAllTeamsHandler_Success()
        {
            List<Team> teams = [];
            for (int i = 0; i < _numberOfModelsInAList; i++) teams.Add(APBuilder.CreateBasicTeam().Get());
            _unitOfWorkMock.Setup(o => o.CompetitorRepository).Returns(_competitorRepositoryMock.Object);
            _competitorRepositoryMock.Setup(o => o.GetAllTeams()).Returns(Task.FromResult((IEnumerable<Team>)teams));
            _mapperMock.Setup(o => o.Map<IEnumerable<TeamDisplayDTO>>(It.IsAny<IEnumerable<Team>>()))
                .Returns(teams.Adapt<IEnumerable<TeamDisplayDTO>>());
            GetAllTeams command = new();
            GetAllTeamsHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object, GetLogger<GetAllTeamsHandler>());

            IEnumerable<TeamDisplayDTO> response = await handler.Handle(command, default);

            _competitorRepositoryMock.Verify(o => o.GetAllTeams(), Times.Once);
            Assert.Equal(teams.Count, response.Count());
            for (int i = 0; i < teams.Count; i++)
            {
                Team team = teams[i];
                TeamDisplayDTO teamDisplayDTO = response.ElementAt(i);

                Assert.Equal(team.Id, teamDisplayDTO.Id);
                Assert.Equal(team.Name, teamDisplayDTO.Name);
                Assert.Equal(team.Players.Count, teamDisplayDTO.PlayerNames.Count);
                team.Players.ForEach(player => Assert.Contains(player.Name, teamDisplayDTO.PlayerNames));
            }
        }
    }
}
