namespace AmdarisProject.Application.Test.Tests.CompetitorTests
{
    public class GetCompetitorMatchPointsHandlerTest
    {
        //private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        //private readonly Mock<ICompetitorRepository> _competitorRepositoryMock = new();
        //private readonly Mock<IMatchRepository> _matchRepositoryMock = new();
        //private readonly Mock<IPointRepository> _pointRepositoryMock = new();

        //[Fact]
        //public async Task Test_GetCompetitorMatchPointsHandler_Success()
        //{
        //    Player player = Builders.CreateBasicPlayer().Get();
        //    Match match = Builders.CreateBasicMatch().SetCompetitorOne(player).Get();
        //    Point point = Builders.CreateBasicPoint().SetValue(5).SetMatch(match).SetPlayer(player).Get();
        //    _unitOfWorkMock.Setup(o => o.CompetitorRepository).Returns(_competitorRepositoryMock.Object);
        //    _unitOfWorkMock.Setup(o => o.MatchRepository).Returns(_matchRepositoryMock.Object);
        //    _unitOfWorkMock.Setup(o => o.PointRepository).Returns(_pointRepositoryMock.Object);

        //    _matchRepositoryMock.Setup(o => o.GetById(It.IsAny<Guid>())).Returns(Task.FromResult((Match?)match));
        //    _competitorRepositoryMock.Setup(o => o.GetById(It.IsAny<Guid>())).Returns(Task.FromResult((Competitor?)player));
        //    _pointRepositoryMock.Setup(o => o.GetByPlayerAndMatch(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(Task.FromResult((Point?)point));

        //    GetCompetitorMatchPoints command = new(player.Id, match.Id);
        //    GetCompetitorMatchPointsHandler handler = new(_unitOfWorkMock.Object);

        //    uint response = await handler.Handle(command, default);

        //    Assert.True(response == 5);
        //    _matchRepositoryMock.Verify(o => o.GetById(It.IsAny<Guid>()), Times.Once);
        //    _competitorRepositoryMock.Verify(o => o.GetById(It.IsAny<Guid>()), Times.Once);
        //    _pointRepositoryMock.Verify(o => o.GetByPlayerAndMatch(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Once);
        //}

        //[Fact]
        //public async Task Test_GetCompetitorMatchPointsHandler_MatchNotFound_Throws_APNotFoundException()
        //{
        //    Match match = Builders.CreateBasicMatch().Get();
        //    _unitOfWorkMock.Setup(o => o.MatchRepository).Returns(_matchRepositoryMock.Object);
        //    _matchRepositoryMock.Setup(o => o.GetById(It.IsAny<Guid>())).Returns(Task.FromResult((Match?)null));
        //    GetCompetitorMatchPoints command = new(It.IsAny<Guid>(), match.Id);
        //    GetCompetitorMatchPointsHandler handler = new(_unitOfWorkMock.Object);

        //    await Assert.ThrowsAsync<APNotFoundException>(async () => await handler.Handle(command, default));
        //    _matchRepositoryMock.Verify(o => o.GetById(It.IsAny<Guid>()), Times.Once);
        //}

        //public static TheoryData<Match> IllegalStatusMatches = new()
        //{
        //    Builders.CreateBasicMatch().SetStatus(MatchStatus.NOT_STARTED).Get(),
        //    Builders.CreateBasicMatch().SetStatus(MatchStatus.CANCELED).Get()
        //};

        //[Theory]
        //[MemberData(nameof(IllegalStatusMatches))]
        //public async Task Test_GetCompetitorMatchPointsHandler_IllegalStatusMatch_Throws_APIllegalStatusException(Match match)
        //{
        //    _unitOfWorkMock.Setup(o => o.MatchRepository).Returns(_matchRepositoryMock.Object);
        //    _matchRepositoryMock.Setup(o => o.GetById(It.IsAny<Guid>())).Returns(Task.FromResult((Match?)match));
        //    GetCompetitorMatchPoints command = new(It.IsAny<Guid>(), match.Id);
        //    GetCompetitorMatchPointsHandler handler = new(_unitOfWorkMock.Object);

        //    await Assert.ThrowsAsync<APIllegalStatusException>(async () => await handler.Handle(command, default));
        //    _matchRepositoryMock.Verify(o => o.GetById(It.IsAny<Guid>()), Times.Once);
        //}

        //[Fact]
        //public async Task Test_GetCompetitorMatchPointsHandler_CompetitorNotInMatch_Throws_AmdarisProjectException()
        //{
        //    Player player = Builders.CreateBasicPlayer().Get();
        //    Match match = Builders.CreateBasicMatch().Get();
        //    _unitOfWorkMock.Setup(o => o.MatchRepository).Returns(_matchRepositoryMock.Object);
        //    _matchRepositoryMock.Setup(o => o.GetById(It.IsAny<Guid>())).Returns(Task.FromResult((Match?)match));
        //    GetCompetitorMatchPoints command = new(player.Id, match.Id);
        //    GetCompetitorMatchPointsHandler handler = new(_unitOfWorkMock.Object);

        //    await Assert.ThrowsAsync<AmdarisProjectException>(async () => await handler.Handle(command, default));
        //    _matchRepositoryMock.Verify(o => o.GetById(It.IsAny<Guid>()), Times.Once);
        //}

        //[Fact]
        //public async Task Test_GetCompetitorMatchPointsHandler_CompetitorNotFound_Throws_APNotFoundException()
        //{
        //    Player player = Builders.CreateBasicPlayer().Get();
        //    Match match = Builders.CreateBasicMatch().SetCompetitorOne(player).Get();
        //    _unitOfWorkMock.Setup(o => o.CompetitorRepository).Returns(_competitorRepositoryMock.Object);
        //    _unitOfWorkMock.Setup(o => o.MatchRepository).Returns(_matchRepositoryMock.Object);
        //    _unitOfWorkMock.Setup(o => o.PointRepository).Returns(_pointRepositoryMock.Object);

        //    _matchRepositoryMock.Setup(o => o.GetById(It.IsAny<Guid>())).Returns(Task.FromResult((Match?)match));
        //    _competitorRepositoryMock.Setup(o => o.GetById(It.IsAny<Guid>())).Returns(Task.FromResult((Competitor?)null));

        //    GetCompetitorMatchPoints command = new(player.Id, match.Id);
        //    GetCompetitorMatchPointsHandler handler = new(_unitOfWorkMock.Object);

        //    await Assert.ThrowsAsync<APNotFoundException>(async () => await handler.Handle(command, default));
        //    _matchRepositoryMock.Verify(o => o.GetById(It.IsAny<Guid>()), Times.Once);
        //    _competitorRepositoryMock.Verify(o => o.GetById(It.IsAny<Guid>()), Times.Once);
        //}

        //[Fact]
        //public async Task Test_GetCompetitorMatchPointsHandler_PointNotFound_Throws_APNotFoundException()
        //{
        //    Player player = Builders.CreateBasicPlayer().Get();
        //    Match match = Builders.CreateBasicMatch().SetCompetitorOne(player).Get();
        //    _unitOfWorkMock.Setup(o => o.CompetitorRepository).Returns(_competitorRepositoryMock.Object);
        //    _unitOfWorkMock.Setup(o => o.MatchRepository).Returns(_matchRepositoryMock.Object);
        //    _unitOfWorkMock.Setup(o => o.PointRepository).Returns(_pointRepositoryMock.Object);

        //    _matchRepositoryMock.Setup(o => o.GetById(It.IsAny<Guid>())).Returns(Task.FromResult((Match?)match));
        //    _competitorRepositoryMock.Setup(o => o.GetById(It.IsAny<Guid>())).Returns(Task.FromResult((Competitor?)player));
        //    _pointRepositoryMock.Setup(o => o.GetByPlayerAndMatch(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(Task.FromResult((Point?)null));

        //    GetCompetitorMatchPoints command = new(player.Id, match.Id);
        //    GetCompetitorMatchPointsHandler handler = new(_unitOfWorkMock.Object);

        //    await Assert.ThrowsAsync<APNotFoundException>(async () => await handler.Handle(command, default));
        //    _matchRepositoryMock.Verify(o => o.GetById(It.IsAny<Guid>()), Times.Once);
        //    _competitorRepositoryMock.Verify(o => o.GetById(It.IsAny<Guid>()), Times.Once);
        //    _pointRepositoryMock.Verify(o => o.GetByPlayerAndMatch(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Once);
        //}
    }
}
