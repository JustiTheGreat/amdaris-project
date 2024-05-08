using AmdarisProject.Application.Dtos.ResponseDTOs;
using AmdarisProject.Application.Handlers.MatchHandlers;
using AmdarisProject.Application.Services.CompetitionMatchCreatorFactoryService.MatchCreatorService;
using AmdarisProject.Application.Services.CompetitionMatchCreatorServices;
using AmdarisProject.Application.Test.ModelBuilder;
using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Exceptions;
using Mapster;
using MapsterMapper;
using Moq;
using Match = AmdarisProject.Domain.Models.Match;

namespace AmdarisProject.Application.Test.Tests.MatchTests
{
    public class CancelMatchHandlerTest : MockObjectUser
    {
        private readonly Mock<ICompetitionMatchCreatorFactoryService> _competitionMatchCreatorFactoryServiceMock = new();
        private readonly Mock<ICompetitionMatchCreatorService> _competitionMatchCreatorMock = new();

        [Fact]
        public async Task Test_CancelMatchHandler_Success()
        {
            MatchBuilder matchBuilder = Builders.CreateBasicMatch();
            Match match = matchBuilder.Get();
            Match updatedMatch = matchBuilder.Clone().SetStatus(MatchStatus.CANCELED).Get();
            _unitOfWorkMock.Setup(o => o.MatchRepository).Returns(_matchRepositoryMock.Object);
            _matchRepositoryMock.Setup(o => o.GetById(It.IsAny<Guid>())).Returns(Task.FromResult((Match?)match));
            _matchRepositoryMock.Setup(o => o.Update(It.IsAny<Match>())).Returns(Task.FromResult(updatedMatch));
            _competitionMatchCreatorFactoryServiceMock.Setup(o => o.GetCompetitionMatchCreator(It.IsAny<Type>()))
                .Returns(_competitionMatchCreatorMock.Object);
            _competitionMatchCreatorMock.Setup(o => o.CreateCompetitionMatches(It.IsAny<Guid>()))
                .Returns(Task.FromResult(It.IsAny<IEnumerable<Match>>()));
            _mapperMock.Setup(o => o.Map<MatchGetDTO>(It.IsAny<Match>())).Returns(updatedMatch.Adapt<MatchGetDTO>());
            CancelMatch command = new(match.Id);
            CancelMatchHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object,
                _competitionMatchCreatorFactoryServiceMock.Object, GetLogger<CancelMatchHandler>());

            MatchGetDTO response = await handler.Handle(command, default);

            Assert.Equal(updatedMatch.Id, response.Id);
            Assert.Equal(updatedMatch.StartTime, response.StartTime);
            Assert.Equal(updatedMatch.EndTime, response.EndTime);
            Assert.Equal(updatedMatch.Status, response.Status);
            Assert.Equal(updatedMatch.CompetitorOne.Id, response.CompetitorOne.Id);
            Assert.Equal(updatedMatch.CompetitorOne.Name, response.CompetitorOne.Name);
            Assert.Equal(updatedMatch.CompetitorTwo.Id, response.CompetitorTwo.Id);
            Assert.Equal(updatedMatch.CompetitorTwo.Name, response.CompetitorTwo.Name);
            Assert.Equal(updatedMatch.Competition.Id, response.Competition.Id);
            Assert.Equal(updatedMatch.Competition.Name, response.Competition.Name);
            Assert.Equal(updatedMatch.Competition.Status, response.Competition.Status);
            Assert.Equal(updatedMatch.Competition.GameFormat.GameType, response.Competition.GameType);
            Assert.Equal(updatedMatch.Competition.GameFormat.CompetitorType, response.Competition.CompetitorType);
            Assert.Equal(updatedMatch.CompetitorOnePoints, response.CompetitorOnePoints);
            Assert.Equal(updatedMatch.CompetitorTwoPoints, response.CompetitorTwoPoints);
            Assert.Equal(updatedMatch.Winner?.Id, response.Winner?.Id);
            Assert.Equal(updatedMatch.Winner?.Name, response.Winner?.Name);
            Assert.Equal(updatedMatch.StageLevel, response.StageLevel);
            Assert.Equal(updatedMatch.StageIndex, response.StageIndex);
            Assert.Equal(updatedMatch.Points.Count, response.Points.Count);
            match.Points.ForEach(point =>
            {
                Assert.Equal(point.Id, response.Points.FirstOrDefault(pointDisplay => pointDisplay.Id.Equals(point.Id))?.Id);
                Assert.Equal(point.Value, response.Points.FirstOrDefault(pointDisplay => pointDisplay.Id.Equals(point.Id))?.Value);
            });
        }

        [Fact]
        public async Task Test_CancelMatchHandler_MatchNotFound_throws_APNotFoundException()
        {
            Match match = Builders.CreateBasicMatch().Get();
            _unitOfWorkMock.Setup(o => o.MatchRepository).Returns(_matchRepositoryMock.Object);
            _matchRepositoryMock.Setup(o => o.GetById(It.IsAny<Guid>())).Returns(Task.FromResult((Match?)null));
            CancelMatch command = new(match.Id);
            CancelMatchHandler handler = new(_unitOfWorkMock.Object, It.IsAny<IMapper>(),
                It.IsAny<ICompetitionMatchCreatorFactoryService>(), GetLogger<CancelMatchHandler>());

            await Assert.ThrowsAsync<APNotFoundException>(async () => await handler.Handle(command, default));
        }

        [Fact]
        public async Task Test_CancelMatchHandler_RollbackIsCalled_throws_Exception()
        {
            Match match = Builders.CreateBasicMatch().Get();
            _unitOfWorkMock.Setup(o => o.MatchRepository).Returns(_matchRepositoryMock.Object);
            _matchRepositoryMock.Setup(o => o.GetById(It.IsAny<Guid>())).Returns(Task.FromResult((Match?)match));
            _matchRepositoryMock.Setup(o => o.Update(It.IsAny<Match>())).Throws<Exception>();
            CancelMatch command = new(match.Id);
            CancelMatchHandler handler = new(_unitOfWorkMock.Object, It.IsAny<IMapper>(),
                It.IsAny<ICompetitionMatchCreatorFactoryService>(), GetLogger<CancelMatchHandler>());

            await Assert.ThrowsAsync<Exception>(async () => await handler.Handle(command, default));

            _unitOfWorkMock.Verify(x => x.RollbackTransactionAsync(), Times.Once());
        }
    }
}
