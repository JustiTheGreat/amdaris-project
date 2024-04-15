using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Dtos.ResponseDTOs.CompetitorResponseDTOs;
using AmdarisProject.Application.Handlers.CompetitorHandlers;
using AmdarisProject.Domain.Models.CompetitorModels;
using AmdarisProject.Presentation;
using MapsterMapper;
using Moq;

namespace AmdarisProject.Application.Test
{
    public class GetPlayersHandlerTest
    {
        private readonly Mock<ICompetitorRepository> _competitorRepositoryMock = new();
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly Mock<IMapper> _mapperMock = new();

        public GetPlayersHandlerTest() => MapsterConfiguration.ConfigureMapster();

        [Fact]
        public async Task Test_CreateCompetitorHandler_Player_Success()
        {
            //ushort numberOfPlayers = 5;
            //List<Player> players = [];
            //for (int i = 0; i < numberOfPlayers; i++) players.Add(Builders.CreateBasicPlayer().Get());
            _unitOfWorkMock.Setup(o => o.CompetitorRepository).Returns(_competitorRepositoryMock.Object);
            _competitorRepositoryMock.Setup(o => o.GetAllPlayers()).Returns(Task.FromResult((IEnumerable<Player>)[]));
            //Moq.Language.ISetupSequentialResult<PlayerResponseDTO> mapperSetup =
            //    _mapperMock.SetupSequence(o => o.Map<PlayerResponseDTO>(It.IsAny<Player>()));
            //for (int i = 0; i < numberOfPlayers; i++) mapperSetup.Returns(players.ElementAt(i).Adapt<PlayerResponseDTO>());
            GetPlayers command = new();
            GetPlayersHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object);

            IEnumerable<PlayerResponseDTO> response = await handler.Handle(command, default);

            _competitorRepositoryMock.Verify(o => o.GetAllPlayers(), Times.Once);

            //for (int i = 0; i < numberOfPlayers; i++)
            //{
            //    Assert.Equal(players.ElementAt(i).Id, response.ElementAt(i).Id);
            //    Assert.Equal(players.ElementAt(i).Name, response.ElementAt(i).Name);
            //}
        }
    }
}
