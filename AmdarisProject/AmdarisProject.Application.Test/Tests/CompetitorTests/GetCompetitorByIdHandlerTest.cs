using AmdarisProject.Application.Dtos.ResponseDTOs.CompetitorResponseDTOs;
using AmdarisProject.Application.Handlers.CompetitorHandlers;
using AmdarisProject.Application.Test.ModelBuilder;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models.CompetitorModels;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.Logging;
using Moq;

namespace AmdarisProject.Application.Test.Tests.CompetitorTests
{
    public class GetCompetitorByIdHandlerTest : MockObjectUser
    {
        [Fact]
        public async Task Test_GetCompetitorByIdHandler_Player_Success()
        {
            Player model = Builders.CreateBasicPlayer().Get();
            _unitOfWorkMock.Setup(o => o.CompetitorRepository).Returns(_competitorRepositoryMock.Object);
            _competitorRepositoryMock.Setup(o => o.GetById(It.IsAny<Guid>())).Returns(Task.FromResult((Competitor?)model));
            _mapperMock.Setup(o => o.Map<PlayerGetDTO>(It.IsAny<Player>())).Returns(model.Adapt<PlayerGetDTO>());
            GetCompetitorById command = new(model.Id);
            GetCompetitorByIdHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object,
                It.IsAny<ILogger<GetCompetitorByIdHandler>>());

            CompetitorGetDTO response = await handler.Handle(command, default);

            Assert.True(response is PlayerGetDTO);
            Assert.Equal(model.Id, response.Id);
            _competitorRepositoryMock.Verify(o => o.GetById(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async Task Test_GetCompetitorByIdHandler_Team_Success()
        {
            Team model = Builders.CreateBasicTeam().Get();
            _unitOfWorkMock.Setup(o => o.CompetitorRepository).Returns(_competitorRepositoryMock.Object);
            _competitorRepositoryMock.Setup(o => o.GetById(It.IsAny<Guid>())).Returns(Task.FromResult((Competitor?)model));
            _mapperMock.Setup(o => o.Map<TeamGetDTO>(It.IsAny<Team>())).Returns(model.Adapt<TeamGetDTO>());
            GetCompetitorById command = new(model.Id);
            GetCompetitorByIdHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object,
                It.IsAny<ILogger<GetCompetitorByIdHandler>>());

            CompetitorGetDTO response = await handler.Handle(command, default);

            Assert.True(response is TeamGetDTO);
            Assert.Equal(model.Id, response.Id);
            _competitorRepositoryMock.Verify(o => o.GetById(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async Task Test_GetCompetitorByIdHandler_CompetitorNotFound_throws_APNotFoundException()
        {
            _unitOfWorkMock.Setup(o => o.CompetitorRepository).Returns(_competitorRepositoryMock.Object);
            _competitorRepositoryMock.Setup(o => o.GetById(It.IsAny<Guid>())).Returns(Task.FromResult((Competitor?)null));
            GetCompetitorById command = new(It.IsAny<Guid>());
            GetCompetitorByIdHandler handler = new(_unitOfWorkMock.Object, It.IsAny<IMapper>(),
                It.IsAny<ILogger<GetCompetitorByIdHandler>>());

            await Assert.ThrowsAsync<APNotFoundException>(async () => await handler.Handle(command, default));

            _competitorRepositoryMock.Verify(o => o.GetById(It.IsAny<Guid>()), Times.Once);
        }
    }
}
