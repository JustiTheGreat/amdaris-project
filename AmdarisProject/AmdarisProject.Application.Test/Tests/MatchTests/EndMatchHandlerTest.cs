using AmdarisProject.Application.Dtos.ResponseDTOs;
using AmdarisProject.Application.Handlers.MatchHandlers;
using AmdarisProject.Application.Services;
using AmdarisProject.Application.Test.ModelBuilders;
using AmdarisProject.Domain.Enums;
using Mapster;
using MapsterMapper;
using Moq;
using Match = AmdarisProject.Domain.Models.Match;

namespace AmdarisProject.Application.Test.Tests.MatchTests
{
    public class EndMatchHandlerTest : MockObjectUser
    {
        private readonly Mock<IEndMatchService> _endMatchServiceMock = new();

        public static TheoryData<MatchStatus> MatchStatuses => new()
        {
            MatchStatus.FINISHED,
            MatchStatus.SPECIAL_WIN_COMPETITOR_ONE,
            MatchStatus.SPECIAL_WIN_COMPETITOR_TWO
        };

        [Theory]
        [MemberData(nameof(MatchStatuses))]
        public async Task Test_EndMatchHandler_Success(MatchStatus matchStatus)
        {
            Match match = APBuilder.CreateBasicMatch().SetStatus(matchStatus).Get();
            _endMatchServiceMock.Setup(o => o.End(It.IsAny<Guid>(), It.IsAny<MatchStatus>())).Returns(Task.FromResult(match));
            _mapperMock.Setup(o => o.Map<MatchGetDTO>(It.IsAny<Match>())).Returns(match.Adapt<MatchGetDTO>());
            EndMatch command = new(match.Id, matchStatus);
            EndMatchHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object, _endMatchServiceMock.Object);

            MatchGetDTO response = await handler.Handle(command, default);

            Assert.Equal(match.Id, response.Id);
            Assert.Equal(match.StartTime, response.StartTime);
            Assert.Equal(match.EndTime, response.EndTime);
            Assert.Equal(match.Status, response.Status);
            Assert.Equal(match.CompetitorOne.Id, response.CompetitorOne.Id);
            Assert.Equal(match.CompetitorOne.Name, response.CompetitorOne.Name);
            Assert.Equal(match.CompetitorTwo.Id, response.CompetitorTwo.Id);
            Assert.Equal(match.CompetitorTwo.Name, response.CompetitorTwo.Name);
            Assert.Equal(match.Competition.Id, response.Competition.Id);
            Assert.Equal(match.Competition.Name, response.Competition.Name);
            Assert.Equal(match.Competition.Status, response.Competition.Status);
            Assert.Equal(match.Competition.GameFormat.GameType, response.Competition.GameType);
            Assert.Equal(match.Competition.GameFormat.CompetitorType, response.Competition.CompetitorType);
            Assert.Equal(match.CompetitorOnePoints, response.CompetitorOnePoints);
            Assert.Equal(match.CompetitorTwoPoints, response.CompetitorTwoPoints);
            Assert.Equal(match.Winner?.Id, response.Winner?.Id);
            Assert.Equal(match.Winner?.Name, response.Winner?.Name);
            Assert.Equal(match.StageLevel, response.StageLevel);
            Assert.Equal(match.StageIndex, response.StageIndex);
            Assert.Equal(match.Points.Count, response.Points.Count);
            match.Points.ForEach(point =>
            {
                Assert.Equal(point.Id, response.Points.FirstOrDefault(pointDisplay => pointDisplay.Id.Equals(point.Id))?.Id);
                Assert.Equal(point.Value, response.Points.FirstOrDefault(pointDisplay => pointDisplay.Id.Equals(point.Id))?.Value);
                Assert.Equal(point.Player.Name, response.Points.FirstOrDefault(pointDisplay => pointDisplay.Id.Equals(point.Id))?.PlayerName);
            });
        }

        [Fact]
        public async Task Test_EndMatchHandler_RollbackIsCalled_throws_Exception()
        {
            Match match = APBuilder.CreateBasicMatch().Get();
            _endMatchServiceMock.Setup(o => o.End(It.IsAny<Guid>(), It.IsAny<MatchStatus>())).Throws<Exception>();
            EndMatch command = new(It.IsAny<Guid>(), It.IsAny<MatchStatus>());
            EndMatchHandler handler = new(_unitOfWorkMock.Object, It.IsAny<IMapper>(), _endMatchServiceMock.Object);

            await Assert.ThrowsAsync<Exception>(async () => await handler.Handle(command, default));

            _unitOfWorkMock.Verify(x => x.RollbackTransactionAsync(), Times.Once());
        }
    }
}
