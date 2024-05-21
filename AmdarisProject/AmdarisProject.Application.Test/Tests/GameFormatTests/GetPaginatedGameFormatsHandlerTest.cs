using AmdarisProject.Application.Common.Models;
using AmdarisProject.Application.Dtos.ResponseDTOs;
using AmdarisProject.Application.Handlers.GameFormatHandlers;
using AmdarisProject.Domain.Models;
using AmdarisProject.TestUtils.ModelBuilders;
using Moq;

namespace AmdarisProject.Application.Test.Tests.GameFormatTests
{
    public class GetPaginatedGameFormatsHandlerTest : ApplicationTestBase
    {
        [Fact]
        public async Task Test_GetPaginatedGameFormatsHandler_Success()
        {
            List<GameFormat> gameFormats = [];
            for (int i = 0; i < _numberOfModelsInAList; i++) gameFormats.Add(APBuilder.CreateBasicGameFormat().Get());
            _gameFormatRepositoryMock.Setup(o => o.GetPaginatedData(It.IsAny<PagedRequest>()))
                .Returns(Task.FromResult((IEnumerable<GameFormat>)gameFormats));
            _mapperMock.Setup(o => o.Map<IEnumerable<GameFormatGetDTO>>(It.IsAny<IEnumerable<GameFormat>>()))
                .Returns(_mapper.Map<IEnumerable<GameFormatGetDTO>>(gameFormats));
            GetPaginatedGameFormats command = new(_pagedRequest);
            GetPaginatedGameFormatsHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object,
                GetLogger<GetPaginatedGameFormatsHandler>());

            PaginatedResult<GameFormatGetDTO> response = await handler.Handle(command, default);

            _gameFormatRepositoryMock.Verify(o => o.GetPaginatedData(It.IsAny<PagedRequest>()), Times.Once);
            Assert.Equal(_pagedRequest.PageIndex, response.PageIndex);
            Assert.Equal(_pagedRequest.PageSize, response.PageSize);
            Assert.Equal(_pagedRequest.PageSize, response.Total);
            Assert.Equal(_pagedRequest.PageSize, response.Items.Count());
            for (int i = 0; i < gameFormats.Count; i++)
            {
                GameFormat gameFormat = gameFormats[i];
                GameFormatGetDTO gameFormatGetDTO = response.Items.ElementAt(i);

                Assert.Equal(gameFormat.Name, gameFormatGetDTO.Name);
                Assert.Equal(gameFormat.GameType.Id, gameFormatGetDTO.GameType.Id);
                Assert.Equal(gameFormat.GameType.Name, gameFormatGetDTO.GameType.Name);
                Assert.Equal(gameFormat.CompetitorType, gameFormatGetDTO.CompetitorType);
                Assert.Equal(gameFormat.TeamSize, gameFormatGetDTO.TeamSize);
                Assert.Equal(gameFormat.WinAt, gameFormatGetDTO.WinAt);
                Assert.Equal(gameFormat.DurationInMinutes, gameFormatGetDTO.DurationInMinutes);
            }
        }
    }
}
