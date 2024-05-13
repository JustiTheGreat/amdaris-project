using AmdarisProject.Application.Dtos.CreateDTOs;
using AmdarisProject.Application.Dtos.ResponseDTOs;
using AmdarisProject.Application.Handlers.GameFormatHandlers;
using AmdarisProject.Application.Test.ModelBuilders;
using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models;
using Mapster;
using MapsterMapper;
using Moq;

namespace AmdarisProject.Application.Test.Tests.GameFormatTests
{
    public class CreateGameFormatHandlerTest : MockObjectUser
    {
        [Fact]
        public async Task Test_CreateGameFormatHandler_Success()
        {
            GameFormat gameFormat = APBuilder.CreateBasicGameFormat().Get();
            _mapperMock.Setup(o => o.Map<GameFormat>(It.IsAny<GameFormatCreateDTO>())).Returns(gameFormat);
            _unitOfWorkMock.Setup(o => o.GameFormatRepository).Returns(_gameFormatRepositoryMock.Object);
            _gameFormatRepositoryMock.Setup(o => o.Create(It.IsAny<GameFormat>())).Returns(Task.FromResult(gameFormat));
            _mapperMock.Setup(o => o.Map<GameFormatGetDTO>(It.IsAny<GameFormat>())).Returns(gameFormat.Adapt<GameFormatGetDTO>());
            GameFormatCreateDTO createDTO = gameFormat.Adapt<GameFormatCreateDTO>();
            CreateGameFormat command = new(createDTO);
            CreateGameFormatHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object, GetLogger<CreateGameFormatHandler>());

            GameFormatGetDTO response = await handler.Handle(command, default);

            Assert.Equal(createDTO.Name, response.Name);
            Assert.Equal(createDTO.GameType, response.GameType);
            Assert.Equal(createDTO.CompetitorType, response.CompetitorType);
            Assert.Equal(createDTO.TeamSize, response.TeamSize);
            Assert.Equal(createDTO.WinAt, response.WinAt);
            Assert.Equal(createDTO.DurationInMinutes, response.DurationInMinutes);
        }

        [Fact]
        public async Task Test_CreateGameFormatHandler_WinningConditions_throws_APArgumentException()
        {
            GameFormatCreateDTO createDTO = APBuilder.CreateBasicGameFormat().SetWinAt(null).SetDurationInMinutes(null).Get().Adapt<GameFormatCreateDTO>();
            CreateGameFormat command = new(createDTO);
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
        public async Task Test_CreateGameFormatHandler_CompetitorRequirements_throws_APArgumentException(GameFormat model)
        {
            GameFormatCreateDTO createDTO = model.Adapt<GameFormatCreateDTO>();
            CreateGameFormat command = new(createDTO);
            CreateGameFormatHandler handler = new(_unitOfWorkMock.Object, It.IsAny<IMapper>(), GetLogger<CreateGameFormatHandler>());

            await Assert.ThrowsAsync<APArgumentException>(async () => await handler.Handle(command, default));
        }

        [Fact]
        public async Task Test_CreateGameFormatHandler_RollbackIsCalled_throws_Exception()
        {
            _unitOfWorkMock.Setup(o => o.GameFormatRepository).Returns(_gameFormatRepositoryMock.Object);
            _gameFormatRepositoryMock.Setup(o => o.Create(It.IsAny<GameFormat>())).Throws<Exception>();
            GameFormatCreateDTO createDTO = APBuilder.CreateBasicGameFormat().Get().Adapt<GameFormatCreateDTO>();
            CreateGameFormat command = new(createDTO);
            CreateGameFormatHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object, GetLogger<CreateGameFormatHandler>());

            await Assert.ThrowsAsync<Exception>(async () => await handler.Handle(command, default));

            _unitOfWorkMock.Verify(o => o.RollbackTransactionAsync(), Times.Once);
        }
    }
}
