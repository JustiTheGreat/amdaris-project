using AmdarisProject.Application.Dtos.DisplayDTOs.CompetitorDisplayDTOs;
using AmdarisProject.Application.Handlers.CompetitorHandlers;
using AmdarisProject.Application.Test.ModelBuilders;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models.CompetitionModels;
using AmdarisProject.Domain.Models.CompetitorModels;
using Mapster;
using Moq;

namespace AmdarisProject.Application.Test.Tests.CompetitorTests
{
    public class GetPlayersNotInCompetitionHandlerTest : MockObjectUser
    {
        [Fact]
        public async Task Test_GetPlayersNotInCompetitionHandler_Success()
        {
            Competition competition = APBuilder.CreateBasicOneVSAllCompetition().Get();
            List<Player> players = [];
            for (int i = 0; i < _numberOfModelsInAList; i++) players.Add(APBuilder.CreateBasicPlayer().Get());
            _unitOfWorkMock.Setup(o => o.CompetitionRepository).Returns(_competitionRepositoryMock.Object);
            _unitOfWorkMock.Setup(o => o.CompetitorRepository).Returns(_competitorRepositoryMock.Object);
            _competitionRepositoryMock.Setup(o => o.GetById(It.IsAny<Guid>())).Returns(Task.FromResult((Competition?)competition));
            _competitorRepositoryMock.Setup(o => o.GetPlayersNotInCompetition(It.IsAny<Guid>()))
                .Returns(Task.FromResult((IEnumerable<Player>)players));
            _mapperMock.Setup(o => o.Map<IEnumerable<PlayerDisplayDTO>>(It.IsAny<IEnumerable<Player>>()))
                .Returns(players.Adapt<IEnumerable<PlayerDisplayDTO>>());
            GetPlayersNotInCompetition command = new(It.IsAny<Guid>());
            GetPlayersNotInCompetitionHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object,
                GetLogger<GetPlayersNotInCompetitionHandler>());

            IEnumerable<PlayerDisplayDTO> response = await handler.Handle(command, default);

            _competitorRepositoryMock.Verify(o => o.GetPlayersNotInCompetition(It.IsAny<Guid>()), Times.Once);
            for (int i = 0; i < _numberOfModelsInAList; i++)
            {
                Assert.Equal(players.ElementAt(i).Id, response.ElementAt(i).Id);
                Assert.Equal(players.ElementAt(i).Name, response.ElementAt(i).Name);
            }
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
