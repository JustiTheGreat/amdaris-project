using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Dtos.ResponseDTOs.CompetitorResponseDTOs;
using AmdarisProject.Application.Handlers.CompetitorHandlers;
using AmdarisProject.Domain.Models.CompetitorModels;
using AmdarisProject.Presentation;
using MapsterMapper;
using Moq;

namespace AmdarisProject.Application.Test.Tests.CompetitorTests
{
    public class GetAllPlayersHandlerTest
    {
        private readonly Mock<ICompetitorRepository> _competitorRepositoryMock = new();
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly Mock<IMapper> _mapperMock = new();

        public GetAllPlayersHandlerTest() => MapsterConfiguration.ConfigureMapster();

        [Fact]
        public async Task Test_GetAllPlayersHandler_Success()
        {
            _unitOfWorkMock.Setup(o => o.CompetitorRepository).Returns(_competitorRepositoryMock.Object);
            _competitorRepositoryMock.Setup(o => o.GetAllPlayers()).Returns(Task.FromResult((IEnumerable<Player>)[]));
            GetPlayers command = new();
            GetAllPlayersHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object);

            IEnumerable<PlayerResponseDTO> response = await handler.Handle(command, default);

            _competitorRepositoryMock.Verify(o => o.GetAllPlayers(), Times.Once);
        }
    }
}
