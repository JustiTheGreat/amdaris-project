using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Dtos.ResponseDTOs;
using AmdarisProject.Application.Handlers.MatchHandlers;
using AmdarisProject.Domain.Enums;
using AmdarisProject.TestUtils;
using AmdarisProject.TestUtils.ModelBuilders;
using AutoMapper;
using Moq;
using Match = AmdarisProject.Domain.Models.Match;

namespace AmdarisProject.Application.Test.Tests.MatchTests
{
    public class EndMatchHandlerTest : ApplicationTestBase
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
            Match match = APBuilder.CreateBasicMatch().SetStatus(matchStatus).SetInitialEndTime(DateTime.UtcNow).Get();
            _endMatchServiceMock.Setup(o => o.End(It.IsAny<Guid>(), It.IsAny<MatchStatus>())).Returns(Task.FromResult(match));
            _mapperMock.Setup(o => o.Map<MatchGetDTO>(It.IsAny<Match>())).Returns(_mapper.Map<MatchGetDTO>(match));
            EndMatch command = new(match.Id, matchStatus);
            EndMatchHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object, _endMatchServiceMock.Object);

            MatchGetDTO response = await handler.Handle(command, default);

            AssertResponse.MatchMatchGetDTO(match, response);
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
