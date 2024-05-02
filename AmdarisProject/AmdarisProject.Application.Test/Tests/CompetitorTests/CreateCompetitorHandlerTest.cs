using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Dtos.CreateDTOs;
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
            CompetitorCreateDTO createDTO = model.Adapt<CompetitorCreateDTO>();
            _unitOfWorkMock.Setup(o => o.CompetitorRepository).Returns(_competitorRepositoryMock.Object);
            _competitorRepositoryMock.Setup(o => o.Create(It.IsAny<Player>())).Returns(Task.FromResult((Competitor)model));
            _mapperMock.Setup(o => o.Map<Player>(It.IsAny<CompetitorCreateDTO>())).Returns(model);
            _mapperMock.Setup(o => o.Map<PlayerGetDTO>(It.IsAny<Player>())).Returns(model.Adapt<PlayerGetDTO>());
            CreateTeam command = new(createDTO);
            CreateTeamHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object);

            CompetitorGetDTO response = await handler.Handle(command, default);

            Assert.True(response is PlayerGetDTO);
            Assert.Equal(createDTO.Name, response.Name);
        }

        [Fact]
        public async Task Test_CreateCompetitorHandler_Team_Success()
        {
            Team model = Builders.CreateBasicTeam().Get();
            CompetitorCreateDTO createDTO = Builders.CreateBasicTeam().Get().Adapt<CompetitorCreateDTO>();
            _unitOfWorkMock.Setup(o => o.CompetitorRepository).Returns(_competitorRepositoryMock.Object);
            _competitorRepositoryMock.Setup(o => o.Create(It.IsAny<Team>())).Returns(Task.FromResult((Competitor)model));
            _mapperMock.Setup(o => o.Map<Team>(It.IsAny<CompetitorCreateDTO>())).Returns(model);
            _mapperMock.Setup(o => o.Map<TeamGetDTO>(It.IsAny<Team>())).Returns(model.Adapt<TeamGetDTO>());
            CreateTeam command = new(createDTO);
            CreateTeamHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object);

            CompetitorGetDTO response = await handler.Handle(command, default);

            Assert.True(response is TeamGetDTO);
            Assert.Equal(createDTO.Name, response.Name);
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
            CompetitorCreateDTO createDTO = model.Adapt<CompetitorCreateDTO>();
            CreateTeam command = new(createDTO);
            CreateTeamHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object);

            await Assert.ThrowsAsync<Exception>(async () => await handler.Handle(command, default));

            _unitOfWorkMock.Verify(o => o.RollbackTransactionAsync(), Times.Once);
        }
    }
}
