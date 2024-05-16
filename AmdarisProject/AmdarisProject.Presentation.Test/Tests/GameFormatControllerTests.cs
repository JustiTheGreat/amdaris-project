using AmdarisProject.Application.Dtos.RequestDTOs.CreateDTOs;
using AmdarisProject.Application.Dtos.ResponseDTOs;
using AmdarisProject.Application.Handlers.GameFormatHandlers;
using AmdarisProject.Application.Test.ModelBuilders;
using AmdarisProject.Domain.Models;
using AmdarisProject.Presentation.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace AmdarisProject.Presentation.Test.Tests
{
    public class GameFormatControllerTests : PresentationTestBase<GameFormatController>
    {
        [Fact]
        public async Task Test_CreateGameFormat_OkStatus()
        {
            //Setup<CreateGameFormat, GameFormatGetDTO, CreateGameFormatHandler>();
            //GameFormat gameFormat = APBuilder.CreateBasicGameFormat().Get();

            //var requestResult = await _controller.CreateGameFormat(gameFormat.Adapt<GameFormatCreateDTO>());

            //var result = requestResult as OkObjectResult;
            //var response = result?.Value as GameFormatGetDTO;
            //Assert.Equal((int)HttpStatusCode.OK, result!.StatusCode);
            //Assert.NotNull(response);
            //Assert.Equal(gameFormat.Name, response.Name);
            //Assert.Equal(gameFormat.GameType, response.GameType);
            //Assert.Equal(gameFormat.CompetitorType, response.CompetitorType);
            //Assert.Equal(gameFormat.TeamSize, response.TeamSize);
            //Assert.Equal(gameFormat.WinAt, response.WinAt);
            //Assert.Equal(gameFormat.DurationInMinutes, response.DurationInMinutes);
        }

        [Fact]
        public async Task Test_GetAllGameFormats_OkStatus()
        {
            //Setup<CreateGameFormat, GameFormatGetDTO, CreateGameFormatHandler>();
            //Seed_GetAllGameFormats(out List<GameFormat> gameFormats);

            //var requestResult = await _controller.GetAllGameFormats();

            //var result = requestResult as OkObjectResult;
            //var response = result?.Value as IEnumerable<GameFormatGetDTO>;
            //Assert.Equal((int)HttpStatusCode.OK, result!.StatusCode);
            //Assert.NotNull(response);
            //Assert.Equal(gameFormats.Count, response.Count());
            //for (int i = 0; i < gameFormats.Count; i++)
            //{
            //    GameFormat gameFormat = gameFormats[i];
            //    GameFormatGetDTO gameFormatGetDTO = response.ElementAt(i);

            //    Assert.Equal(gameFormat.Id, gameFormatGetDTO.Id);
            //    Assert.Equal(gameFormat.Name, gameFormatGetDTO.Name);
            //    Assert.Equal(gameFormat.GameType, gameFormatGetDTO.GameType);
            //    Assert.Equal(gameFormat.CompetitorType, gameFormatGetDTO.CompetitorType);
            //    Assert.Equal(gameFormat.TeamSize, gameFormatGetDTO.TeamSize);
            //    Assert.Equal(gameFormat.WinAt, gameFormatGetDTO.WinAt);
            //    Assert.Equal(gameFormat.DurationInMinutes, gameFormatGetDTO.DurationInMinutes);
            //}
        }

        private void Seed_GetAllGameFormats(out List<GameFormat> gameFormats)
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
