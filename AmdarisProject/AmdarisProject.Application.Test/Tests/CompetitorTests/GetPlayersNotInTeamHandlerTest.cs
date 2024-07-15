using AmdarisProject.Application.Dtos.ResponseDTOs.DisplayDTOs;
using AmdarisProject.Application.Handlers.CompetitorHandlers;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models.CompetitorModels;
using AmdarisProject.TestUtils.ModelBuilders;
using Moq;

namespace AmdarisProject.Application.Test.Tests.CompetitorTests
{
    public class GetPlayersNotInTeamHandlerTest : ApplicationTestBase
    {
        [Fact]
        public async Task Test_GetPlayersNotInTeamHandler_Success()
        {
            Team team = APBuilder.CreateBasicTeam().Get();
            List<Player> players = [];
            for (int i = 0; i < _numberOfModelsInAList; i++) players.Add(APBuilder.CreateBasicPlayer().Get());
            _competitorRepositoryMock.Setup(o => o.GetTeamById(It.IsAny<Guid>()))
                .Returns(Task.FromResult((Team?)team));
            _competitorRepositoryMock.Setup(o => o.GetPlayersNotInTeam(It.IsAny<Guid>()))
                .Returns(Task.FromResult((IEnumerable<Player>)players));
            _mapperMock.Setup(o => o.Map<IEnumerable<CompetitorDisplayDTO>>(It.IsAny<IEnumerable<Player>>()))
                .Returns(_mapper.Map<IEnumerable<CompetitorDisplayDTO>>(players));
            GetPlayersNotInTeam command = new(It.IsAny<Guid>());
            GetPlayersNotInTeamHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object,
                GetLogger<GetPlayersNotInTeamHandler>());

            IEnumerable<CompetitorDisplayDTO> response = await handler.Handle(command, default);

            _competitorRepositoryMock.Verify(o => o.GetPlayersNotInTeam(It.IsAny<Guid>()), Times.Once);
            Assert.Equal(players.Count, response.Count());
            for (int i = 0; i < players.Count; i++)
            {
                Player player = players[i];
                CompetitorDisplayDTO competitorDisplayDTO = response.ElementAt(i);

                Assert.Equal(player.Id, competitorDisplayDTO.Id);
                Assert.Equal(player.Name, competitorDisplayDTO.Name);
            }
        }

        [Fact]
        public async Task Test_GetPlayersNotInTeamHandler_TeamNotFound_throws_APNotFoundException()
        {
            _competitorRepositoryMock.Setup(o => o.GetTeamById(It.IsAny<Guid>())).Returns(Task.FromResult((Team?)null));
            GetPlayersNotInTeam command = new(It.IsAny<Guid>());
            GetPlayersNotInTeamHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object,
                GetLogger<GetPlayersNotInTeamHandler>());

            await Assert.ThrowsAsync<APNotFoundException>(async () => await handler.Handle(command, default));

            _competitorRepositoryMock.Verify(o => o.GetTeamById(It.IsAny<Guid>()), Times.Once);
        }
    }
}
