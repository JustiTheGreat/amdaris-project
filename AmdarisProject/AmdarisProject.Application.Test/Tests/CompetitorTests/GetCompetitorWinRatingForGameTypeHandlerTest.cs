using AmdarisProject.Application.Handlers.CompetitorHandlers;
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
            GetCompetitorWinRatings command = new(player.Id, gameType.Id);
            GetCompetitorWinRatingsHandler handler = new(_unitOfWorkMock.Object,
                GetLogger<GetCompetitorWinRatingsHandler>());

            double response = await handler.Handle(command, default);

            _matchRepositoryMock.Verify(o => o.GetCompetitorWinRatingForGameType(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async Task Test_GetCompetitorWinRatingForGameTypeHandler_CompetitorNotFound_throws_APNotFoundException()
        {
            _competitorRepositoryMock.Setup(o => o.GetById(It.IsAny<Guid>())).Returns(Task.FromResult((Competitor?)null));
            GetCompetitorWinRatings command = new(It.IsAny<Guid>(), It.IsAny<Guid>());
            GetCompetitorWinRatingsHandler handler = new(_unitOfWorkMock.Object,
                GetLogger<GetCompetitorWinRatingsHandler>());

            await Assert.ThrowsAsync<APNotFoundException>(async () => await handler.Handle(command, default));

            _competitorRepositoryMock.Verify(o => o.GetById(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async Task Test_GetCompetitorWinRatingForGameTypeHandler_GameTypeNotFound_throws_APNotFoundException()
        {
            Player player = APBuilder.CreateBasicPlayer().Get();
            _competitorRepositoryMock.Setup(o => o.GetById(It.IsAny<Guid>())).Returns(Task.FromResult((Competitor?)player));
            _gameTypeRepositoryMock.Setup(o => o.GetById(It.IsAny<Guid>())).Returns(Task.FromResult((GameType?)null));
            GetCompetitorWinRatings command = new(player.Id, It.IsAny<Guid>());
            GetCompetitorWinRatingsHandler handler = new(_unitOfWorkMock.Object,
                GetLogger<GetCompetitorWinRatingsHandler>());

            await Assert.ThrowsAsync<APNotFoundException>(async () => await handler.Handle(command, default));

            _gameTypeRepositoryMock.Verify(o => o.GetById(It.IsAny<Guid>()), Times.Once);
        }
    }
}
