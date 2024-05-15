using AmdarisProject.Application.Dtos.ResponseDTOs;
using AmdarisProject.Application.Handlers.GameFormatHandlers;
using AmdarisProject.Application.Test.ModelBuilders;
using AmdarisProject.Domain.Models;
using Mapster;
using Moq;

namespace AmdarisProject.Application.Test.Tests.GameFormatTests
{
    public class GetAllGameFormatsHandlerTest : MockObjectUser
    {
        [Fact]
        public async Task Test_GetAllGameFormatsHandler_Success()
        {
            List<GameFormat> gameFormats = [];
            for (int i = 0; i < _numberOfModelsInAList; i++) gameFormats.Add(APBuilder.CreateBasicGameFormat().Get());
            _unitOfWorkMock.Setup(o => o.GameFormatRepository).Returns(_gameFormatRepositoryMock.Object);
            _gameFormatRepositoryMock.Setup(o => o.GetAll()).Returns(Task.FromResult((IEnumerable<GameFormat>)gameFormats));
            _mapperMock.Setup(o => o.Map<IEnumerable<GameFormatGetDTO>>(It.IsAny<IEnumerable<GameFormat>>()))
                .Returns(gameFormats.Adapt<IEnumerable<GameFormatGetDTO>>());
            GetAllGameFormats command = new();
            GetPagedGameFormatsHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object, GetLogger<GetPagedGameFormatsHandler>());

            IEnumerable<GameFormatGetDTO> response = await handler.Handle(command, default);

            _gameFormatRepositoryMock.Verify(o => o.GetAll(), Times.Once);
            Assert.Equal(gameFormats.Count, response.Count());
            for (int i = 0; i < gameFormats.Count; i++)
            {
                GameFormat gameFormat = gameFormats[i];
                GameFormatGetDTO gameFormatGetDTO = response.ElementAt(i);

                Assert.Equal(gameFormat.Name, gameFormatGetDTO.Name);
                Assert.Equal(gameFormat.GameType, gameFormatGetDTO.GameType);
                Assert.Equal(gameFormat.CompetitorType, gameFormatGetDTO.CompetitorType);
                Assert.Equal(gameFormat.TeamSize, gameFormatGetDTO.TeamSize);
                Assert.Equal(gameFormat.WinAt, gameFormatGetDTO.WinAt);
                Assert.Equal(gameFormat.DurationInMinutes, gameFormatGetDTO.DurationInMinutes);
            }
        }
    }
}
