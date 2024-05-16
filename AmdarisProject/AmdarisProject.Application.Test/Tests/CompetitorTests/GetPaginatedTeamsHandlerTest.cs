using AmdarisProject.Application.Common.Models;
using AmdarisProject.Application.Dtos.DisplayDTOs.CompetitorDisplayDTOs;
using AmdarisProject.Application.Handlers.CompetitorHandlers;
using AmdarisProject.Domain.Models.CompetitorModels;
using AmdarisProject.TestUtils.ModelBuilders;
using Moq;

namespace AmdarisProject.Application.Test.Tests.CompetitorTests
{
    public class GetPaginatedTeamsHandlerTest : ApplicationTestBase
    {
        [Fact]
        public async Task Test_GetPaginatedTeamsHandler_Success()
        {
            List<Team> teams = [];
            for (int i = 0; i < _numberOfModelsInAList; i++) teams.Add(APBuilder.CreateBasicTeam().Get());
            _competitorRepositoryMock.Setup(o => o.GetPaginatedTeams(It.IsAny<PagedRequest>()))
                .Returns(Task.FromResult((IEnumerable<Team>)teams));
            _mapperMock.Setup(o => o.Map<IEnumerable<TeamDisplayDTO>>(It.IsAny<IEnumerable<Team>>()))
                .Returns(_mapper.Map<IEnumerable<TeamDisplayDTO>>(teams));
            GetPaginatedTeams command = new(_pagedRequest);
            GetPaginatedTeamsHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object,
                GetLogger<GetPaginatedTeamsHandler>());

            PaginatedResult<TeamDisplayDTO> response = await handler.Handle(command, default);

            _competitorRepositoryMock.Verify(o => o.GetPaginatedTeams(It.IsAny<PagedRequest>()), Times.Once);
            Assert.Equal(_pagedRequest.PageIndex, response.PageIndex);
            Assert.Equal(_pagedRequest.PageSize, response.PageSize);
            Assert.Equal(_pagedRequest.PageSize, response.Total);
            Assert.Equal(_pagedRequest.PageSize, response.Items.Count());
            for (int i = 0; i < teams.Count; i++)
            {
                Team team = teams[i];
                TeamDisplayDTO teamDisplayDTO = response.Items.ElementAt(i);

                Assert.Equal(team.Id, teamDisplayDTO.Id);
                Assert.Equal(team.Name, teamDisplayDTO.Name);
                Assert.Equal(team.Players.Count, teamDisplayDTO.PlayerNames.Count);
                team.Players.ForEach(player => Assert.Contains(player.Name, teamDisplayDTO.PlayerNames));
            }
        }
    }
}
