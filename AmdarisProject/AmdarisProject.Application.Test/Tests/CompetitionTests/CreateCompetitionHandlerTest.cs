using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Dtos.CreateDTOs.CompetitionCreateDTOs;
using AmdarisProject.Application.Dtos.ResponseDTOs.CompetitionResponseDTOs;
using AmdarisProject.Application.Handlers.CompetitionHandlers;
using AmdarisProject.Application.Test.ModelBuilder;
using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models;
using AmdarisProject.Domain.Models.CompetitionModels;
using AmdarisProject.Presentation;
using Mapster;
using MapsterMapper;
using Moq;

namespace AmdarisProject.Application.Test.Tests.CompetitionTests
{
    public class CreateCompetitionHandlerTest
    {
        private readonly Mock<ICompetitionRepository> _competitionRepositoryMock = new();
        private readonly Mock<IGameFormatRepository> _gameFormatRepositoryMock = new();
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly Mock<IMapper> _mapperMock = new();

        public CreateCompetitionHandlerTest() => MapsterConfiguration.ConfigureMapster();

        [Fact]
        public async Task Test_CreateCompetitionHandler_OneVSAllCompetition_Success()
        {
            OneVSAllCompetition model = Builders.CreateBasicOneVSAllCompetition().Get();
            _unitOfWorkMock.Setup(o => o.CompetitionRepository).Returns(_competitionRepositoryMock.Object);
            _unitOfWorkMock.Setup(o => o.GameFormatRepository).Returns(_gameFormatRepositoryMock.Object);
            _gameFormatRepositoryMock.Setup(o => o.GetById(It.IsAny<Guid>())).Returns(Task.FromResult((GameFormat?)model.GameFormat));
            _competitionRepositoryMock.Setup(o => o.Create(It.IsAny<OneVSAllCompetition>())).Returns(Task.FromResult((Competition)model));
            _mapperMock.Setup(o => o.Map<OneVSAllCompetition>(It.IsAny<OneVSAllCompetitionCreateDTO>()))
                .Returns(model);
            _mapperMock.Setup(o => o.Map<OneVSAllCompetitionResponseDTO>(It.IsAny<OneVSAllCompetition>()))
                .Returns(model.Adapt<OneVSAllCompetitionResponseDTO>());
            OneVSAllCompetitionCreateDTO createDTO = model.Adapt<OneVSAllCompetitionCreateDTO>();
            CreateCompetition command = new(createDTO);
            CreateCompetitionHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object);

            CompetitionGetDTO response = await handler.Handle(command, default);

            Assert.True(response is OneVSAllCompetitionResponseDTO);
            Assert.Equal(createDTO.Name, response.Name);
            Assert.Equal(createDTO.Location, response.Location);
            Assert.Equal(createDTO.StartTime, response.StartTime);
            Assert.Equal(CompetitionStatus.ORGANIZING, response.Status);
            Assert.Equal(createDTO.BreakInSeconds, response.BreakInSeconds);
            Assert.Equal(model.GameFormat.GameType, response.GameType);
            Assert.Equal(model.GameFormat.CompetitorType, response.CompetitorType);
            Assert.Equal(model.GameFormat.TeamSize, response.TeamSize);
            Assert.Equal(model.GameFormat.WinAt, response.WinAt);
            Assert.Equal(model.GameFormat.DurationInSeconds, response.DurationInSeconds);
        }

        [Fact]
        public async Task Test_CreateCompetitionHandler_TournamentCompetition_Success()
        {
            TournamentCompetition model = Builders.CreateBasicTournamentCompetition().Get();
            _unitOfWorkMock.Setup(o => o.CompetitionRepository).Returns(_competitionRepositoryMock.Object);
            _unitOfWorkMock.Setup(o => o.GameFormatRepository).Returns(_gameFormatRepositoryMock.Object);
            _gameFormatRepositoryMock.Setup(o => o.GetById(It.IsAny<Guid>())).Returns(Task.FromResult((GameFormat?)model.GameFormat));
            _competitionRepositoryMock.Setup(o => o.Create(It.IsAny<TournamentCompetition>())).Returns(Task.FromResult((Competition)model));
            _mapperMock.Setup(o => o.Map<TournamentCompetition>(It.IsAny<TournamentCompetitionCreateDTO>())).Returns(model);
            _mapperMock.Setup(o => o.Map<TournamentCompetitionResponseDTO>(It.IsAny<TournamentCompetition>()))
                .Returns(model.Adapt<TournamentCompetitionResponseDTO>());
            TournamentCompetitionCreateDTO createDTO = model.Adapt<TournamentCompetitionCreateDTO>();
            CreateCompetition command = new(createDTO);
            CreateCompetitionHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object);

            CompetitionGetDTO response = await handler.Handle(command, default);

            Assert.True(response is TournamentCompetitionResponseDTO);
            Assert.Equal(createDTO.Name, response.Name);
            Assert.Equal(createDTO.Location, response.Location);
            Assert.Equal(createDTO.StartTime, response.StartTime);
            Assert.Equal(CompetitionStatus.ORGANIZING, response.Status);
            Assert.Equal(createDTO.BreakInSeconds, response.BreakInSeconds);
            Assert.Equal(model.GameFormat.GameType, response.GameType);
            Assert.Equal(model.GameFormat.CompetitorType, response.CompetitorType);
            Assert.Equal(model.GameFormat.TeamSize, response.TeamSize);
            Assert.Equal(model.GameFormat.WinAt, response.WinAt);
            Assert.Equal(model.GameFormat.DurationInSeconds, response.DurationInSeconds);
            Assert.Equal(createDTO.StageLevel, ((TournamentCompetitionResponseDTO)response).StageLevel);
        }

        public static TheoryData<Competition> Status => new()
        {
            Builders.CreateBasicOneVSAllCompetition().SetStatus(CompetitionStatus.NOT_STARTED).Get(),
            Builders.CreateBasicOneVSAllCompetition().SetStatus(CompetitionStatus.STARTED).Get(),
            Builders.CreateBasicOneVSAllCompetition().SetStatus(CompetitionStatus.FINISHED).Get(),
            Builders.CreateBasicOneVSAllCompetition().SetStatus(CompetitionStatus.CANCELED).Get(),
            Builders.CreateBasicTournamentCompetition().SetStatus(CompetitionStatus.NOT_STARTED).Get(),
            Builders.CreateBasicTournamentCompetition().SetStatus(CompetitionStatus.STARTED).Get(),
            Builders.CreateBasicTournamentCompetition().SetStatus(CompetitionStatus.FINISHED).Get(),
            Builders.CreateBasicTournamentCompetition().SetStatus(CompetitionStatus.CANCELED).Get()
        };

        [Theory]
        [MemberData(nameof(Status))]
        public async Task Test_CreateCompetitionHandler_IllegalStatus_Throws_APIllegalStatusException(Competition model)
        {
            CompetitionCreateDTO createDTO = model is OneVSAllCompetition
                ? model.Adapt<OneVSAllCompetitionCreateDTO>()
                : model.Adapt<TournamentCompetitionCreateDTO>();
            CreateCompetition command = new(createDTO);
            CreateCompetitionHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object);

            await Assert.ThrowsAsync<APIllegalStatusException>(async () => await handler.Handle(command, default));
        }

        [Fact]
        public async Task Test_CreateCompetitionHandler_OneVSAllCompetition_Transaction_Throws_Exception()
        {
            OneVSAllCompetition model = Builders.CreateBasicOneVSAllCompetition().Get();
            _unitOfWorkMock.Setup(o => o.CompetitionRepository).Returns(_competitionRepositoryMock.Object);
            _unitOfWorkMock.Setup(o => o.GameFormatRepository).Returns(_gameFormatRepositoryMock.Object);
            _gameFormatRepositoryMock.Setup(o => o.GetById(It.IsAny<Guid>())).Returns(Task.FromResult((GameFormat?)model.GameFormat));
            _competitionRepositoryMock.Setup(o => o.Create(It.IsAny<OneVSAllCompetition>())).Throws<Exception>();
            _mapperMock.Setup(o => o.Map<OneVSAllCompetition>(It.IsAny<OneVSAllCompetitionCreateDTO>())).Returns(model);
            CompetitionCreateDTO createDTO = model.Adapt<OneVSAllCompetitionCreateDTO>();
            CreateCompetition command = new(createDTO);
            CreateCompetitionHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object);

            await Assert.ThrowsAsync<Exception>(async () => await handler.Handle(command, default));

            _unitOfWorkMock.Verify(o => o.RollbackTransactionAsync(), Times.Once);
        }

        [Fact]
        public async Task Test_CreateCompetitionHandler_TournamentCompetition_Transaction_Throws_Exception()
        {
            TournamentCompetition model = Builders.CreateBasicTournamentCompetition().Get();
            _unitOfWorkMock.Setup(o => o.CompetitionRepository).Returns(_competitionRepositoryMock.Object);
            _unitOfWorkMock.Setup(o => o.GameFormatRepository).Returns(_gameFormatRepositoryMock.Object);
            _gameFormatRepositoryMock.Setup(o => o.GetById(It.IsAny<Guid>())).Returns(Task.FromResult((GameFormat?)model.GameFormat));
            _competitionRepositoryMock.Setup(o => o.Create(It.IsAny<TournamentCompetition>())).Throws<Exception>();
            _mapperMock.Setup(o => o.Map<TournamentCompetition>(It.IsAny<TournamentCompetitionCreateDTO>())).Returns(model);
            CompetitionCreateDTO createDTO = model.Adapt<TournamentCompetitionCreateDTO>();
            CreateCompetition command = new(createDTO);
            CreateCompetitionHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object);

            await Assert.ThrowsAsync<Exception>(async () => await handler.Handle(command, default));

            _unitOfWorkMock.Verify(o => o.RollbackTransactionAsync(), Times.Once);
        }
    }
}
