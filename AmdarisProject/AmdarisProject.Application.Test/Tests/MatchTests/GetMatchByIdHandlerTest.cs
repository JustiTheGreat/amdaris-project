using AmdarisProject.Application.Dtos.ResponseDTOs;
using AmdarisProject.Application.Handlers.MatchHandlers;
using AmdarisProject.Application.Test.ModelBuilders;
using AmdarisProject.Domain.Exceptions;
using Mapster;
using MapsterMapper;
using Moq;
using Match = AmdarisProject.Domain.Models.Match;

namespace AmdarisProject.Application.Test.Tests.MatchTests
{
    public class GetMatchByIdHandlerTest : MockObjectUser
    {
        [Fact]
        public async Task Test_GetMatchByIdHandler_Success()
        {
            Match match = Builder.CreateBasicMatch().Get();
            _unitOfWorkMock.Setup(o => o.MatchRepository).Returns(_matchRepositoryMock.Object);
            _matchRepositoryMock.Setup(o => o.GetById(It.IsAny<Guid>())).Returns(Task.FromResult((Match?)match));
            _mapperMock.Setup(o => o.Map<MatchGetDTO>(It.IsAny<Match>())).Returns(match.Adapt<MatchGetDTO>());
            GetMatchById command = new(match.Id);
            GetMatchByIdHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object, GetLogger<GetMatchByIdHandler>());

            MatchGetDTO response = await handler.Handle(command, default);

            _matchRepositoryMock.Verify(o => o.GetById(It.IsAny<Guid>()), Times.Once);
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
            });
        }

        [Fact]
        public async Task Test_GetMatchByIdHandler_MatchNotFound_throws_APNotFoundException()
        {
            Match match = Builder.CreateBasicMatch().Get();
            _unitOfWorkMock.Setup(o => o.MatchRepository).Returns(_matchRepositoryMock.Object);
            _matchRepositoryMock.Setup(o => o.GetById(It.IsAny<Guid>())).Returns(Task.FromResult((Match?)null));
            GetMatchById command = new(match.Id);
            GetMatchByIdHandler handler = new(_unitOfWorkMock.Object, It.IsAny<IMapper>(), GetLogger<GetMatchByIdHandler>());

            await Assert.ThrowsAsync<APNotFoundException>(async () => await handler.Handle(command, default));
        }
    }
}
