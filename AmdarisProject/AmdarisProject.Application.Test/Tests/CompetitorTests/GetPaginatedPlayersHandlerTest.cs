using AmdarisProject.Application.Common.Models;
using AmdarisProject.Application.Dtos.ResponseDTOs.DisplayDTOs;
using AmdarisProject.Application.Handlers.CompetitorHandlers;
using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Models.CompetitionModels;
using AmdarisProject.Domain.Models.CompetitorModels;
using AmdarisProject.TestUtils.ModelBuilders;
using Moq;

namespace AmdarisProject.Application.Test.Tests.CompetitorTests
{
    public class GetPaginatedPlayersHandlerTest : ApplicationTestBase
    {
        [Fact]
        public async Task Test_GetPaginatedPlayersHandler_Success()
        {

            List<Player> players = [];
            for (int i = 0; i < _numberOfModelsInAList; i++) players.Add(APBuilder.CreateBasicPlayer().Get());
            var paginatedResult = Tuple.Create((IEnumerable<Player>)players, players.Count());
            _competitorRepositoryMock.Setup(o => o.GetPaginatedPlayers(It.IsAny<PagedRequest>()))
                .Returns(Task.FromResult(paginatedResult));
            _mapperMock.Setup(o => o.Map<IEnumerable<CompetitorDisplayDTO>>(It.IsAny<IEnumerable<Player>>()))
                .Returns(_mapper.Map<IEnumerable<CompetitorDisplayDTO>>(players));
            GetPaginatedPlayers command = new(_pagedRequest);
            GetPaginatedPlayersHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object,
                GetLogger<GetPaginatedPlayersHandler>());

            PaginatedResult<CompetitorDisplayDTO> response = await handler.Handle(command, default);

            _competitorRepositoryMock.Verify(o => o.GetPaginatedPlayers(It.IsAny<PagedRequest>()), Times.Once);
            Assert.Equal(_pagedRequest.PageIndex, response.PageIndex);
            Assert.Equal(_pagedRequest.PageSize, response.PageSize);
            Assert.Equal(_pagedRequest.PageSize, response.Total);
            Assert.Equal(_pagedRequest.PageSize, response.Items.Count());
            for (int i = 0; i < players.Count; i++)
            {
                Player player = players[i];
                CompetitorDisplayDTO competitorDisplayDTO = response.Items.ElementAt(i);

                Assert.Equal(player.Name, competitorDisplayDTO.Name);
                Assert.Equal(CompetitorType.PLAYER.ToString(), competitorDisplayDTO.CompetitorType);
                Assert.Equal(player.Competitions.Count(), competitorDisplayDTO.NumberOfCompetitions);
                Assert.Equal(player.Matches.Count(), competitorDisplayDTO.NumberOfMatches);
                Assert.Equal(player.Teams.Count(), competitorDisplayDTO.NumberOfTeams);
                Assert.Null(competitorDisplayDTO.NumberOfPlayers);
                Assert.Null(competitorDisplayDTO.NumberOfActivePlayers);
            }
        }
    }
}
