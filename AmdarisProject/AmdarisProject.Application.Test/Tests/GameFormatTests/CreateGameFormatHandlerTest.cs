using AmdarisProject.Application.Dtos.RequestDTOs.CreateDTOs;
using AmdarisProject.Application.Dtos.ResponseDTOs;
using AmdarisProject.Application.Handlers.GameFormatHandlers;
using AmdarisProject.Application.Test.ModelBuilders;
using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models;
using AutoMapper;
using Moq;

namespace AmdarisProject.Application.Test.Tests.GameFormatTests
{
    public class CreateGameFormatHandlerTest : ApplicationTestBase
    {
        [Fact]
        public async Task Test_CreateGameFormatHandler_Success()
        {
            GameFormat gameFormat = APBuilder.CreateBasicGameFormat().Get();
            _mapperMock.Setup(o => o.Map<GameFormat>(It.IsAny<GameFormatCreateDTO>())).Returns(gameFormat);
            _gameFormatRepositoryMock.Setup(o => o.Create(It.IsAny<GameFormat>())).Returns(Task.FromResult(gameFormat));
            _mapperMock.Setup(o => o.Map<GameFormatGetDTO>(It.IsAny<GameFormat>())).Returns(_mapper.Map<GameFormatGetDTO>(gameFormat));
            CreateGameFormat command = new(_mapper.Map<GameFormatCreateDTO>(gameFormat));
            CreateGameFormatHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object, GetLogger<CreateGameFormatHandler>());

            GameFormatGetDTO response = await handler.Handle(command, default);

            Assert.Equal(gameFormat.Name, response.Name);
            Assert.Equal(gameFormat.GameType, response.GameType);
            Assert.Equal(gameFormat.CompetitorType, response.CompetitorType);
            Assert.Equal(gameFormat.TeamSize, response.TeamSize);
            Assert.Equal(gameFormat.WinAt, response.WinAt);
            Assert.Equal(gameFormat.DurationInMinutes, response.DurationInMinutes);
        }

        [Fact]
        public async Task Test_CreateGameFormatHandler_WinningConditions_throws_APArgumentException()
        {
            GameFormat gameFormat = APBuilder.CreateBasicGameFormat().SetWinAt(null).SetDurationInMinutes(null).Get();
            CreateGameFormat command = new(_mapper.Map<GameFormatCreateDTO>(gameFormat));
            CreateGameFormatHandler handler = new(_unitOfWorkMock.Object, It.IsAny<IMapper>(), GetLogger<CreateGameFormatHandler>());

            await Assert.ThrowsAsync<APArgumentException>(async () => await handler.Handle(command, default));
        }

        public static TheoryData<GameFormat> CompetitorTypeAndTeamSize => new()
        {
            APBuilder.CreateBasicGameFormat().SetCompetitorType(CompetitorType.PLAYER).SetTeamSize(2).Get(),
            APBuilder.CreateBasicGameFormat().SetCompetitorType(CompetitorType.TEAM).SetTeamSize(null).Get(),
            APBuilder.CreateBasicGameFormat().SetCompetitorType(CompetitorType.TEAM).SetTeamSize(1).Get(),
        };

        [Theory]
        [MemberData(nameof(CompetitorTypeAndTeamSize))]
        public async Task Test_CreateGameFormatHandler_CompetitorRequirements_throws_APArgumentException(GameFormat gameFormat)
        {
            CreateGameFormat command = new(_mapper.Map<GameFormatCreateDTO>(gameFormat));
            CreateGameFormatHandler handler = new(_unitOfWorkMock.Object, It.IsAny<IMapper>(), GetLogger<CreateGameFormatHandler>());

            await Assert.ThrowsAsync<APArgumentException>(async () => await handler.Handle(command, default));
        }

        [Fact]
        public async Task Test_CreateGameFormatHandler_RollbackIsCalled_throws_Exception()
        {
            _unitOfWorkMock.Setup(o => o.GameFormatRepository).Returns(_gameFormatRepositoryMock.Object);
            _gameFormatRepositoryMock.Setup(o => o.Create(It.IsAny<GameFormat>())).Throws<Exception>();
            GameFormat gameFormat = APBuilder.CreateBasicGameFormat().Get();
            CreateGameFormat command = new(_mapper.Map<GameFormatCreateDTO>(gameFormat));
            CreateGameFormatHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object, GetLogger<CreateGameFormatHandler>());

            await Assert.ThrowsAsync<Exception>(async () => await handler.Handle(command, default));

            _unitOfWorkMock.Verify(o => o.RollbackTransactionAsync(), Times.Once);
        }
    }
}
