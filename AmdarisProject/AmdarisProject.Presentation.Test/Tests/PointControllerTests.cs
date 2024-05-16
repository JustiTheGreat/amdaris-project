using AmdarisProject.Application.Dtos.ResponseDTOs;
using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Models;
using AmdarisProject.handlers.point;
using AmdarisProject.Presentation.Controllers;
using AmdarisProject.TestUtils.ModelBuilders;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace AmdarisProject.Presentation.Test.Tests
{
    public class PointControllerTests : PresentationTestBase<PointController>
    {
        [Fact]
        public async Task Test_AddValueToPointValue_OkStatus()
        {
            Setup<AddValueToPointValue, PointGetDTO, AddValueToPointValueHandler>();
            uint pointsToAdd = 1;
            Seed_AddValueToPointValue(out Point point, pointsToAdd);

            var requestResult = await _controller.AddValueToPointValue(point.Player.Id, point.Match.Id, pointsToAdd);

            var result = requestResult as OkObjectResult;
            var response = result?.Value as PointGetDTO;
            Assert.Equal((int)HttpStatusCode.OK, result!.StatusCode);
            Assert.NotNull(response);
            Assert.Equal(point.Player.Id, response.Player.Id);
            Assert.Equal(point.Player.Name, response.Player.Name);
            Assert.Equal(point.Match.Id, response.Match);
            Assert.Equal(point.Value + pointsToAdd, response.Value);
        }

        private void Seed_AddValueToPointValue(out Point point, uint pointsToAdd)
        {
            Match match = APBuilder.CreateBasicMatch().SetStatus(MatchStatus.STARTED)
                .SetCompetitorOnePoints(0).SetCompetitorTwoPoints(0).Get();
            point = APBuilder.CreateBasicPoint().SetMatch(match).Get();
            _dbContext.Add(point);
            _dbContext.SaveChanges();
            Detach(point);
        }
    }
}
