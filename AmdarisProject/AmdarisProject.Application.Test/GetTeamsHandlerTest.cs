using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Dtos.ResponseDTOs.CompetitorResponseDTOs;
using AmdarisProject.Application.Handlers.CompetitorHandlers;
using AmdarisProject.Domain.Models.CompetitorModels;
using AmdarisProject.Presentation;
using MapsterMapper;
using Moq;

namespace AmdarisProject.Application.Test
{
    public class GetTeamsHandlerTest
    {
        private readonly Mock<ICompetitorRepository> _competitorRepositoryMock = new();
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly Mock<IMapper> _mapperMock = new();

        public GetTeamsHandlerTest() => MapsterConfiguration.ConfigureMapster();

        [Fact]
        public async Task Test_CreateCompetitorHandler_Player_Success()
        {
            _unitOfWorkMock.Setup(o => o.CompetitorRepository).Returns(_competitorRepositoryMock.Object);
            _competitorRepositoryMock.Setup(o => o.GetAllTeams()).Returns(Task.FromResult((IEnumerable<Team>)[]));
            GetTeams command = new();
            GetTeamsHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object);

            IEnumerable<TeamResponseDTO> response = await handler.Handle(command, default);

            _competitorRepositoryMock.Verify(o => o.GetAllTeams(), Times.Once);
        }
    }
}
