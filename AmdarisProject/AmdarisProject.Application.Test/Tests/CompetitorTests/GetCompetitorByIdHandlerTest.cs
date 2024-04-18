using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Dtos.ResponseDTOs.CompetitorResponseDTOs;
using AmdarisProject.Application.Handlers.CompetitorHandlers;
using AmdarisProject.Application.Test.ModelBuilder;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models.CompetitorModels;
using AmdarisProject.Presentation;
using Mapster;
using MapsterMapper;
using Moq;

namespace AmdarisProject.Application.Test.Tests.CompetitorTests
{
    public class GetCompetitorByIdHandlerTest
    {
        private readonly Mock<ICompetitorRepository> _competitorRepositoryMock = new();
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly Mock<IMapper> _mapperMock = new();

        public GetCompetitorByIdHandlerTest() => MapsterConfiguration.ConfigureMapster();

        [Fact]
        public async Task Test_GetCompetitorByIdHandlerTest_Player_Success()
        {
            Player model = Builders.CreateBasicPlayer().Get();
            _unitOfWorkMock.Setup(o => o.CompetitorRepository).Returns(_competitorRepositoryMock.Object);
            _competitorRepositoryMock.Setup(o => o.GetById(It.IsAny<Guid>())).Returns(Task.FromResult((Competitor?)model));
            _mapperMock.Setup(o => o.Map<PlayerResponseDTO>(It.IsAny<Player>())).Returns(model.Adapt<PlayerResponseDTO>());
            GetCompetitorById command = new(model.Id);
            GetCompetitorByIdHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object);

            CompetitorResponseDTO response = await handler.Handle(command, default);

            Assert.True(response is PlayerResponseDTO);
            Assert.Equal(model.Id, response.Id);
            _competitorRepositoryMock.Verify(o => o.GetById(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async Task Test_GetCompetitorByIdHandlerTest_Team_Success()
        {
            Team model = Builders.CreateBasicTeam().Get();
            _unitOfWorkMock.Setup(o => o.CompetitorRepository).Returns(_competitorRepositoryMock.Object);
            _competitorRepositoryMock.Setup(o => o.GetById(It.IsAny<Guid>())).Returns(Task.FromResult((Competitor?)model));
            _mapperMock.Setup(o => o.Map<TeamResponseDTO>(It.IsAny<Team>())).Returns(model.Adapt<TeamResponseDTO>());
            GetCompetitorById command = new(model.Id);
            GetCompetitorByIdHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object);

            CompetitorResponseDTO response = await handler.Handle(command, default);

            Assert.True(response is TeamResponseDTO);
            Assert.Equal(model.Id, response.Id);
            _competitorRepositoryMock.Verify(o => o.GetById(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async Task Test_GetCompetitorByIdHandlerTest_CompetitorNotFound_Throws_APNotFoundException()
        {
            _unitOfWorkMock.Setup(o => o.CompetitorRepository).Returns(_competitorRepositoryMock.Object);
            _competitorRepositoryMock.Setup(o => o.GetById(It.IsAny<Guid>())).Returns(Task.FromResult((Competitor?)null));
            GetCompetitorById command = new(It.IsAny<Guid>());
            GetCompetitorByIdHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object);

            await Assert.ThrowsAsync<APNotFoundException>(async () => await handler.Handle(command, default));

            _competitorRepositoryMock.Verify(o => o.GetById(It.IsAny<Guid>()), Times.Once);
        }
    }
}
