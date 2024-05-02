using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Dtos.CreateDTOs;
using AmdarisProject.Application.Dtos.ResponseDTOs;
using AmdarisProject.Application.Handlers.GameFormatHandlers;
using AmdarisProject.Application.Test.ModelBuilder;
using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models;
using AmdarisProject.Presentation;
using Mapster;
using MapsterMapper;
using Moq;

namespace AmdarisProject.Application.Test.Tests.GameFormatTests
{
    public class CreateGameFormatHandlerTest
    {
        private readonly Mock<IGameFormatRepository> _gameFormatRepositoryMock = new();
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly Mock<IMapper> _mapperMock = new();

        public CreateGameFormatHandlerTest() => MapsterConfiguration.ConfigureMapster();

        [Fact]
        public async Task Test_CreateGameFormatHandler_Success()
        {
            GameFormat model = Builders.CreateBasicGameFormat().Get();
            _unitOfWorkMock.Setup(o => o.GameFormatRepository).Returns(_gameFormatRepositoryMock.Object);
            _gameFormatRepositoryMock.Setup(o => o.Create(It.IsAny<GameFormat>())).Returns(Task.FromResult(model));
            _mapperMock.Setup(o => o.Map<GameFormat>(It.IsAny<GameFormatCreateDTO>())).Returns(model);
            _mapperMock.Setup(o => o.Map<GameFormatGetDTO>(It.IsAny<GameFormat>())).Returns(model.Adapt<GameFormatGetDTO>());
            GameFormatCreateDTO createDTO = model.Adapt<GameFormatCreateDTO>();
            CreateGameFormat command = new(createDTO);
            CreateGameFormatHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object);

            GameFormatGetDTO response = await handler.Handle(command, default);

            Assert.Equal(createDTO.Name, response.Name);
            Assert.Equal(createDTO.GameType, response.GameType);
            Assert.Equal(createDTO.CompetitorType, response.CompetitorType);
            Assert.Equal(createDTO.TeamSize, response.TeamSize);
            Assert.Equal(createDTO.WinAt, response.WinAt);
            Assert.Equal(createDTO.DurationInMinutes, response.DurationInMinutes);
        }

        public static TheoryData<GameFormat> WinConditions => new()
        {
            Builders.CreateBasicGameFormat().SetWinAt(null).SetDurationInMinutes(null).Get(),
        };

        public static TheoryData<GameFormat> CompetitorTypeAndTeamSize => new()
        {
            Builders.CreateBasicGameFormat().SetCompetitorType(CompetitorType.PLAYER).SetTeamSize(2).Get(),
            Builders.CreateBasicGameFormat().SetCompetitorType(CompetitorType.TEAM).SetTeamSize(null).Get(),
            Builders.CreateBasicGameFormat().SetCompetitorType(CompetitorType.TEAM).SetTeamSize(1).Get(),
        };

        [Theory]
        [MemberData(nameof(WinConditions))]
        [MemberData(nameof(CompetitorTypeAndTeamSize))]
        public async Task Test_CreateGameFormatHandler_BadData_Throws_APArgumentException(GameFormat model)
        {
            GameFormatCreateDTO createDTO = model.Adapt<GameFormatCreateDTO>();
            CreateGameFormat command = new(createDTO);
            CreateGameFormatHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object);

            await Assert.ThrowsAsync<APArgumentException>(async () => await handler.Handle(command, default));
        }

        [Fact]
        public async Task Test_CreateGameFormatHandler_Transaction_Throws_Exception()
        {
            GameFormat model = Builders.CreateBasicGameFormat().Get();
            _unitOfWorkMock.Setup(o => o.GameFormatRepository).Returns(_gameFormatRepositoryMock.Object);
            _gameFormatRepositoryMock.Setup(o => o.Create(It.IsAny<GameFormat>())).Throws<Exception>();
            CreateGameFormat command = new(model.Adapt<GameFormatCreateDTO>());
            CreateGameFormatHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object);

            await Assert.ThrowsAsync<Exception>(async () => await handler.Handle(command, default));

            _unitOfWorkMock.Verify(o => o.RollbackTransactionAsync(), Times.Once);
        }
    }
}
