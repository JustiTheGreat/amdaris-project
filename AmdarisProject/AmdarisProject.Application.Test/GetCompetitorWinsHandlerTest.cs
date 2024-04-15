using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Handlers.CompetitorHandlers;
using AmdarisProject.Application.Test.ModelBuilder;
using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Models;
using AmdarisProject.Domain.Models.CompetitorModels;
using Moq;
using Match = AmdarisProject.Domain.Models.Match;


namespace AmdarisProject.Application.Test
{
    public class GetCompetitorWinsHandlerTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly Mock<ICompetitorRepository> _competitorRepositoryMock = new();
        private readonly Mock<IMatchRepository> _matchRepositoryMock = new();
        private readonly Mock<IPointRepository> _pointRepositoryMock = new();

        [Fact]
        public async Task Test_GetCompetitorWinsHandler_Success()
        {
            Player player = Builders.CreateBasicPlayer().Get();
            ushort numberOfMatches = 5;
            List<Match> matches = [];
            List<Point> points = [];
            for (int i = 0; i < numberOfMatches; i++)
            {
                matches.Add(Builders.CreateBasicMatch().SetCompetitorOne(player).Get());
                points.Add(Builders.CreateBasicPoint().SetValue(5).Get());
                points.Add(Builders.CreateBasicPoint().SetValue(4).Get());
            }
            _unitOfWorkMock.Setup(o => o.CompetitorRepository).Returns(_competitorRepositoryMock.Object);
            _unitOfWorkMock.Setup(o => o.MatchRepository).Returns(_matchRepositoryMock.Object);
            _unitOfWorkMock.Setup(o => o.PointRepository).Returns(_pointRepositoryMock.Object);

            _matchRepositoryMock.Setup(o => o.GetAllByCompetitorAndGameType(It.IsAny<Guid>(), It.IsAny<GameType>()))
                .Returns(Task.FromResult((IEnumerable<Match>)matches));
            var v = _matchRepositoryMock.SetupSequence(o => o.GetById(It.IsAny<Guid>()));
            for (int i = 0; i < numberOfMatches; i++)
                v.Returns(Task.FromResult((Match?)matches[i])).
                    Returns(Task.FromResult((Match?)matches[i]))
                    .Returns(Task.FromResult((Match?)matches[i]));

            var v2 = _competitorRepositoryMock.SetupSequence(o => o.GetById(It.IsAny<Guid>()));
            for (int i = 0; i < numberOfMatches; i++)
                v2.Returns(Task.FromResult((Competitor?)matches[i].CompetitorOne))
                    .Returns(Task.FromResult((Competitor?)matches[i].CompetitorTwo));

            var v3 = _pointRepositoryMock.SetupSequence(o => o.GetByPlayerAndMatch(It.IsAny<Guid>(), It.IsAny<Guid>()));
            for (int i = 0; i < numberOfMatches; i++)
                v3.Returns(Task.FromResult((Point?)points[i * 2])).Returns(Task.FromResult((Point?)points[i * 2 + 1]));

            GetCompetitorWins command = new(player.Id, It.IsAny<GameType>());
            GetCompetitorWinsHandler handler = new(_unitOfWorkMock.Object);

            uint response = await handler.Handle(command, default);

            Assert.True(response == 5);
            _matchRepositoryMock.Verify(o => o.GetAllByCompetitorAndGameType(It.IsAny<Guid>(), It.IsAny<GameType>()), Times.Once);
            _matchRepositoryMock.Verify(o => o.GetById(It.IsAny<Guid>()), Times.Exactly(3 * numberOfMatches));
            _competitorRepositoryMock.Verify(o => o.GetById(It.IsAny<Guid>()), Times.Exactly(2 * numberOfMatches));
            _pointRepositoryMock.Verify(o => o.GetByPlayerAndMatch(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Exactly(2 * numberOfMatches));
        }

        [Fact]
        public async Task Test_GetCompetitorWinsHandler_MatchNotFound_Throws_AggregateException()
        {
            ushort numberOfMatches = 5;
            List<Match> matches = [];
            for (int i = 0; i < numberOfMatches; i++)
                matches.Add(Builders.CreateBasicMatch().Get());
            _unitOfWorkMock.Setup(o => o.MatchRepository).Returns(_matchRepositoryMock.Object);

            _matchRepositoryMock.Setup(o => o.GetAllByCompetitorAndGameType(It.IsAny<Guid>(), It.IsAny<GameType>()))
                .Returns(Task.FromResult((IEnumerable<Match>)matches));
            _matchRepositoryMock.Setup(o => o.GetById(It.IsAny<Guid>())).Returns(Task.FromResult((Match?)null));

            GetCompetitorWins command = new(It.IsAny<Guid>(), It.IsAny<GameType>());
            GetCompetitorWinsHandler handler = new(_unitOfWorkMock.Object);

            await Assert.ThrowsAsync<AggregateException>(async () => await handler.Handle(command, default));

            _matchRepositoryMock.Verify(o => o.GetAllByCompetitorAndGameType(It.IsAny<Guid>(), It.IsAny<GameType>()), Times.Once);
            _matchRepositoryMock.Verify(o => o.GetById(It.IsAny<Guid>()), Times.Once);
        }
    }
}
