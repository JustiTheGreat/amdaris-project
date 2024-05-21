using AmdarisProject.Application.Common.Models;
using AmdarisProject.Application.Dtos.RequestDTOs.CreateDTOs;
using AmdarisProject.Application.Dtos.ResponseDTOs;
using AmdarisProject.Application.Handlers.GameFormatHandlers;
using AmdarisProject.Domain.Models;
using AmdarisProject.Presentation.Controllers;
using AmdarisProject.TestUtils.ModelBuilders;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace AmdarisProject.Presentation.Test.Tests
{
    public class GameFormatControllerTests : PresentationTestBase<GameFormatController>
    {
        [Fact]
        public async Task Test_CreateGameFormat_CreatedStatus()
        {
            Setup<CreateGameFormat, GameFormatGetDTO, CreateGameFormatHandler>();
            Seed_CreateGameFormat(out GameFormat gameFormat);

            var requestResult = await _controller.CreateGameFormat(_mapper.Map<GameFormatCreateDTO>(gameFormat));

            var result = requestResult as CreatedResult;
            Assert.NotNull(result);
        }

        private void Seed_CreateGameFormat(out GameFormat gameFormat)
        {
            GameType gameType = APBuilder.CreateBasicGameType().Get();
            _dbContext.Add(gameType);
            _dbContext.SaveChanges();
            gameFormat = APBuilder.CreateBasicGameFormat().SetGameType(gameType).Get();
        }

        [Fact]
        public async Task Test_GetPaginatedGameFormats_OkStatus()
        {
            Setup<GetPaginatedGameFormats, PaginatedResult<GameFormatGetDTO>, GetPaginatedGameFormatsHandler>();
            Seed_GetPaginatedGameFormats(out List<GameFormat> gameFormats);

            var requestResult = await _controller.GetPaginatedGameFormats(_pagedRequest);

            var result = requestResult as OkObjectResult;
            var response = result?.Value as PaginatedResult<GameFormatGetDTO>;
            Assert.Equal((int)HttpStatusCode.OK, result!.StatusCode);
            Assert.NotNull(response);
            Assert.Equal(_pagedRequest.PageIndex, response.PageIndex);
            Assert.Equal(_pagedRequest.PageSize, response.PageSize);
            Assert.Equal(_pagedRequest.PageSize, response.Total);
            Assert.Equal(_pagedRequest.PageSize, response.Items.Count());
            gameFormats.ForEach(gameFormat =>
            {
                GameFormatGetDTO gameFormatGetDTO =
                    response.Items.First(gameFormatGetDTO => gameFormatGetDTO.Id.Equals(gameFormat.Id));

                Assert.Equal(gameFormat.Id, gameFormatGetDTO.Id);
                Assert.Equal(gameFormat.Name, gameFormatGetDTO.Name);
                Assert.Equal(gameFormat.GameType.Id, gameFormatGetDTO.GameType.Id);
                Assert.Equal(gameFormat.GameType.Name, gameFormatGetDTO.GameType.Name);
                Assert.Equal(gameFormat.CompetitorType, gameFormatGetDTO.CompetitorType);
                Assert.Equal(gameFormat.TeamSize, gameFormatGetDTO.TeamSize);
                Assert.Equal(gameFormat.WinAt, gameFormatGetDTO.WinAt);
                Assert.Equal(gameFormat.DurationInMinutes, gameFormatGetDTO.DurationInMinutes);
            });
        }

        private void Seed_GetPaginatedGameFormats(out List<GameFormat> gameFormats)
        {
            gameFormats = [];
            for (int i = 0; i < _numberOfModelsInAList; i++)
            {
                GameFormat gameFormat = APBuilder.CreateBasicGameFormat().Get();
                _dbContext.Add(gameFormat);
                gameFormats.Add(gameFormat);
            }
            _dbContext.SaveChanges();
            gameFormats.ForEach(Detach);
        }
    }
}
