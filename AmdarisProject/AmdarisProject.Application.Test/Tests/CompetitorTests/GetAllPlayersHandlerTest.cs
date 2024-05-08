using AmdarisProject.Application.Dtos.DisplayDTOs.CompetitorDisplayDTOs;
using AmdarisProject.Application.Handlers.CompetitorHandlers;
using AmdarisProject.Application.Handlers.GameFormatHandlers;
using AmdarisProject.Application.Test.ModelBuilder;
using AmdarisProject.Domain.Models.CompetitorModels;
using Mapster;
using Microsoft.Extensions.Logging;
using Moq;

namespace AmdarisProject.Application.Test.Tests.CompetitorTests
{
    public class GetAllPlayersHandlerTest : MockObjectUser
    {
        [Fact]
        public async Task Test_GetAllTeamsHandler_Success()
        {
            List<Player> players = [];
            for (int i = 0; i < NumberOfModelsInAList; i++) players.Add(Builders.CreateBasicPlayer().Get());
            _unitOfWorkMock.Setup(o => o.CompetitorRepository).Returns(_competitorRepositoryMock.Object);
            _competitorRepositoryMock.Setup(o => o.GetAllPlayers()).Returns(Task.FromResult((IEnumerable<Player>)players));
            _mapperMock.Setup(o => o.Map<IEnumerable<PlayerDisplayDTO>>(It.IsAny<IEnumerable<Player>>()))
                .Returns(players.Adapt<IEnumerable<PlayerDisplayDTO>>());
            GetAllPlayers command = new();
            GetAllPlayersHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object, 
                It.IsAny<ILogger<GetAllPlayersHandler>>());

            IEnumerable<PlayerDisplayDTO> response = await handler.Handle(command, default);

            _competitorRepositoryMock.Verify(o => o.GetAllPlayers(), Times.Once);
            for (int i = 0; i < NumberOfModelsInAList; i++)
            {
                Assert.Equal(players.ElementAt(i).Id, response.ElementAt(i).Id);
                Assert.Equal(players.ElementAt(i).Name, response.ElementAt(i).Name);
            }
        }
    }
}
