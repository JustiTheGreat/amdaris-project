using AmdarisProject.Application.Common.Models;
using AmdarisProject.Application.Dtos.ResponseDTOs.GetDTOs;
using AmdarisProject.Application.Handlers.GameTypesHandlers;
using AmdarisProject.Domain.Models;
using AmdarisProject.Presentation.Controllers;
using AmdarisProject.TestUtils.ModelBuilders;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace AmdarisProject.Presentation.Test.Tests
{
    public class GameTypeControllerTests : PresentationTestBase<GameTypeController>
    {
        [Fact]
        public async Task Test_GetPaginatedGameTypes_OkStatus()
        {
            Setup<GetPaginatedGameTypes, PaginatedResult<GameTypeGetDTO>, GetPaginatedGameTypesHandler>();
            Seed_GetPaginatedGameTypes(out List<GameType> gameTypes);

            var requestResult = await _controller.GetPaginatedGameTypes(_pagedRequest);

            var result = requestResult as OkObjectResult;
            var response = result?.Value as PaginatedResult<GameTypeGetDTO>;
            Assert.Equal((int)HttpStatusCode.OK, result!.StatusCode);
            Assert.NotNull(response);
            Assert.Equal(_pagedRequest.PageIndex, response.PageIndex);
            Assert.Equal(_pagedRequest.PageSize, response.PageSize);
            Assert.Equal(_pagedRequest.PageSize, response.Total);
            Assert.Equal(_pagedRequest.PageSize, response.Items.Count());
            gameTypes.ForEach(gameFormat =>
            {
                GameTypeGetDTO gameTypeGetDTO =
                    response.Items.First(gameTypeGetDTO => gameTypeGetDTO.Id.Equals(gameFormat.Id));

                Assert.Equal(gameFormat.Id, gameTypeGetDTO.Id);
                Assert.Equal(gameFormat.Name, gameTypeGetDTO.Name);
            });
        }

        private void Seed_GetPaginatedGameTypes(out List<GameType> gameTypes)
        {
            gameTypes = [];
            for (int i = 0; i < _numberOfModelsInAList; i++)
            {
                GameType gameType = APBuilder.CreateBasicGameType().Get();
                _dbContext.Add(gameType);
                gameTypes.Add(gameType);
            }
            _dbContext.SaveChanges();
            gameTypes.ForEach(Detach);
        }
    }
}
