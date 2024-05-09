using AmdarisProject.Application.Dtos.ResponseDTOs;
using AmdarisProject.Application.Handlers.MatchHandlers;
using AmdarisProject.Application.Test.ModelBuilders;
using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models;
using AmdarisProject.Domain.Models.CompetitionModels;
using Mapster;
using MapsterMapper;
using Moq;
using Match = AmdarisProject.Domain.Models.Match;

namespace AmdarisProject.Application.Test.Tests.MatchTests
{
    public class StartMatchHandlerTest : MockObjectUser
    {
        [Fact]
        public async Task Test_StartMatchHandler_Success()
        {
            MatchBuilder matchBuilder = Builder.CreateBasicMatch()
                .SetCompetition(Builder.CreateBasicOneVSAllCompetition().SetStatus(CompetitionStatus.STARTED).Get());
            Match match = matchBuilder.Get();
            Match updated = matchBuilder.Clone().SetStatus(MatchStatus.STARTED).InitializePoints().Get();
            _unitOfWorkMock.Setup(o => o.MatchRepository).Returns(_matchRepositoryMock.Object);
            _unitOfWorkMock.Setup(o => o.PointRepository).Returns(_pointRepositoryMock.Object);
            _unitOfWorkMock.Setup(o => o.CompetitionRepository).Returns(_competitionRepositoryMock.Object);
            _matchRepositoryMock.Setup(o => o.GetById(It.IsAny<Guid>())).Returns(Task.FromResult((Match?)match));
            _matchRepositoryMock.Setup(o => o.Update(It.IsAny<Match>())).Returns(Task.FromResult(updated));
            var v = _pointRepositoryMock.SetupSequence(o => o.Create(It.IsAny<Point>()));
            match.Points.ForEach(point => v.Returns(Task.FromResult(point)));

            _matchRepositoryMock.Setup(o => o.GetNotStartedByCompetitionOrderedByStartTime(It.IsAny<Guid>()))
                .Returns(Task.FromResult((IEnumerable<Match>)[]));
            _mapperMock.Setup(o => o.Map<MatchGetDTO>(It.IsAny<Match>())).Returns(updated.Adapt<MatchGetDTO>());
            StartMatch command = new(match.Id);
            StartMatchHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object, GetLogger<StartMatchHandler>());

            MatchGetDTO response = await handler.Handle(command, default);

            Assert.Equal(match.Id, response.Id);
            Assert.NotNull(match.StartTime);
            Assert.Equal(updated.EndTime, response.EndTime);
            Assert.Equal(updated.Status, response.Status);
            Assert.Equal(updated.CompetitorOne.Id, response.CompetitorOne.Id);
            Assert.Equal(updated.CompetitorOne.Name, response.CompetitorOne.Name);
            Assert.Equal(updated.CompetitorTwo.Id, response.CompetitorTwo.Id);
            Assert.Equal(updated.CompetitorTwo.Name, response.CompetitorTwo.Name);
            Assert.Equal(updated.Competition.Id, response.Competition.Id);
            Assert.Equal(updated.Competition.Name, response.Competition.Name);
            Assert.Equal(updated.Competition.Status, response.Competition.Status);
            Assert.Equal(updated.Competition.GameFormat.GameType, response.Competition.GameType);
            Assert.Equal(updated.Competition.GameFormat.CompetitorType, response.Competition.CompetitorType);
            Assert.Equal(updated.CompetitorOnePoints, response.CompetitorOnePoints);
            Assert.Equal(updated.CompetitorTwoPoints, response.CompetitorTwoPoints);
            Assert.Equal(updated.Winner?.Id, response.Winner?.Id);
            Assert.Equal(updated.Winner?.Name, response.Winner?.Name);
            Assert.Equal(updated.StageLevel, response.StageLevel);
            Assert.Equal(updated.StageIndex, response.StageIndex);
            Assert.Equal(updated.Points.Count, response.Points.Count);
            match.Points.ForEach(point =>
            {
                Assert.Equal(point.Id, response.Points.FirstOrDefault(pointDisplay => pointDisplay.Id.Equals(point.Id))?.Id);
                Assert.Equal(point.Value, response.Points.FirstOrDefault(pointDisplay => pointDisplay.Id.Equals(point.Id))?.Value);
            });
        }

        [Fact]
        public async Task Test_StartMatchHandler_MatchNotFound_throws_APIllegalStatusException()
        {
            _unitOfWorkMock.Setup(o => o.MatchRepository).Returns(_matchRepositoryMock.Object);
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
            Match match = Builder.CreateBasicMatch()
                .SetCompetition(Builder.CreateBasicOneVSAllCompetition().SetStatus(competitionStatus).Get()).Get();
            _unitOfWorkMock.Setup(o => o.MatchRepository).Returns(_matchRepositoryMock.Object);
            _matchRepositoryMock.Setup(o => o.GetById(It.IsAny<Guid>())).Returns(Task.FromResult((Match?)match));
            StartMatch command = new(match.Id);
            StartMatchHandler handler = new(_unitOfWorkMock.Object, It.IsAny<IMapper>(), GetLogger<StartMatchHandler>());

            await Assert.ThrowsAsync<APIllegalStatusException>(async () => await handler.Handle(command, default));
        }

        [Fact]
        public async Task Test_EndMatchHandler_AnotherMatchIsBeingPlayed_throws_AmdarisProjectException()
        {
            Competition competition = Builder.CreateBasicOneVSAllCompetition().SetStatus(CompetitionStatus.STARTED).Get();
            Match match = Builder.CreateBasicMatch().SetCompetition(competition).Get();
            competition.Matches.AddRange([match, Builder.CreateBasicMatch().SetCompetition(competition).SetStatus(MatchStatus.STARTED).Get()]);
            _unitOfWorkMock.Setup(o => o.MatchRepository).Returns(_matchRepositoryMock.Object);
            _matchRepositoryMock.Setup(o => o.GetById(It.IsAny<Guid>())).Returns(Task.FromResult((Match?)match));
            StartMatch command = new(match.Id);
            StartMatchHandler handler = new(_unitOfWorkMock.Object, It.IsAny<IMapper>(), GetLogger<StartMatchHandler>());

            await Assert.ThrowsAsync<AmdarisProjectException>(async () => await handler.Handle(command, default));
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
            Match match = Builder.CreateBasicMatch().SetStatus(matchStatus)
                .SetCompetition(Builder.CreateBasicOneVSAllCompetition().SetStatus(CompetitionStatus.STARTED).Get()).Get();
            _unitOfWorkMock.Setup(o => o.MatchRepository).Returns(_matchRepositoryMock.Object);
            _matchRepositoryMock.Setup(o => o.GetById(It.IsAny<Guid>())).Returns(Task.FromResult((Match?)match));
            StartMatch command = new(match.Id);
            StartMatchHandler handler = new(_unitOfWorkMock.Object, It.IsAny<IMapper>(), GetLogger<StartMatchHandler>());

            await Assert.ThrowsAsync<APIllegalStatusException>(async () => await handler.Handle(command, default));
        }
    }
}
