using AmdarisProject.Application.Dtos.ResponseDTOs;
using AmdarisProject.Application.Handlers.MatchHandlers;
using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models;
using AmdarisProject.Domain.Models.CompetitionModels;
using AmdarisProject.TestUtils;
using AmdarisProject.TestUtils.ModelBuilders;
using AutoMapper;
using Moq;
using Match = AmdarisProject.Domain.Models.Match;

namespace AmdarisProject.Application.Test.Tests.MatchTests
{
    public class StartMatchHandlerTest : ApplicationTestBase
    {
        [Fact]
        public async Task Test_StartMatchHandler_Success()
        {
            MatchBuilder matchBuilder = APBuilder.CreateBasicMatch()
                .SetCompetition(APBuilder.CreateBasicOneVSAllCompetition().SetStatus(CompetitionStatus.STARTED).Get());
            Match match = matchBuilder.Get();
            _matchRepositoryMock.Setup(o => o.GetById(It.IsAny<Guid>())).Returns(Task.FromResult((Match?)match));
            match = matchBuilder.Clone().SetStatus(MatchStatus.STARTED).SetStartTime(DateTime.UtcNow).InitializePoints().Get();
            _matchRepositoryMock.Setup(o => o.Update(It.IsAny<Match>())).Returns(Task.FromResult(match));
            var v = _pointRepositoryMock.SetupSequence(o => o.Create(It.IsAny<Point>()));
            match.Points.ForEach(point => v.Returns(Task.FromResult(point)));
            _matchRepositoryMock.Setup(o => o.GetNotStartedByCompetitionOrderedByStartTime(It.IsAny<Guid>()))
                .Returns(Task.FromResult((IEnumerable<Match>)[]));
            _mapperMock.Setup(o => o.Map<MatchGetDTO>(It.IsAny<Match>())).Returns(_mapper.Map<MatchGetDTO>(match));
            StartMatch command = new(match.Id);
            StartMatchHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object, GetLogger<StartMatchHandler>());

            MatchGetDTO response = await handler.Handle(command, default);

            AssertResponse.MatchMatchGetDTO(match, response, startMatch: true);
        }

        [Fact]
        public async Task Test_StartMatchHandler_MatchNotFound_throws_APIllegalStatusException()
        {
            _matchRepositoryMock.Setup(o => o.GetById(It.IsAny<Guid>())).Returns(Task.FromResult((Match?)null));
            StartMatch command = new(It.IsAny<Guid>());
            StartMatchHandler handler = new(_unitOfWorkMock.Object, It.IsAny<IMapper>(), GetLogger<StartMatchHandler>());

            await Assert.ThrowsAsync<APNotFoundException>(async () => await handler.Handle(command, default));
        }

        public static TheoryData<CompetitionStatus> CompetitionStatuses => new()
        {
            CompetitionStatus.ORGANIZING,
            CompetitionStatus.NOT_STARTED,
            CompetitionStatus.FINISHED,
            CompetitionStatus.CANCELED
        };

        [Theory]
        [MemberData(nameof(CompetitionStatuses))]
        public async Task Test_EndMatchHandler_IllegalCompetitionStatus_throws_APIllegalStatusException(CompetitionStatus competitionStatus)
        {
            Match match = APBuilder.CreateBasicMatch()
                .SetCompetition(APBuilder.CreateBasicOneVSAllCompetition().SetStatus(competitionStatus).Get()).Get();
            _matchRepositoryMock.Setup(o => o.GetById(It.IsAny<Guid>())).Returns(Task.FromResult((Match?)match));
            StartMatch command = new(match.Id);
            StartMatchHandler handler = new(_unitOfWorkMock.Object, It.IsAny<IMapper>(), GetLogger<StartMatchHandler>());

            await Assert.ThrowsAsync<APIllegalStatusException>(async () => await handler.Handle(command, default));
        }

        [Fact]
        public async Task Test_EndMatchHandler_AnotherMatchIsBeingPlayed_throws_AmdarisProjectException()
        {
            Competition competition = APBuilder.CreateBasicOneVSAllCompetition().SetStatus(CompetitionStatus.STARTED).Get();
            Match match = APBuilder.CreateBasicMatch().SetCompetition(competition).Get();
            competition.Matches.AddRange([match, APBuilder.CreateBasicMatch().SetCompetition(competition).SetStatus(MatchStatus.STARTED).Get()]);
            _matchRepositoryMock.Setup(o => o.GetById(It.IsAny<Guid>())).Returns(Task.FromResult((Match?)match));
            StartMatch command = new(match.Id);
            StartMatchHandler handler = new(_unitOfWorkMock.Object, It.IsAny<IMapper>(), GetLogger<StartMatchHandler>());

            await Assert.ThrowsAsync<APException>(async () => await handler.Handle(command, default));
        }

        public static TheoryData<MatchStatus> MatchStatuses => new()
        {
            MatchStatus.STARTED,
            MatchStatus.FINISHED,
            MatchStatus.SPECIAL_WIN_COMPETITOR_ONE,
            MatchStatus.SPECIAL_WIN_COMPETITOR_TWO,
            MatchStatus.CANCELED
        };

        [Theory]
        [MemberData(nameof(MatchStatuses))]
        public async Task Test_EndMatchHandler_IllegalMatchStatus_throws_APIllegalStatusException(MatchStatus matchStatus)
        {
            Match match = APBuilder.CreateBasicMatch().SetStatus(matchStatus)
                .SetCompetition(APBuilder.CreateBasicOneVSAllCompetition().SetStatus(CompetitionStatus.STARTED).Get()).Get();
            _matchRepositoryMock.Setup(o => o.GetById(It.IsAny<Guid>())).Returns(Task.FromResult((Match?)match));
            StartMatch command = new(match.Id);
            StartMatchHandler handler = new(_unitOfWorkMock.Object, It.IsAny<IMapper>(), GetLogger<StartMatchHandler>());

            await Assert.ThrowsAsync<APIllegalStatusException>(async () => await handler.Handle(command, default));
        }
    }
}
