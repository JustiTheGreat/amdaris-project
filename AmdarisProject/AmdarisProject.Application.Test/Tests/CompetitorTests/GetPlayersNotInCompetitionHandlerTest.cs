using AmdarisProject.Application.Dtos.DisplayDTOs.CompetitorDisplayDTOs;
using AmdarisProject.Application.Handlers.CompetitorHandlers;
using AmdarisProject.Application.Test.ModelBuilders;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models.CompetitionModels;
using AmdarisProject.Domain.Models.CompetitorModels;
using Moq;

namespace AmdarisProject.Application.Test.Tests.CompetitorTests
{
    public class GetPlayersNotInCompetitionHandlerTest : ApplicationTestBase
    {
        [Fact]
        public async Task Test_GetPlayersNotInCompetitionHandler_Success()
        {
            //Competition competition = APBuilder.CreateBasicOneVSAllCompetition().Get();
            //List<Player> players = [];
            //for (int i = 0; i < _numberOfModelsInAList; i++) players.Add(APBuilder.CreateBasicPlayer().Get());
            //_unitOfWorkMock.Setup(o => o.CompetitionRepository).Returns(_competitionRepositoryMock.Object);
            //_unitOfWorkMock.Setup(o => o.CompetitorRepository).Returns(_competitorRepositoryMock.Object);
            //_competitionRepositoryMock.Setup(o => o.GetById(It.IsAny<Guid>())).Returns(Task.FromResult((Competition?)competition));
            //_competitorRepositoryMock.Setup(o => o.GetPlayersNotInCompetition(It.IsAny<Guid>()))
            //    .Returns(Task.FromResult((IEnumerable<Player>)players));
            //_mapperMock.Setup(o => o.Map<IEnumerable<PlayerDisplayDTO>>(It.IsAny<IEnumerable<Player>>()))
            //    .Returns(players.Adapt<IEnumerable<PlayerDisplayDTO>>());
            //GetPlayersNotInCompetition command = new(It.IsAny<Guid>());
            //GetPlayersNotInCompetitionHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object,
            //    GetLogger<GetPlayersNotInCompetitionHandler>());

            //IEnumerable<PlayerDisplayDTO> response = await handler.Handle(command, default);

            //_competitorRepositoryMock.Verify(o => o.GetPlayersNotInCompetition(It.IsAny<Guid>()), Times.Once);
            //Assert.Equal(players.Count, response.Count());
            //for (int i = 0; i < players.Count; i++)
            //{
            //    Player player = players[i];
            //    PlayerDisplayDTO playerDisplayDTO = response.ElementAt(i);

            //    Assert.Equal(player.Id, playerDisplayDTO.Id);
            //    Assert.Equal(player.Name, playerDisplayDTO.Name);
            //}
        }

        [Fact]
        public async Task Test_GetPlayersNotInCompetitionHandler_CompetitionNotFound_throws_APNotFoundException()
        {
            _unitOfWorkMock.Setup(o => o.CompetitionRepository).Returns(_competitionRepositoryMock.Object);
            _competitionRepositoryMock.Setup(o => o.GetById(It.IsAny<Guid>())).Returns(Task.FromResult((Competition?)null));
            GetPlayersNotInCompetition command = new(It.IsAny<Guid>());
            GetPlayersNotInCompetitionHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object,
                GetLogger<GetPlayersNotInCompetitionHandler>());

            await Assert.ThrowsAsync<APNotFoundException>(async () => await handler.Handle(command, default));

            _competitionRepositoryMock.Verify(o => o.GetById(It.IsAny<Guid>()), Times.Once);
        }
    }
}
