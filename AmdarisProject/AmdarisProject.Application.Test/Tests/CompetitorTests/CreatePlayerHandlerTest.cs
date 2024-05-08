using AmdarisProject.Application.Dtos.CreateDTOs;
using AmdarisProject.Application.Dtos.ResponseDTOs.CompetitorResponseDTOs;
using AmdarisProject.Application.Handlers.CompetitorHandlers;
using AmdarisProject.Application.Test.ModelBuilder;
using AmdarisProject.Domain.Models.CompetitorModels;
using Mapster;
using Moq;

namespace AmdarisProject.Application.Test.Tests.CompetitorTests
{
    public class CreatePlayerHandlerTest : MockObjectUser
    {
        [Fact]
        public async Task Test_CreatePlayerHandler_Success()
        {
            Player player = Builders.CreateBasicPlayer().Get();
            CompetitorCreateDTO createDTO = player.Adapt<CompetitorCreateDTO>();
            _mapperMock.Setup(o => o.Map<Player>(It.IsAny<CompetitorCreateDTO>())).Returns(player);
            _unitOfWorkMock.Setup(o => o.CompetitorRepository).Returns(_competitorRepositoryMock.Object);
            _competitorRepositoryMock.Setup(o => o.Create(It.IsAny<Player>())).Returns(Task.FromResult((Competitor)player));
            _mapperMock.Setup(o => o.Map<PlayerGetDTO>(It.IsAny<Player>())).Returns(player.Adapt<PlayerGetDTO>());
            CreatePlayer command = new(createDTO);
            CreatePlayerHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object, GetLogger<CreatePlayerHandler>());

            PlayerGetDTO response = await handler.Handle(command, default);

            Assert.Equal(createDTO.Name, response.Name);
            Assert.Empty(response.Matches);
            Assert.Empty(response.WonMatches);
            Assert.Empty(response.Competitions);
            Assert.Empty(response.Points);
            Assert.Empty(response.Teams);
        }

        [Fact]
        public async Task Test_CreatePlayerHandler_RollbackIsCalled_throws_Exception()
        {
            _unitOfWorkMock.Setup(o => o.CompetitorRepository).Returns(_competitorRepositoryMock.Object);
            _competitorRepositoryMock.Setup(o => o.Create(It.IsAny<Competitor>())).Throws<Exception>();
            CreatePlayer command = new(It.IsAny<CompetitorCreateDTO>());
            CreatePlayerHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object, GetLogger<CreatePlayerHandler>());

            await Assert.ThrowsAsync<Exception>(async () => await handler.Handle(command, default));

            _unitOfWorkMock.Verify(o => o.RollbackTransactionAsync(), Times.Once);
        }
    }
}
