using AmdarisProject.Application.Handlers.CompetitorHandlers;
using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models;
using AmdarisProject.Domain.Models.CompetitorModels;
using AmdarisProject.TestUtils.ModelBuilders;
using Moq;

namespace AmdarisProject.Application.Test.Tests.CompetitorTests
{
    public class GetCompetitorWinRatingForGameTypeHandlerTest : ApplicationTestBase
    {
        [Fact]
        public async Task Test_GetCompetitorWinRatingForGameTypeHandler_Success()
        {
            Player player = APBuilder.CreateBasicPlayer().Get();
            GameType gameType = APBuilder.CreateBasicGameType().Get();
            _competitorRepositoryMock.Setup(o => o.GetById(It.IsAny<Guid>())).Returns(Task.FromResult((Competitor?)player));
            _gameTypeRepositoryMock.Setup(o => o.GetById(It.IsAny<Guid>())).Returns(Task.FromResult((GameType?)gameType));
            _matchRepositoryMock.Setup(o => o.GetCompetitorWinRatingForGameType(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .Returns(Task.FromResult(0.5));
            GetCompetitorWinRatingForGameType command = new(player.Id, gameType.Id);
            GetCompetitorWinRatingForGameTypeHandler handler = new(_unitOfWorkMock.Object,
                GetLogger<GetCompetitorWinRatingForGameTypeHandler>());

            double response = await handler.Handle(command, default);

            _matchRepositoryMock.Verify(o => o.GetCompetitorWinRatingForGameType(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async Task Test_GetCompetitorWinRatingForGameTypeHandler_CompetitorNotFound_throws_APNotFoundException()
        {
            _competitorRepositoryMock.Setup(o => o.GetById(It.IsAny<Guid>())).Returns(Task.FromResult((Competitor?)null));
            GetCompetitorWinRatingForGameType command = new(It.IsAny<Guid>(), It.IsAny<Guid>());
            GetCompetitorWinRatingForGameTypeHandler handler = new(_unitOfWorkMock.Object,
                GetLogger<GetCompetitorWinRatingForGameTypeHandler>());

            await Assert.ThrowsAsync<APNotFoundException>(async () => await handler.Handle(command, default));

            _competitorRepositoryMock.Verify(o => o.GetById(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async Task Test_GetCompetitorWinRatingForGameTypeHandler_GameTypeNotFound_throws_APNotFoundException()
        {
            Player player = APBuilder.CreateBasicPlayer().Get();
            _competitorRepositoryMock.Setup(o => o.GetById(It.IsAny<Guid>())).Returns(Task.FromResult((Competitor?)player));
            _gameTypeRepositoryMock.Setup(o => o.GetById(It.IsAny<Guid>())).Returns(Task.FromResult((GameType?)null));
            GetCompetitorWinRatingForGameType command = new(player.Id, It.IsAny<Guid>());
            GetCompetitorWinRatingForGameTypeHandler handler = new(_unitOfWorkMock.Object,
                GetLogger<GetCompetitorWinRatingForGameTypeHandler>());

            await Assert.ThrowsAsync<APNotFoundException>(async () => await handler.Handle(command, default));

            _gameTypeRepositoryMock.Verify(o => o.GetById(It.IsAny<Guid>()), Times.Once);
        }
    }
}
