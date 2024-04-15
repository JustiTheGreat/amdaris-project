using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Dtos.CreateDTOs.CompetitionCreateDTOs;
using AmdarisProject.Application.Dtos.ResponseDTOs.CompetitionResponseDTOs;
using AmdarisProject.Application.Handlers.CompetitionHandlers;
using AmdarisProject.Application.Test.ModelBuilder;
using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models.CompetitionModels;
using AmdarisProject.Presentation;
using Mapster;
using MapsterMapper;
using Moq;

namespace AmdarisProject.Application.Test
{
    public class CreateCompetitionHandlerTest
    {
        private readonly Mock<ICompetitionRepository> _competitionRepositoryMock = new();
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly Mock<IMapper> _mapperMock = new();

        public CreateCompetitionHandlerTest() => MapsterConfiguration.ConfigureMapster();

        [Fact]
        public async Task Test_CreateCompetitionHandler_OneVSAllCompetition_Success()
        {
            OneVSAllCompetition model = Builders.CreateBasicOneVSAllCompetition().Get();
            _unitOfWorkMock.Setup(o => o.CompetitionRepository).Returns(_competitionRepositoryMock.Object);
            _competitionRepositoryMock.Setup(o => o.Create(It.IsAny<OneVSAllCompetition>())).Returns(Task.FromResult((Competition)model));
            _mapperMock.Setup(o => o.Map<OneVSAllCompetition>(It.IsAny<OneVSAllCompetitionCreateDTO>()))
                .Returns(model);
            _mapperMock.Setup(o => o.Map<OneVSAllCompetitionResponseDTO>(It.IsAny<OneVSAllCompetition>()))
                .Returns(model.Adapt<OneVSAllCompetitionResponseDTO>());
            OneVSAllCompetitionCreateDTO createDTO = model.Adapt<OneVSAllCompetitionCreateDTO>();
            CreateCompetition command = new(createDTO);
            CreateCompetitionHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object);

            CompetitionResponseDTO response = await handler.Handle(command, default);

            Assert.True(response is OneVSAllCompetitionResponseDTO);
            Assert.Equal(createDTO.Name, response.Name);
            Assert.Equal(createDTO.Location, response.Location);
            Assert.Equal(createDTO.StartTime, response.StartTime);
            Assert.Equal(createDTO.Status, response.Status);
            Assert.Equal(createDTO.WinAt, response.WinAt);
            Assert.Equal(createDTO.DurationInSeconds, response.DurationInSeconds);
            Assert.Equal(createDTO.BreakInSeconds, response.BreakInSeconds);
            Assert.Equal(createDTO.GameType, response.GameType);
            Assert.Equal(createDTO.CompetitorType, response.CompetitorType);
            Assert.Equal(createDTO.TeamSize, response.TeamSize);
        }

        [Fact]
        public async Task Test_CreateCompetitionHandler_TournamentCompetition_Success()
        {
            TournamentCompetition model = Builders.CreateBasicTournamentCompetition().Get();
            _unitOfWorkMock.Setup(o => o.CompetitionRepository).Returns(_competitionRepositoryMock.Object);
            _competitionRepositoryMock.Setup(o => o.Create(It.IsAny<TournamentCompetition>())).Returns(Task.FromResult((Competition)model));
            _mapperMock.Setup(o => o.Map<TournamentCompetition>(It.IsAny<TournamentCompetitionCreateDTO>())).Returns(model);
            _mapperMock.Setup(o => o.Map<TournamentCompetitionResponseDTO>(It.IsAny<TournamentCompetition>()))
                .Returns(model.Adapt<TournamentCompetitionResponseDTO>());
            TournamentCompetitionCreateDTO createDTO = model.Adapt<TournamentCompetitionCreateDTO>();
            CreateCompetition command = new(createDTO);
            CreateCompetitionHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object);

            CompetitionResponseDTO response = await handler.Handle(command, default);

            Assert.True(response is TournamentCompetitionResponseDTO);
            Assert.Equal(createDTO.Name, response.Name);
            Assert.Equal(createDTO.Location, response.Location);
            Assert.Equal(createDTO.StartTime, response.StartTime);
            Assert.Equal(createDTO.Status, response.Status);
            Assert.Equal(createDTO.WinAt, response.WinAt);
            Assert.Equal(createDTO.DurationInSeconds, response.DurationInSeconds);
            Assert.Equal(createDTO.BreakInSeconds, response.BreakInSeconds);
            Assert.Equal(createDTO.GameType, response.GameType);
            Assert.Equal(createDTO.CompetitorType, response.CompetitorType);
            Assert.Equal(createDTO.TeamSize, response.TeamSize);
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

        public static TheoryData<Competition> WinConditions => new()
        {
            Builders.CreateBasicOneVSAllCompetition().SetWinAt(null).SetDurationInSeconds(null).SetBreakInSeconds(null).Get(),
            Builders.CreateBasicOneVSAllCompetition().SetWinAt(null).SetDurationInSeconds(null).Get(),
            Builders.CreateBasicOneVSAllCompetition().SetWinAt(null).SetBreakInSeconds(null).Get(),
            Builders.CreateBasicOneVSAllCompetition().SetDurationInSeconds(null).Get(),
            Builders.CreateBasicOneVSAllCompetition().SetBreakInSeconds(null).Get(),
            Builders.CreateBasicTournamentCompetition().SetWinAt(null).SetDurationInSeconds(null).SetBreakInSeconds(null).Get(),
            Builders.CreateBasicTournamentCompetition().SetWinAt(null).SetDurationInSeconds(null).Get(),
            Builders.CreateBasicTournamentCompetition().SetWinAt(null).SetBreakInSeconds(null).Get(),
            Builders.CreateBasicTournamentCompetition().SetDurationInSeconds(null).Get(),
            Builders.CreateBasicTournamentCompetition().SetBreakInSeconds(null).Get(),
        };

        public static TheoryData<Competition> CompetitorTypeAndTeamSize => new()
        {
            Builders.CreateBasicOneVSAllCompetition().SetTeamSize(2).Get(),
            Builders.CreateBasicOneVSAllCompetition().SetCompetitorType(CompetitorType.TEAM).Get(),
            Builders.CreateBasicOneVSAllCompetition().SetCompetitorType(CompetitorType.TEAM).SetTeamSize(1).Get(),
            Builders.CreateBasicTournamentCompetition().SetTeamSize(2).Get(),
            Builders.CreateBasicTournamentCompetition().SetCompetitorType(CompetitorType.TEAM).Get(),
            Builders.CreateBasicTournamentCompetition().SetCompetitorType(CompetitorType.TEAM).SetTeamSize(1).Get(),
        };

        [Theory]
        [MemberData(nameof(WinConditions))]
        [MemberData(nameof(CompetitorTypeAndTeamSize))]
        public async Task Test_CreateCompetitionHandler_IllegalWinConditions_Throws_APArgumentException(Competition model)
        {
            CompetitionCreateDTO createDTO = model is OneVSAllCompetition
                ? model.Adapt<OneVSAllCompetitionCreateDTO>()
                : model.Adapt<TournamentCompetitionCreateDTO>();
            CreateCompetition command = new(createDTO);
            CreateCompetitionHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object);

            await Assert.ThrowsAsync<APArgumentException>(async () => await handler.Handle(command, default));
        }

        public static TheoryData<Competition> Transaction => new()
        {
            Builders.CreateBasicOneVSAllCompetition().Get(),
            Builders.CreateBasicTournamentCompetition().Get()
        };


        [Theory]
        [MemberData(nameof(Transaction))]
        public async Task Test_CreateCompetitionHandler_Transaction_Throws_Exception(Competition model)
        {
            _unitOfWorkMock.Setup(o => o.CompetitionRepository).Returns(_competitionRepositoryMock.Object);
            _competitionRepositoryMock.Setup(o => o.Create(It.IsAny<Competition>())).Throws<Exception>();
            CompetitionCreateDTO createDTO = model is OneVSAllCompetition
                ? model.Adapt<OneVSAllCompetitionCreateDTO>()
                : model.Adapt<TournamentCompetitionCreateDTO>();
            CreateCompetition command = new(createDTO);
            CreateCompetitionHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object);

            await Assert.ThrowsAsync<Exception>(async () => await handler.Handle(command, default));

            _unitOfWorkMock.Verify(o => o.RollbackTransactionAsync(), Times.Once);
        }
    }
}
