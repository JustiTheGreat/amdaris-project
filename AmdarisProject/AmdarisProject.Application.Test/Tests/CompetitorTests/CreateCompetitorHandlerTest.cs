using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Dtos.CreateDTOs.CompetitorCreateDTOs;
using AmdarisProject.Application.Dtos.ResponseDTOs.CompetitorResponseDTOs;
using AmdarisProject.Application.Handlers.CompetitorHandlers;
using AmdarisProject.Application.Test.ModelBuilder;
using AmdarisProject.Domain.Models.CompetitorModels;
using AmdarisProject.Presentation;
using Mapster;
using MapsterMapper;
using Moq;

namespace AmdarisProject.Application.Test.Tests.CompetitorTests
{
    public class CreateCompetitorHandlerTest
    {
        private readonly Mock<ICompetitorRepository> _competitorRepositoryMock = new();
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly Mock<IMapper> _mapperMock = new();

        public CreateCompetitorHandlerTest() => MapsterConfiguration.ConfigureMapster();

        [Fact]
        public async Task Test_CreateCompetitorHandler_Player_Success()
        {
            Player model = Builders.CreateBasicPlayer().Get();
            PlayerCreateDTO createDTO = model.Adapt<PlayerCreateDTO>();
            _unitOfWorkMock.Setup(o => o.CompetitorRepository).Returns(_competitorRepositoryMock.Object);
            _competitorRepositoryMock.Setup(o => o.Create(It.IsAny<Player>())).Returns(Task.FromResult((Competitor)model));
            _mapperMock.Setup(o => o.Map<Player>(It.IsAny<PlayerCreateDTO>())).Returns(model);
            _mapperMock.Setup(o => o.Map<PlayerResponseDTO>(It.IsAny<Player>())).Returns(model.Adapt<PlayerResponseDTO>());
            CreateCompetitor command = new(createDTO);
            CreateCompetitorHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object);

            CompetitorResponseDTO response = await handler.Handle(command, default);

            Assert.True(response is PlayerResponseDTO);
            Assert.Equal(createDTO.Name, response.Name);
        }

        [Fact]
        public async Task Test_CreateCompetitorHandler_Team_Success()
        {
            Team model = Builders.CreateBasicTeam().Get();
            TeamCreateDTO createDTO = Builders.CreateBasicTeam().Get().Adapt<TeamCreateDTO>();
            _unitOfWorkMock.Setup(o => o.CompetitorRepository).Returns(_competitorRepositoryMock.Object);
            _competitorRepositoryMock.Setup(o => o.Create(It.IsAny<Team>())).Returns(Task.FromResult((Competitor)model));
            _mapperMock.Setup(o => o.Map<Team>(It.IsAny<TeamCreateDTO>())).Returns(model);
            _mapperMock.Setup(o => o.Map<TeamResponseDTO>(It.IsAny<Team>())).Returns(model.Adapt<TeamResponseDTO>());
            CreateCompetitor command = new(createDTO);
            CreateCompetitorHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object);

            CompetitorResponseDTO response = await handler.Handle(command, default);

            Assert.True(response is TeamResponseDTO);
            Assert.Equal(createDTO.Name, response.Name);
            Assert.Equal(createDTO.TeamSize, ((TeamResponseDTO)response).TeamSize);
        }

        //[Fact]
        //public async Task Test_CreateCompetitorHandler_ForTeamWith0TeamSize_Throws_APArgumentException()
        //{
        //    TeamCreateDTO dtoData = Builders.CreateBasicTeam().SetTeamSize(0).Get().Adapt<TeamCreateDTO>();
        //    CreateCompetitor command = new(dtoData);

        //    CreateCompetitorHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object);

        //    await Assert.ThrowsAsync<APArgumentException>(async () => await handler.Handle(command, default));
        //}

        public static TheoryData<Competitor> Transaction => new()
        {
            Builders.CreateBasicPlayer().Get(),
            Builders.CreateBasicTeam().Get()
        };


        [Theory]
        [MemberData(nameof(Transaction))]
        public async Task Test_CreateCompetitorHandler_Transaction_Throws_Exception(Competitor model)
        {
            _unitOfWorkMock.Setup(o => o.CompetitorRepository).Returns(_competitorRepositoryMock.Object);
            _competitorRepositoryMock.Setup(o => o.Create(It.IsAny<Competitor>())).Throws<Exception>();
            CompetitorCreateDTO createDTO = model is Player
                ? model.Adapt<PlayerCreateDTO>()
                : model.Adapt<TeamCreateDTO>();
            CreateCompetitor command = new(createDTO);
            CreateCompetitorHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object);

            await Assert.ThrowsAsync<Exception>(async () => await handler.Handle(command, default));

            _unitOfWorkMock.Verify(o => o.RollbackTransactionAsync(), Times.Once);
        }
    }
}
