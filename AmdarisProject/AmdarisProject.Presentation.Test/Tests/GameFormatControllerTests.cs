using AmdarisProject.Application.Dtos.CreateDTOs;
using AmdarisProject.Application.Dtos.ResponseDTOs;
using AmdarisProject.Application.Handlers.GameFormatHandlers;
using AmdarisProject.Application.Test.ModelBuilders;
using AmdarisProject.Domain.Models;
using AmdarisProject.Presentation.Controllers;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace AmdarisProject.Presentation.Test.Tests
{
    public class GameFormatControllerTests : ControllerTests<GameFormatController>
    {
        [Fact]
        public async Task Test_CreateGameFormat_Success()
        {
            Setup<CreateGameFormat, GameFormatGetDTO, CreateGameFormatHandler>();
            GameFormatCreateDTO dto = APBuilder.CreateBasicGameFormat().Get().Adapt<GameFormatCreateDTO>();

            var requestResult = await _controller.CreateGameFormat(dto);

            var result = requestResult as OkObjectResult;
            var response = result?.Value as GameFormatGetDTO;
            Assert.Equal((int)HttpStatusCode.OK, result!.StatusCode);
            Assert.NotNull(response);
            Assert.Equal(dto.Name, response.Name);
            Assert.Equal(dto.GameType, response.GameType);
            Assert.Equal(dto.CompetitorType, response.CompetitorType);
            Assert.Equal(dto.TeamSize, response.TeamSize);
            Assert.Equal(dto.WinAt, response.WinAt);
            Assert.Equal(dto.DurationInMinutes, response.DurationInMinutes);
        }

        [Fact]
        public async Task Test_GetAllGameFormats_Success()
        {
            Setup<CreateGameFormat, GameFormatGetDTO, CreateGameFormatHandler>();
            Seed_GetAllGameFormats(out List<GameFormat> models);

            var requestResult = await _controller.GetAllGameFormats();

            var result = requestResult as OkObjectResult;
            var response = result?.Value as IEnumerable<GameFormatGetDTO>;
            Assert.Equal((int)HttpStatusCode.OK, result!.StatusCode);
            Assert.NotNull(response);
            for (int i = 0; i < _numberOfModelsInAList; i++)
            {
                Assert.Equal(models.ElementAt(i).Id, response.ElementAt(i).Id);
                Assert.Equal(models.ElementAt(i).Name, response.ElementAt(i).Name);
                Assert.Equal(models.ElementAt(i).GameType, response.ElementAt(i).GameType);
                Assert.Equal(models.ElementAt(i).CompetitorType, response.ElementAt(i).CompetitorType);
                Assert.Equal(models.ElementAt(i).TeamSize, response.ElementAt(i).TeamSize);
                Assert.Equal(models.ElementAt(i).WinAt, response.ElementAt(i).WinAt);
                Assert.Equal(models.ElementAt(i).DurationInMinutes, response.ElementAt(i).DurationInMinutes);
            }
        }

        private void Seed_GetAllGameFormats(out List<GameFormat> models)
        {
            models = [];
            for (int i = 0; i < _numberOfModelsInAList; i++)
            {
                GameFormat model = APBuilder.CreateBasicGameFormat().Get();
                _dbContext.Add(model);
                models.Add(model);
            }
            _dbContext.SaveChanges();
            models.ForEach(Detach);
        }
    }
}
