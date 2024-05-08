using AmdarisProject.Application.Dtos.ResponseDTOs;
using AmdarisProject.Application.Services;
using AmdarisProject.Application.Test.ModelBuilder;
using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models;
using AmdarisProject.handlers.point;
using Mapster;
using MapsterMapper;
using Moq;
using Match = AmdarisProject.Domain.Models.Match;

namespace AmdarisProject.Application.Test.Tests.PointHandlers
{
    public class AddValueToPointValueHandlerTest : MockObjectUser
    {
        private readonly Mock<IEndMatchService> _endMatchServiceMock = new();

        [Fact]
        public async Task Test_AddValueToPointValueHandler_Success()
        {
            uint pointsToAdd = (uint)Builders.CreateBasicGameFormat().Get().WinAt! - 1;
            PointBuilder builder = Builders.CreateBasicPoint();
            Point point = builder.Get();
            Point aux = builder.Clone().SetValue(point.Value + pointsToAdd).Get();
            point.Match.Status = MatchStatus.STARTED;
            _unitOfWorkMock.Setup(o => o.MatchRepository).Returns(_matchRepositoryMock.Object);
            _unitOfWorkMock.Setup(o => o.PointRepository).Returns(_pointRepositoryMock.Object);
            _matchRepositoryMock.Setup(o => o.GetById(It.IsAny<Guid>())).Returns(Task.FromResult((Match?)point.Match));
            _pointRepositoryMock.Setup(o => o.GetByPlayerAndMatch(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .Returns(Task.FromResult((Point?)point));
            _pointRepositoryMock.Setup(o => o.Update(It.IsAny<Point>())).Returns(Task.FromResult(aux));
            _matchRepositoryMock.Setup(o => o.Update(It.IsAny<Match>())).Returns(Task.FromResult(aux.Match));
            _mapperMock.Setup(o => o.Map<PointGetDTO>(It.IsAny<Point>())).Returns(aux.Adapt<PointGetDTO>());
            AddValueToPointValue command = new(point.Player.Id, point.Match.Id, pointsToAdd);
            AddValueToPointValueHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object, It.IsAny<IEndMatchService>(),
                GetLogger<AddValueToPointValueHandler>());

            PointGetDTO response = await handler.Handle(command, default);

            Assert.Equal(point.Id, response.Id);
            Assert.Equal(aux.Value, response.Value);
            Assert.True(point.Match.Id.Equals(response.Match));
            Assert.Equal(point.Player.Id, response.Player.Id);
            Assert.Equal(point.Player.Name, response.Player.Name);
        }

        [Fact]
        public async Task Test_AddValueToPointValueHandler_MatchNotFound_throws_APNotFoundException()
        {
            _unitOfWorkMock.Setup(o => o.MatchRepository).Returns(_matchRepositoryMock.Object);
            _matchRepositoryMock.Setup(o => o.GetById(It.IsAny<Guid>())).Returns(Task.FromResult((Match?)null));
            AddValueToPointValue command = new(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<uint>());
            AddValueToPointValueHandler handler = new(_unitOfWorkMock.Object, It.IsAny<IMapper>(), It.IsAny<IEndMatchService>(),
                GetLogger<AddValueToPointValueHandler>());

            await Assert.ThrowsAsync<APNotFoundException>(async () => await handler.Handle(command, default));
        }

        public static TheoryData<Match> Status => new()
        {
            Builders.CreateBasicMatch().SetStatus(MatchStatus.NOT_STARTED).Get(),
            Builders.CreateBasicMatch().SetStatus(MatchStatus.FINISHED).Get(),
            Builders.CreateBasicMatch().SetStatus(MatchStatus.SPECIAL_WIN_COMPETITOR_ONE).Get(),
            Builders.CreateBasicMatch().SetStatus(MatchStatus.SPECIAL_WIN_COMPETITOR_TWO).Get(),
            Builders.CreateBasicMatch().SetStatus(MatchStatus.CANCELED).Get(),
        };

        [Theory]
        [MemberData(nameof(Status))]
        public async Task Test_AddValueToPointValueHandler_IllegalMatchStatus_throws_APIllegalStatusException(Match match)
        {
            _unitOfWorkMock.Setup(o => o.MatchRepository).Returns(_matchRepositoryMock.Object);
            _matchRepositoryMock.Setup(o => o.GetById(It.IsAny<Guid>())).Returns(Task.FromResult((Match?)match));
            AddValueToPointValue command = new(It.IsAny<Guid>(), match.Id, It.IsAny<uint>());
            AddValueToPointValueHandler handler = new(_unitOfWorkMock.Object, It.IsAny<IMapper>(), It.IsAny<IEndMatchService>(),
                GetLogger<AddValueToPointValueHandler>());

            await Assert.ThrowsAsync<APIllegalStatusException>(async () => await handler.Handle(command, default));
        }

        public static TheoryData<Match> WinningScores => new()
        {
            Builders.CreateBasicMatch().SetStatus(MatchStatus.STARTED).SetCompetitorOnePoints(Builders.CreateBasicGameFormat().Get().WinAt).SetCompetitorTwoPoints(0).Get(),
            Builders.CreateBasicMatch().SetStatus(MatchStatus.STARTED).SetCompetitorOnePoints(0).SetCompetitorTwoPoints(Builders.CreateBasicGameFormat().Get().WinAt).Get(),
        };

        [Theory]
        [MemberData(nameof(WinningScores))]
        public async Task Test_AddValueToPointValueHandler_MatchHasACompetitorWithTheWinningScore_throws_AmdarisProjectException(Match match)
        {
            _unitOfWorkMock.Setup(o => o.MatchRepository).Returns(_matchRepositoryMock.Object);
            _unitOfWorkMock.Setup(o => o.PointRepository).Returns(_pointRepositoryMock.Object);
            _matchRepositoryMock.Setup(o => o.GetById(It.IsAny<Guid>())).Returns(Task.FromResult((Match?)match));
            AddValueToPointValue command = new(It.IsAny<Guid>(), match.Id, It.IsAny<uint>());
            AddValueToPointValueHandler handler = new(_unitOfWorkMock.Object, It.IsAny<IMapper>(), It.IsAny<IEndMatchService>(),
                GetLogger<AddValueToPointValueHandler>());

            await Assert.ThrowsAsync<AmdarisProjectException>(async () => await handler.Handle(command, default));
        }

        [Fact]
        public async Task Test_AddValueToPointValueHandler_PointNotFound_throws_APNotFoundException()
        {
            Point point = Builders.CreateBasicPoint().Get();
            point.Match.Status = MatchStatus.STARTED;
            _unitOfWorkMock.Setup(o => o.MatchRepository).Returns(_matchRepositoryMock.Object);
            _unitOfWorkMock.Setup(o => o.PointRepository).Returns(_pointRepositoryMock.Object);
            _matchRepositoryMock.Setup(o => o.GetById(It.IsAny<Guid>())).Returns(Task.FromResult((Match?)point.Match));
            _pointRepositoryMock.Setup(o => o.GetByPlayerAndMatch(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(Task.FromResult((Point?)null));
            AddValueToPointValue command = new(point.Player.Id, point.Match.Id, It.IsAny<uint>());
            AddValueToPointValueHandler handler = new(_unitOfWorkMock.Object, It.IsAny<IMapper>(), It.IsAny<IEndMatchService>(),
                GetLogger<AddValueToPointValueHandler>());

            await Assert.ThrowsAsync<APNotFoundException>(async () => await handler.Handle(command, default));
        }

        [Theory]
        [MemberData(nameof(WinningScores))]
        public async Task Test_AddValueToPointValueHandler_EndMatchIsCalled(Match match)
        {
            uint pointsToAdd = (uint)Builders.CreateBasicGameFormat().Get().WinAt!;
            PointBuilder builder = Builders.CreateBasicPoint();
            Point point = builder.Get();
            Point aux = builder.Clone().SetValue(point.Value + pointsToAdd).SetMatch(match).Get();
            point.Match.Status = MatchStatus.STARTED;
            _unitOfWorkMock.Setup(o => o.MatchRepository).Returns(_matchRepositoryMock.Object);
            _unitOfWorkMock.Setup(o => o.PointRepository).Returns(_pointRepositoryMock.Object);
            _matchRepositoryMock.Setup(o => o.GetById(It.IsAny<Guid>())).Returns(Task.FromResult((Match?)point.Match));
            _pointRepositoryMock.Setup(o => o.GetByPlayerAndMatch(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .Returns(Task.FromResult((Point?)point));
            _pointRepositoryMock.Setup(o => o.Update(It.IsAny<Point>())).Returns(Task.FromResult(aux));
            _matchRepositoryMock.Setup(o => o.Update(It.IsAny<Match>())).Returns(Task.FromResult(aux.Match));
            _mapperMock.Setup(o => o.Map<PointGetDTO>(It.IsAny<Point>())).Returns(aux.Adapt<PointGetDTO>());
            AddValueToPointValue command = new(point.Player.Id, point.Match.Id, pointsToAdd);
            AddValueToPointValueHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object, _endMatchServiceMock.Object,
                GetLogger<AddValueToPointValueHandler>());

            PointGetDTO response = await handler.Handle(command, default);

            _endMatchServiceMock.Verify(x => x.End(It.IsAny<Guid>(), It.IsAny<MatchStatus>()), Times.Once());
        }

        [Fact]
        public async Task Test_AddValueToPointValueHandler_RollbackIsCalled_throws_Exception()
        {
            uint pointsToAdd = (uint)Builders.CreateBasicGameFormat().Get().WinAt!;
            PointBuilder builder = Builders.CreateBasicPoint();
            Point point = builder.Get();
            Point aux = builder.Clone().SetValue(point.Value + pointsToAdd).Get();
            point.Match.Status = MatchStatus.STARTED;
            _unitOfWorkMock.Setup(o => o.MatchRepository).Returns(_matchRepositoryMock.Object);
            _unitOfWorkMock.Setup(o => o.PointRepository).Returns(_pointRepositoryMock.Object);
            _matchRepositoryMock.Setup(o => o.GetById(It.IsAny<Guid>())).Returns(Task.FromResult((Match?)point.Match));
            _pointRepositoryMock.Setup(o => o.GetByPlayerAndMatch(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .Returns(Task.FromResult((Point?)point));
            _pointRepositoryMock.Setup(o => o.Update(It.IsAny<Point>())).Throws<Exception>();
            AddValueToPointValue command = new(point.Player.Id, point.Match.Id, pointsToAdd);
            AddValueToPointValueHandler handler = new(_unitOfWorkMock.Object, It.IsAny<IMapper>(), It.IsAny<IEndMatchService>(),
                GetLogger<AddValueToPointValueHandler>());

            await Assert.ThrowsAsync<Exception>(async () => await handler.Handle(command, default));

            _unitOfWorkMock.Verify(x => x.RollbackTransactionAsync(), Times.Once());
        }
    }
}
