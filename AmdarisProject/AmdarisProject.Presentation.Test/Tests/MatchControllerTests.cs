using AmdarisProject.Application.Dtos.ResponseDTOs;
using AmdarisProject.Application.Handlers.MatchHandlers;
using AmdarisProject.Application.Test.ModelBuilders;
using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Models;
using AmdarisProject.Domain.Models.CompetitionModels;
using AmdarisProject.Presentation.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace AmdarisProject.Presentation.Test.Tests
{
    public class MatchControllerTests : ControllerTests<MatchController>
    {
        [Fact]
        public async Task Test_GetMatchById_OkStatus()
        {
            Setup<GetMatchById, MatchGetDTO, GetMatchByIdHandler>();
            Seed_GetMatchById(out Match match);

            var requestResult = await _controller.GetMatchById(match.Id);

            var result = requestResult as OkObjectResult;
            var response = result?.Value as MatchGetDTO;
            Assert.Equal((int)HttpStatusCode.OK, result!.StatusCode);
            Assert.NotNull(response);
            AssertResponse.MatchMatchGetDTO(match, response);
        }

        private void Seed_GetMatchById(out Match match)
        {
            match = APBuilder.CreateBasicMatch().Get();
            _dbContext.Add(match);
            _dbContext.SaveChanges();
            Detach(match);
        }

        [Fact]
        public async Task Test_StartMatch_OkStatus()
        {
            Setup<StartMatch, MatchGetDTO, StartMatchHandler>();
            Seed_StartMatch(out Match match);

            var requestResult = await _controller.StartMatch(match.Id);

            var result = requestResult as OkObjectResult;
            var response = result?.Value as MatchGetDTO;
            Assert.Equal((int)HttpStatusCode.OK, result!.StatusCode);
            Assert.NotNull(response);
            AssertResponse.MatchMatchGetDTO(match, response, startMatch: true);
        }

        private void Seed_StartMatch(out Match match)
        {
            Competition competition = APBuilder.CreateBasicOneVSAllCompetition().SetStatus(CompetitionStatus.STARTED).Get();
            MatchBuilder matchBuilder = APBuilder.CreateBasicMatch().SetCompetition(competition);
            match = matchBuilder.Get();
            _dbContext.Add(match);
            _dbContext.SaveChanges();
            Detach(match);
            Detach(competition);
            match = matchBuilder.SetStatus(MatchStatus.STARTED).InitializePoints().Get();
        }

        public static TheoryData<MatchStatus> EndStatuses => new()
        {
            MatchStatus.FINISHED,
            MatchStatus.SPECIAL_WIN_COMPETITOR_ONE,
            MatchStatus.SPECIAL_WIN_COMPETITOR_TWO
        };

        [Theory]
        [MemberData(nameof(EndStatuses))]
        public async Task Test_EndMatch_OkStatus(MatchStatus endStatus)
        {
            Setup<EndMatch, MatchGetDTO, EndMatchHandler>();
            Seed_EndMatch(out Match match, endStatus);

            var requestResult = await _controller.EndMatch(match.Id, endStatus);

            var result = requestResult as OkObjectResult;
            var response = result?.Value as MatchGetDTO;
            Assert.Equal((int)HttpStatusCode.OK, result!.StatusCode);
            Assert.NotNull(response);
            AssertResponse.MatchMatchGetDTO(match, response, endMatch: true);
        }

        private void Seed_EndMatch(out Match match, MatchStatus endStatus)
        {
            GameFormat gameFormat = APBuilder.CreateBasicGameFormat().Get();
            MatchBuilder matchBuilder = APBuilder.CreateBasicMatch()
                .SetStatus(MatchStatus.STARTED)
                .SetCompetitorOnePoints(gameFormat.WinAt)
                .SetCompetitorTwoPoints(0);
            match = matchBuilder.Get();
            _dbContext.Add(match);
            _dbContext.SaveChanges();
            Detach(match);
            match = matchBuilder.SetStatus(endStatus).SetWinner(
                endStatus is MatchStatus.SPECIAL_WIN_COMPETITOR_TWO ? match.CompetitorTwo : match.CompetitorOne).Get();
        }

        [Fact]
        public async Task Test_CancelMatch_OkStatus()
        {
            Setup<CancelMatch, MatchGetDTO, CancelMatchHandler>();
            Seed_CancelMatch(out Match match);

            var requestResult = await _controller.CancelMatch(match.Id);

            var result = requestResult as OkObjectResult;
            var response = result?.Value as MatchGetDTO;
            Assert.Equal((int)HttpStatusCode.OK, result!.StatusCode);
            Assert.NotNull(response);
            AssertResponse.MatchMatchGetDTO(match, response);
        }

        private void Seed_CancelMatch(out Match match)
        {
            MatchBuilder matchBuilder = APBuilder.CreateBasicMatch();
            match = matchBuilder.Get();
            _dbContext.Add(match);
            _dbContext.SaveChanges();
            Detach(match);
            match = matchBuilder.SetStatus(MatchStatus.CANCELED).Get();
        }
    }
}
