using AmdarisProject.Application.Dtos.RequestDTOs.CreateDTOs;
using AmdarisProject.Application.Dtos.ResponseDTOs.CompetitorResponseDTOs;
using AmdarisProject.Application.Handlers.CompetitorHandlers;
using AmdarisProject.Domain.Models.CompetitorModels;
using AmdarisProject.TestUtils;
using AmdarisProject.TestUtils.ModelBuilders;
using Moq;

namespace AmdarisProject.Application.Test.Tests.CompetitorTests
{
    public class CreatePlayerHandlerTest : ApplicationTestBase
    {
        [Fact]
        public async Task Test_CreatePlayerHandler_Success()
        {
            Player player = APBuilder.CreateBasicPlayer().Get();
            _mapperMock.Setup(o => o.Map<Player>(It.IsAny<CompetitorCreateDTO>())).Returns(player);
            _competitorRepositoryMock.Setup(o => o.Create(It.IsAny<Player>())).Returns(Task.FromResult((Competitor)player));
            _mapperMock.Setup(o => o.Map<PlayerGetDTO>(It.IsAny<Player>())).Returns(_mapper.Map<PlayerGetDTO>(player));
            CreatePlayer command = new(_mapper.Map<CompetitorCreateDTO>(player));
            CreatePlayerHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object, GetLogger<CreatePlayerHandler>());

            PlayerGetDTO response = await handler.Handle(command, default);

            AssertResponse.PlayerGetDTO(player, response, createPlayer: true);
        }

        [Fact]
        public async Task Test_CreatePlayerHandler_RollbackIsCalled_throws_Exception()
        {
            _competitorRepositoryMock.Setup(o => o.Create(It.IsAny<Competitor>())).Throws<Exception>();
            CreatePlayer command = new(It.IsAny<CompetitorCreateDTO>());
            CreatePlayerHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object, GetLogger<CreatePlayerHandler>());

            await Assert.ThrowsAsync<Exception>(async () => await handler.Handle(command, default));

            _unitOfWorkMock.Verify(o => o.RollbackTransactionAsync(), Times.Once);
        }
    }
}
