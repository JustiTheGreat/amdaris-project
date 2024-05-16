using AmdarisProject.Application.Common.Models;
using AmdarisProject.Application.Dtos.DisplayDTOs.CompetitorDisplayDTOs;
using AmdarisProject.Application.Handlers.CompetitorHandlers;
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
            _competitorRepositoryMock.Setup(o => o.GetPaginatedPlayers(It.IsAny<PagedRequest>()))
                .Returns(Task.FromResult((IEnumerable<Player>)players));
            _mapperMock.Setup(o => o.Map<IEnumerable<PlayerDisplayDTO>>(It.IsAny<IEnumerable<Player>>()))
                .Returns(_mapper.Map<IEnumerable<PlayerDisplayDTO>>(players));
            GetPaginatedPlayers command = new(_pagedRequest);
            GetPaginatedPlayersHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object,
                GetLogger<GetPaginatedPlayersHandler>());

            PaginatedResult<PlayerDisplayDTO> response = await handler.Handle(command, default);

            _competitorRepositoryMock.Verify(o => o.GetPaginatedPlayers(It.IsAny<PagedRequest>()), Times.Once);
            Assert.Equal(_pagedRequest.PageIndex, response.PageIndex);
            Assert.Equal(_pagedRequest.PageSize, response.PageSize);
            Assert.Equal(_pagedRequest.PageSize, response.Total);
            Assert.Equal(_pagedRequest.PageSize, response.Items.Count());
            for (int i = 0; i < players.Count; i++)
            {
                Player player = players[i];
                PlayerDisplayDTO playerDisplayDTO = response.Items.ElementAt(i);

                Assert.Equal(player.Id, playerDisplayDTO.Id);
                Assert.Equal(player.Name, playerDisplayDTO.Name);
            }
        }
    }
}
