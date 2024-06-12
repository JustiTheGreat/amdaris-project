using AmdarisProject.Application.Common.Models;
using AmdarisProject.Application.Dtos.ResponseDTOs.DisplayDTOs;
using AmdarisProject.Application.Handlers.CompetitorHandlers;
using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Models;
using AmdarisProject.Domain.Models.CompetitorModels;
using AmdarisProject.TestUtils.ModelBuilders;
using Moq;
using System.Numerics;

namespace AmdarisProject.Application.Test.Tests.CompetitorTests
{
    public class GetPaginatedTeamsHandlerTest : ApplicationTestBase
    {
        [Fact]
        public async Task Test_GetPaginatedTeamsHandler_Success()
        {
            List<Team> teams = [];
            for (int i = 0; i < _numberOfModelsInAList; i++) teams.Add(APBuilder.CreateBasicTeam().Get());
            var paginatedResult = Tuple.Create((IEnumerable<Team>)teams, teams.Count());
            _competitorRepositoryMock.Setup(o => o.GetPaginatedTeams(It.IsAny<PagedRequest>()))
                .Returns(Task.FromResult(paginatedResult));
            _mapperMock.Setup(o => o.Map<IEnumerable<CompetitorDisplayDTO>>(It.IsAny<IEnumerable<Team>>()))
                .Returns(_mapper.Map<IEnumerable<CompetitorDisplayDTO>>(teams));
            GetPaginatedTeams command = new(_pagedRequest);
            GetPaginatedTeamsHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object,
                GetLogger<GetPaginatedTeamsHandler>());

            PaginatedResult<CompetitorDisplayDTO> response = await handler.Handle(command, default);

            _competitorRepositoryMock.Verify(o => o.GetPaginatedTeams(It.IsAny<PagedRequest>()), Times.Once);
            Assert.Equal(_pagedRequest.PageIndex, response.PageIndex);
            Assert.Equal(_pagedRequest.PageSize, response.PageSize);
            Assert.Equal(_pagedRequest.PageSize, response.Total);
            Assert.Equal(_pagedRequest.PageSize, response.Items.Count());
            for (int i = 0; i < teams.Count; i++)
            {
                Team team = teams[i];
                CompetitorDisplayDTO competitorDisplayDTO = response.Items.ElementAt(i);

                Assert.Equal(team.Id, competitorDisplayDTO.Id);
                Assert.Equal(team.Name, competitorDisplayDTO.Name);
                Assert.Equal(CompetitorType.TEAM.ToString(), competitorDisplayDTO.CompetitorType);
                Assert.Equal(team.Competitions.Count(), competitorDisplayDTO.NumberOfCompetitions);
                Assert.Equal(team.Matches.Count(), competitorDisplayDTO.NumberOfMatches);
                Assert.Null(competitorDisplayDTO.NumberOfTeams);
                Assert.Equal(team.TeamPlayers.Count(), competitorDisplayDTO.NumberOfPlayers);
                Assert.Equal(team.TeamPlayers.Count(teamPlayer => teamPlayer.IsActive), competitorDisplayDTO.NumberOfActivePlayers);
            }
        }
    }
}
