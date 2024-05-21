using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Dtos.ResponseDTOs;
using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models;
using AmdarisProject.handlers.point;
using AmdarisProject.TestUtils.ModelBuilders;
using AutoMapper;
using Moq;
using Match = AmdarisProject.Domain.Models.Match;

namespace AmdarisProject.Application.Test.Tests.PointHandlers
{
    public class AddValueToPointValueHandlerTest : ApplicationTestBase
    {
        private readonly Mock<IEndMatchService> _endMatchServiceMock = new();

        [Fact]
        public async Task Test_AddValueToPointValueHandler_Success()
        {
            uint pointsToAdd = (uint)APBuilder.CreateBasicGameFormat().Get().WinAt! - 1;
            PointBuilder pointBuilder = APBuilder.CreateBasicPoint().SetValue(0)
                .SetMatch(APBuilder.CreateBasicMatch().SetStatus(MatchStatus.STARTED).Get());
            Point point = pointBuilder.Get();
            _pointRepositoryMock.Setup(o => o.GetByMatchAndPlayer(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .Returns(Task.FromResult((Point?)point));
            point = pointBuilder.Clone().SetValue(point.Value + pointsToAdd).Get();
            _pointRepositoryMock.Setup(o => o.Update(It.IsAny<Point>())).Returns(Task.FromResult(point));
            _mapperMock.Setup(o => o.Map<PointGetDTO>(It.IsAny<Point>())).Returns(_mapper.Map<PointGetDTO>(point));
            AddValueToPointValue command = new(point.Match.Id, point.Player.Id, pointsToAdd);
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
        public async Task Test_AddValueToPointValueHandler_PointNotFound_throws_APNotFoundException()
        {
            _pointRepositoryMock.Setup(o => o.GetByMatchAndPlayer(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(Task.FromResult((Point?)null));
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
            Point point = APBuilder.CreateBasicPoint().SetMatch(match).Get();
            _pointRepositoryMock.Setup(o => o.GetByMatchAndPlayer(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .Returns(Task.FromResult((Point?)point));
            AddValueToPointValue command = new(match.Id, It.IsAny<Guid>(), It.IsAny<uint>());
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
            Point point = APBuilder.CreateBasicPoint().SetMatch(match).Get();
            _pointRepositoryMock.Setup(o => o.GetByMatchAndPlayer(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .Returns(Task.FromResult((Point?)point));
            AddValueToPointValue command = new(match.Id, It.IsAny<Guid>(), It.IsAny<uint>());
            AddValueToPointValueHandler handler = new(_unitOfWorkMock.Object, It.IsAny<IMapper>(), It.IsAny<IEndMatchService>(),
                GetLogger<AddValueToPointValueHandler>());

            await Assert.ThrowsAsync<AmdarisProjectException>(async () => await handler.Handle(command, default));
        }

        [Fact]
        public async Task Test_AddValueToPointValueHandler_EndMatchIsCalled()
        {
            uint pointsToAdd = (uint)APBuilder.CreateBasicGameFormat().Get().WinAt!;
            MatchBuilder matchBuilder = APBuilder.CreateBasicMatch().SetStatus(MatchStatus.STARTED).InitializePoints();
            PointBuilder pointBuilder = APBuilder.CreateBasicPoint().SetMatch(matchBuilder.Get());
            Point point = pointBuilder.Get();
            _pointRepositoryMock.Setup(o => o.GetByMatchAndPlayer(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .Returns(Task.FromResult((Point?)point));
            point = pointBuilder.Clone().SetMatch(matchBuilder.Clone().Get()).AddValue(pointsToAdd).Get();
            _pointRepositoryMock.Setup(o => o.Update(It.IsAny<Point>())).Returns(Task.FromResult(point));
            _mapperMock.Setup(o => o.Map<PointGetDTO>(It.IsAny<Point>())).Returns(_mapper.Map<PointGetDTO>(point));
            AddValueToPointValue command = new(point.Match.Id, point.Player.Id, pointsToAdd);
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
            _pointRepositoryMock.Setup(o => o.GetByMatchAndPlayer(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .Returns(Task.FromResult((Point?)point));
            _pointRepositoryMock.Setup(o => o.Update(It.IsAny<Point>())).Throws<Exception>();
            AddValueToPointValue command = new(point.Match.Id, point.Player.Id, pointsToAdd);
            AddValueToPointValueHandler handler = new(_unitOfWorkMock.Object, It.IsAny<IMapper>(), It.IsAny<IEndMatchService>(),
                GetLogger<AddValueToPointValueHandler>());

            await Assert.ThrowsAsync<Exception>(async () => await handler.Handle(command, default));

            _unitOfWorkMock.Verify(x => x.RollbackTransactionAsync(), Times.Once());
        }
    }
}
