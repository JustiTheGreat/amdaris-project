using AmdarisProject.Application.Dtos.DisplayDTOs.CompetitorDisplayDTOs;
using AmdarisProject.Application.Handlers.CompetitorHandlers;
using AmdarisProject.Application.Test.ModelBuilder;
using AmdarisProject.Domain.Models.CompetitorModels;
using Mapster;
using Moq;

namespace AmdarisProject.Application.Test.Tests.CompetitorTests
{
    public class GetPlayersNotInTeamHandlerTest : MockObjectUser
    {
        [Fact]
        public async Task Test_GetPlayersNotInTeamHandler_Success()
        {
            List<Player> players = [];
            for (int i = 0; i < NumberOfModelsInAList; i++) players.Add(Builders.CreateBasicPlayer().Get());
            _unitOfWorkMock.Setup(o => o.CompetitorRepository).Returns(_competitorRepositoryMock.Object);
            _competitorRepositoryMock.Setup(o => o.GetPlayersNotInTeam(It.IsAny<Guid>()))
                .Returns(Task.FromResult((IEnumerable<Player>)players));
            _mapperMock.Setup(o => o.Map<IEnumerable<PlayerDisplayDTO>>(It.IsAny<IEnumerable<Player>>()))
                .Returns(players.Adapt<IEnumerable<PlayerDisplayDTO>>());
            GetPlayersNotInTeam command = new(It.IsAny<Guid>());
            GetPlayersNotInTeamHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object);

            IEnumerable<PlayerDisplayDTO> response = await handler.Handle(command, default);

            _competitorRepositoryMock.Verify(o => o.GetPlayersNotInTeam(It.IsAny<Guid>()), Times.Once);
            for (int i = 0; i < NumberOfModelsInAList; i++)
            {
                Assert.Equal(players.ElementAt(i).Id, response.ElementAt(i).Id);
                Assert.Equal(players.ElementAt(i).Name, response.ElementAt(i).Name);
            }
        }
    }
}
