using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Dtos.ResponseDTOs;
using AmdarisProject.Application.Handlers.GameFormatHandlers;
using AmdarisProject.Domain.Models;
using AmdarisProject.Presentation;
using MapsterMapper;
using Moq;

namespace AmdarisProject.Application.Test.Tests.GameFormatTests
{
    public class GetAllGameFormatsHandlerTest
    {
        private readonly Mock<IGameFormatRepository> _gameFormatRepository = new();
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly Mock<IMapper> _mapperMock = new();

        public GetAllGameFormatsHandlerTest() => MapsterConfiguration.ConfigureMapster();

        [Fact]
        public async Task Test_GetAllGameFormatsHandler_Success()
        {
            _unitOfWorkMock.Setup(o => o.GameFormatRepository).Returns(_gameFormatRepository.Object);
            _gameFormatRepository.Setup(o => o.GetAll()).Returns(Task.FromResult((IEnumerable<GameFormat>)[]));
            GetAllGameFormats command = new();
            GetAllGameFormatsHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object);

            IEnumerable<GameFormatGetDTO> response = await handler.Handle(command, default);

            _gameFormatRepository.Verify(o => o.GetAll(), Times.Once);
        }
    }
}
