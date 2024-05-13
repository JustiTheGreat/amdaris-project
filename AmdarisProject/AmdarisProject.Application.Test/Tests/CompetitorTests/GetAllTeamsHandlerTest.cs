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
            for (int i = 0; i < _numberOfModelsInAList; i++)
            {
                Assert.Equal(teams.ElementAt(i).Id, response.ElementAt(i).Id);
                Assert.Equal(teams.ElementAt(i).Name, response.ElementAt(i).Name);
                Assert.Equal(teams.ElementAt(i).Players.Count, response.ElementAt(i).PlayerNames.Count);
                teams.ElementAt(i).Players.ForEach(player =>
                {
                    Assert.Equal(player.Name, response.ElementAt(i).PlayerNames.FirstOrDefault(name => name.Equals(player.Name)));
                });
            }
        }
    }
}
