using AmdarisProject.Application.Dtos.ResponseDTOs;
using AmdarisProject.Application.Handlers.CompetitorHandlers;
using AmdarisProject.Application.Handlers.GameFormatHandlers;
using AmdarisProject.Application.Test.ModelBuilder;
using AmdarisProject.Domain.Models;
using Mapster;
using Microsoft.Extensions.Logging;
using Moq;

namespace AmdarisProject.Application.Test.Tests.GameFormatTests
{
    public class GetAllGameFormatsHandlerTest : MockObjectUser
    {
        [Fact]
        public async Task Test_GetAllGameFormatsHandler_Success()
        {
            List<GameFormat> models = [];
            for (int i = 0; i < NumberOfModelsInAList; i++) models.Add(Builders.CreateBasicGameFormat().Get());
            IEnumerable<GameFormatGetDTO> dtos = models.Adapt<IEnumerable<GameFormatGetDTO>>();
            _unitOfWorkMock.Setup(o => o.GameFormatRepository).Returns(_gameFormatRepositoryMock.Object);
            _gameFormatRepositoryMock.Setup(o => o.GetAll()).Returns(Task.FromResult((IEnumerable<GameFormat>)models));
            _mapperMock.Setup(o => o.Map<IEnumerable<GameFormatGetDTO>>(It.IsAny<IEnumerable<GameFormat>>())).Returns(dtos);
            GetAllGameFormats command = new();
            GetAllGameFormatsHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object,
                It.IsAny<ILogger<GetAllGameFormatsHandler>>());

            IEnumerable<GameFormatGetDTO> response = await handler.Handle(command, default);

            _gameFormatRepositoryMock.Verify(o => o.GetAll(), Times.Once);
            for (int i = 0; i < NumberOfModelsInAList; i++)
            {
                Assert.Equal(response.ElementAt(i).Name, models.ElementAt(i).Name);
                Assert.Equal(response.ElementAt(i).GameType, models.ElementAt(i).GameType);
                Assert.Equal(response.ElementAt(i).CompetitorType, models.ElementAt(i).CompetitorType);
                Assert.Equal(response.ElementAt(i).TeamSize, models.ElementAt(i).TeamSize);
                Assert.Equal(response.ElementAt(i).WinAt, models.ElementAt(i).WinAt);
                Assert.Equal(response.ElementAt(i).DurationInMinutes, models.ElementAt(i).DurationInMinutes);
            }
        }
    }
}
