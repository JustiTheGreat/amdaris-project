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
            List<GameFormat> models = [];
            for (int i = 0; i < _numberOfModelsInAList; i++) models.Add(APBuilder.CreateBasicGameFormat().Get());
            IEnumerable<GameFormatGetDTO> dtos = models.Adapt<IEnumerable<GameFormatGetDTO>>();
            _unitOfWorkMock.Setup(o => o.GameFormatRepository).Returns(_gameFormatRepositoryMock.Object);
            _gameFormatRepositoryMock.Setup(o => o.GetAll()).Returns(Task.FromResult((IEnumerable<GameFormat>)models));
            _mapperMock.Setup(o => o.Map<IEnumerable<GameFormatGetDTO>>(It.IsAny<IEnumerable<GameFormat>>())).Returns(dtos);
            GetAllGameFormats command = new();
            GetAllGameFormatsHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object, GetLogger<GetAllGameFormatsHandler>());

            IEnumerable<GameFormatGetDTO> response = await handler.Handle(command, default);

            _gameFormatRepositoryMock.Verify(o => o.GetAll(), Times.Once);
            for (int i = 0; i < _numberOfModelsInAList; i++)
            {
                Assert.Equal(models.ElementAt(i).Name, response.ElementAt(i).Name);
                Assert.Equal(models.ElementAt(i).GameType, response.ElementAt(i).GameType);
                Assert.Equal(models.ElementAt(i).CompetitorType, response.ElementAt(i).CompetitorType);
                Assert.Equal(models.ElementAt(i).TeamSize, response.ElementAt(i).TeamSize);
                Assert.Equal(models.ElementAt(i).WinAt, response.ElementAt(i).WinAt);
                Assert.Equal(models.ElementAt(i).DurationInMinutes, response.ElementAt(i).DurationInMinutes);
            }
        }
    }
}
