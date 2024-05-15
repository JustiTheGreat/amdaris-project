using AmdarisProject.Application.Dtos.DisplayDTOs.CompetitorDisplayDTOs;
using AmdarisProject.Application.Handlers.CompetitorHandlers;
using AmdarisProject.Application.Test.ModelBuilders;
using AmdarisProject.Domain.Models.CompetitorModels;
using Mapster;
using Moq;

namespace AmdarisProject.Application.Test.Tests.CompetitorTests
{
    public class GetAllPlayersHandlerTest : MockObjectUser
    {
        [Fact]
        public async Task Test_GetAllPlayersHandler_Success()
        {
            List<Player> players = [];
            for (int i = 0; i < _numberOfModelsInAList; i++) players.Add(APBuilder.CreateBasicPlayer().Get());
            _unitOfWorkMock.Setup(o => o.CompetitorRepository).Returns(_competitorRepositoryMock.Object);
            _competitorRepositoryMock.Setup(o => o.GetAllPlayers()).Returns(Task.FromResult((IEnumerable<Player>)players));
            _mapperMock.Setup(o => o.Map<IEnumerable<PlayerDisplayDTO>>(It.IsAny<IEnumerable<Player>>()))
                .Returns(players.Adapt<IEnumerable<PlayerDisplayDTO>>());
            GetPagedPlayers command = new();
            GetPagedPlayersHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object, GetLogger<GetPagedPlayersHandler>());

            IEnumerable<PlayerDisplayDTO> response = await handler.Handle(command, default);

            _competitorRepositoryMock.Verify(o => o.GetAllPlayers(), Times.Once);
            Assert.Equal(players.Count, response.Count());
            for (int i = 0; i < players.Count; i++)
            {
                Player player = players[i];
                PlayerDisplayDTO playerDisplayDTO = response.ElementAt(i);

                Assert.Equal(player.Id, playerDisplayDTO.Id);
                Assert.Equal(player.Name, playerDisplayDTO.Name);
            }
        }
    }
}
