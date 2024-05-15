using AmdarisProject.Application.Common.Abstractions;
using AmdarisProject.Application.Dtos.ResponseDTOs;
using AmdarisProject.Application.Test.ModelBuilders;
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
            uint pointsToAdd = (uint)APBuilder.CreateBasicGameFormat().Get().WinAt! - 1;
            PointBuilder pointBuilder = APBuilder.CreateBasicPoint().SetValue(0)
                .SetMatch(APBuilder.CreateBasicMatch().SetStatus(MatchStatus.STARTED).Get());
            Point point = pointBuilder.Get();
            _unitOfWorkMock.Setup(o => o.MatchRepository).Returns(_matchRepositoryMock.Object);
            _unitOfWorkMock.Setup(o => o.PointRepository).Returns(_pointRepositoryMock.Object);
            _matchRepositoryMock.Setup(o => o.GetById(It.IsAny<Guid>())).Returns(Task.FromResult((Match?)point.Match));
            _pointRepositoryMock.Setup(o => o.GetByPlayerAndMatch(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .Returns(Task.FromResult((Point?)point));
            point = pointBuilder.Clone().SetValue(point.Value + pointsToAdd).Get();
            _pointRepositoryMock.Setup(o => o.Update(It.IsAny<Point>())).Returns(Task.FromResult(point));
            _matchRepositoryMock.Setup(o => o.Update(It.IsAny<Match>())).Returns(Task.FromResult(point.Match));
            _mapperMock.Setup(o => o.Map<PointGetDTO>(It.IsAny<Point>())).Returns(point.Adapt<PointGetDTO>());
            AddValueToPointValue command = new(point.Player.Id, point.Match.Id, pointsToAdd);
            AddValueToPointValueHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object, It.IsAny<IEndMatchService>(),
                GetLogger<AddValueToPointValueHandler>());

            PointGetDTO response = await handler.Handle(command, default);

            Assert.Equal(point.Id, response.Id);
            Assert.Equal(point.Value, response.Value);
            Assert.Equal(point.Match.Id, response.Match);
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
            APBuilder.CreateBasicMatch().SetStatus(MatchStatus.NOT_STARTED).Get(),
            APBuilder.CreateBasicMatch().SetStatus(MatchStatus.FINISHED).Get(),
            APBuilder.CreateBasicMatch().SetStatus(MatchStatus.SPECIAL_WIN_COMPETITOR_ONE).Get(),
            APBuilder.CreateBasicMatch().SetStatus(MatchStatus.SPECIAL_WIN_COMPETITOR_TWO).Get(),
            APBuilder.CreateBasicMatch().SetStatus(MatchStatus.CANCELED).Get(),
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
            APBuilder.CreateBasicMatch().SetStatus(MatchStatus.STARTED).SetCompetitorOnePoints(APBuilder.CreateBasicGameFormat().Get().WinAt).SetCompetitorTwoPoints(0).Get(),
            APBuilder.CreateBasicMatch().SetStatus(MatchStatus.STARTED).SetCompetitorOnePoints(0).SetCompetitorTwoPoints(APBuilder.CreateBasicGameFormat().Get().WinAt).Get(),
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
            Point point = APBuilder.CreateBasicPoint()
                .SetMatch(APBuilder.CreateBasicMatch().SetStatus(MatchStatus.STARTED).Get()).Get();
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
            uint pointsToAdd = (uint)APBuilder.CreateBasicGameFormat().Get().WinAt!;
            PointBuilder pointBuilder = APBuilder.CreateBasicPoint().SetValue(0)
                .SetMatch(APBuilder.CreateBasicMatch().SetStatus(MatchStatus.STARTED).Get());
            Point point = pointBuilder.Get();
            _unitOfWorkMock.Setup(o => o.MatchRepository).Returns(_matchRepositoryMock.Object);
            _unitOfWorkMock.Setup(o => o.PointRepository).Returns(_pointRepositoryMock.Object);
            _matchRepositoryMock.Setup(o => o.GetById(It.IsAny<Guid>())).Returns(Task.FromResult((Match?)point.Match));
            _pointRepositoryMock.Setup(o => o.GetByPlayerAndMatch(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .Returns(Task.FromResult((Point?)point));
            point = pointBuilder.Clone().SetValue(point.Value + pointsToAdd).SetMatch(match).Get();
            _pointRepositoryMock.Setup(o => o.Update(It.IsAny<Point>())).Returns(Task.FromResult(point));
            _matchRepositoryMock.Setup(o => o.Update(It.IsAny<Match>())).Returns(Task.FromResult(point.Match));
            _mapperMock.Setup(o => o.Map<PointGetDTO>(It.IsAny<Point>())).Returns(point.Adapt<PointGetDTO>());
            AddValueToPointValue command = new(point.Player.Id, point.Match.Id, pointsToAdd);
            AddValueToPointValueHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object, _endMatchServiceMock.Object,
                GetLogger<AddValueToPointValueHandler>());

            PointGetDTO response = await handler.Handle(command, default);

            _endMatchServiceMock.Verify(x => x.End(It.IsAny<Guid>(), It.IsAny<MatchStatus>()), Times.Once());
        }

        [Fact]
        public async Task Test_AddValueToPointValueHandler_RollbackIsCalled_throws_Exception()
        {
            uint pointsToAdd = (uint)APBuilder.CreateBasicGameFormat().Get().WinAt!;
            Point point = APBuilder.CreateBasicPoint().SetValue(0)
                .SetMatch(APBuilder.CreateBasicMatch().SetStatus(MatchStatus.STARTED).Get()).Get();
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
