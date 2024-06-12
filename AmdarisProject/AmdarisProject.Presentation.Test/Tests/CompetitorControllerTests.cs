using AmdarisProject.Application.Common.Models;
using AmdarisProject.Application.Dtos.RequestDTOs.CreateDTOs;
using AmdarisProject.Application.Dtos.ResponseDTOs.CompetitorResponseDTOs;
using AmdarisProject.Application.Dtos.ResponseDTOs.DisplayDTOs;
using AmdarisProject.Application.Handlers.CompetitorHandlers;
using AmdarisProject.Domain.Models;
using AmdarisProject.Domain.Models.CompetitionModels;
using AmdarisProject.Domain.Models.CompetitorModels;
using AmdarisProject.Presentation.Controllers;
using AmdarisProject.TestUtils;
using AmdarisProject.TestUtils.ModelBuilders;
using AmdarisProject.TestUtils.ModelBuilders.CompetitionBuilders;
using AmdarisProject.TestUtils.ModelBuilders.CompetitorBuilders;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace AmdarisProject.Presentation.Test.Tests
{
    public class CompetitorControllerTests : PresentationTestBase<CompetitorController>
    {
        public class PointControllerTests : PresentationTestBase<CompetitorController>
        {
            [Fact]
            public async Task Test_GetCompetitorById_Player_OkStatus()
            {
                Setup<GetCompetitorById, CompetitorGetDTO, GetCompetitorByIdHandler>();
                Seed_GetCompetitorById(out Player player);

                var requestResult = await _controller.GetCompetitorById(player.Id);

                var result = requestResult as OkObjectResult;
                var response = result?.Value as PlayerGetDTO;
                Assert.Equal((int)HttpStatusCode.OK, result!.StatusCode);
                Assert.NotNull(response);
                AssertResponse.PlayerGetDTO(player, response);
            }

            private void Seed_GetCompetitorById(out Player player)
            {
                player = APBuilder.CreateBasicPlayer().Get();
                _dbContext.Add(player);
                _dbContext.SaveChanges();
                Detach(player);
            }

            [Fact]
            public async Task Test_GetCompetitorById_Team_OkStatus()
            {
                Setup<GetCompetitorById, CompetitorGetDTO, GetCompetitorByIdHandler>();
                Seed_GetCompetitorById(out Team team);

                var requestResult = await _controller.GetCompetitorById(team.Id);

                var result = requestResult as OkObjectResult;
                var response = result?.Value as TeamGetDTO;
                Assert.Equal((int)HttpStatusCode.OK, result!.StatusCode);
                Assert.NotNull(response);
                AssertResponse.TeamGetDTO(team, response);
            }

            private void Seed_GetCompetitorById(out Team team)
            {
                team = APBuilder.CreateBasicTeam().Get();
                _dbContext.Add(team);
                _dbContext.SaveChanges();
                Detach(team);
            }

            [Fact]
            public async Task Test_CreatePlayer_CreatedStatus()
            {
                Setup<CreatePlayer, PlayerGetDTO, CreatePlayerHandler>();
                Player player = APBuilder.CreateBasicPlayer().Get();

                var requestResult = await _controller.CreatePlayer(_mapper.Map<CompetitorCreateDTO>(player));

                var result = requestResult as CreatedResult;
                Assert.NotNull(result);
            }

            [Fact]
            public async Task Test_CreateTeam_CreatedStatus()
            {
                Setup<CreateTeam, TeamGetDTO, CreateTeamHandler>();
                Team team = APBuilder.CreateBasicTeam().Get();

                var requestResult = await _controller.CreateTeam(_mapper.Map<CompetitorCreateDTO>(team));

                var result = requestResult as CreatedResult;
                Assert.NotNull(result);
            }

            [Fact]
            public async Task Test_GetPaginatedPlayers_OkStatus()
            {
                Setup<GetPaginatedPlayers, PaginatedResult<CompetitorDisplayDTO>, GetPaginatedPlayersHandler>();
                Seed_GetPaginatedPlayers(out List<Player> players);

                var requestResult = await _controller.GetPaginatedPlayers(_pagedRequest);

                var result = requestResult as OkObjectResult;
                var response = result?.Value as PaginatedResult<CompetitorDisplayDTO>;
                Assert.Equal((int)HttpStatusCode.OK, result!.StatusCode);
                Assert.NotNull(response);
                Assert.Equal(_pagedRequest.PageIndex, response.PageIndex);
                Assert.Equal(_pagedRequest.PageSize, response.PageSize);
                Assert.Equal(_pagedRequest.PageSize, response.Total);
                Assert.Equal(_pagedRequest.PageSize, response.Items.Count());
                players.ForEach(player =>
                {
                    CompetitorDisplayDTO competitorDisplayDTO =
                        response.Items.First(competitorDisplayDTO => competitorDisplayDTO.Id.Equals(player.Id));

                    Assert.Equal(player.Id, competitorDisplayDTO.Id);
                    Assert.Equal(player.Name, competitorDisplayDTO.Name);
                });
            }

            private void Seed_GetPaginatedPlayers(out List<Player> players)
            {
                players = [];
                for (int i = 0; i < _numberOfModelsInAList; i++)
                {
                    Player player = APBuilder.CreateBasicPlayer().Get();
                    _dbContext.Add(player);
                    players.Add(player);
                }
                _dbContext.SaveChanges();
                players.ForEach(Detach);
            }

            [Fact]
            public async Task Test_GetPaginatedTeams_OkStatus()
            {
                Setup<GetPaginatedTeams, PaginatedResult<CompetitorDisplayDTO>, GetPaginatedTeamsHandler>();
                Seed_GetPaginatedTeams(out List<Team> teams);

                var requestResult = await _controller.GetPaginatedTeams(_pagedRequest);

                var result = requestResult as OkObjectResult;
                var response = result?.Value as PaginatedResult<CompetitorDisplayDTO>;
                Assert.Equal((int)HttpStatusCode.OK, result!.StatusCode);
                Assert.NotNull(response);
                Assert.Equal(_pagedRequest.PageIndex, response.PageIndex);
                Assert.Equal(_pagedRequest.PageSize, response.PageSize);
                Assert.Equal(_pagedRequest.PageSize, response.Total);
                Assert.Equal(_pagedRequest.PageSize, response.Items.Count());
                teams.ForEach(team =>
                {
                    CompetitorDisplayDTO competitorDisplayDTO =
                        response.Items.First(competitorDisplayDTO => competitorDisplayDTO.Id.Equals(team.Id));

                    Assert.Equal(team.Id, competitorDisplayDTO.Id);
                    Assert.Equal(team.Name, competitorDisplayDTO.Name);
                });
            }

            private void Seed_GetPaginatedTeams(out List<Team> teams)
            {
                teams = [];
                for (int i = 0; i < _numberOfModelsInAList; i++)
                {
                    Team team = APBuilder.CreateBasicTeam().Get();
                    _dbContext.Add(team);
                    teams.Add(team);
                }
                _dbContext.SaveChanges();
                teams.ForEach(Detach);
            }

            [Fact]
            public async Task Test_GetPlayersNotInTeam_OkStatus()
            {
                Setup<GetPlayersNotInTeam, IEnumerable<CompetitorDisplayDTO>, GetPlayersNotInTeamHandler>();
                Seed_GetPlayersNotInTeam(out List<Player> playersNotInTeam, out Guid teamId);

                var requestResult = await _controller.GetPlayersNotInTeam(teamId);

                var result = requestResult as OkObjectResult;
                var response = result?.Value as IEnumerable<CompetitorDisplayDTO>;
                Assert.Equal((int)HttpStatusCode.OK, result!.StatusCode);
                Assert.NotNull(response);
                Assert.Equal(playersNotInTeam.Count, response.Count());
                playersNotInTeam.ForEach(player =>
                {
                    CompetitorDisplayDTO competitorDisplayDTO =
                        response.First(competitorDisplayDTO => competitorDisplayDTO.Id.Equals(player.Id));

                    Assert.Equal(player.Id, competitorDisplayDTO.Id);
                    Assert.Equal(player.Name, competitorDisplayDTO.Name);
                });
            }

            private void Seed_GetPlayersNotInTeam(out List<Player> playersNotInTeam, out Guid teamId)
            {
                playersNotInTeam = [];

                for (int i = 0; i < _numberOfModelsInAList; i++)
                {
                    Player player = APBuilder.CreateBasicPlayer().Get();
                    playersNotInTeam.Add(player);
                    _dbContext.Add(player);
                }

                TeamBuilder teamBuilder = APBuilder.CreateBasicTeam();

                for (int i = 0; i < _numberOfModelsInAList / 2; i++)
                    teamBuilder.AddPlayer(playersNotInTeam[i]);

                Team team = teamBuilder.Get();
                teamId = team.Id;

                _dbContext.Add(team);
                _dbContext.SaveChanges();
                playersNotInTeam.ForEach(Detach);

                foreach (Player player in team.Players)
                    playersNotInTeam.Remove(player);
            }

            [Fact]
            public async Task Test_GetPlayersNotInCompetition_OkStatus()
            {
                Setup<GetPlayersNotInCompetition, IEnumerable<CompetitorDisplayDTO>, GetPlayersNotInCompetitionHandler>();
                Seed_GetPlayersNotInCompetition(out List<Player> playersNotInCompetitions, out Guid teamId);

                var requestResult = await _controller.GetPlayersNotInCompetition(teamId);

                var result = requestResult as OkObjectResult;
                var response = result?.Value as IEnumerable<CompetitorDisplayDTO>;
                Assert.Equal((int)HttpStatusCode.OK, result!.StatusCode);
                Assert.NotNull(response);
                Assert.Equal(playersNotInCompetitions.Count, response.Count());
                playersNotInCompetitions.ForEach(player =>
                {
                    CompetitorDisplayDTO competitorDisplayDTO =
                        response.First(competitorDisplayDTO => competitorDisplayDTO.Id.Equals(player.Id));

                    Assert.Equal(player.Id, competitorDisplayDTO.Id);
                    Assert.Equal(player.Name, competitorDisplayDTO.Name);
                });
            }

            private void Seed_GetPlayersNotInCompetition(out List<Player> playersNotInCompetition, out Guid competitionId)
            {
                playersNotInCompetition = [];

                for (int i = 0; i < _numberOfModelsInAList; i++)
                {
                    Player player = APBuilder.CreateBasicPlayer().Get();
                    playersNotInCompetition.Add(player);
                    _dbContext.Add(player);
                }

                OneVSAllCompetitionBuilder competitionBuilder = APBuilder.CreateBasicOneVSAllCompetition();

                for (int i = 0; i < _numberOfModelsInAList / 2; i++)
                    competitionBuilder.AddCompetitor(playersNotInCompetition[i]);

                Competition competition = competitionBuilder.Get();
                competitionId = competition.Id;

                _dbContext.Add(competition);
                _dbContext.SaveChanges();
                playersNotInCompetition.ForEach(Detach);

                foreach (Player player in competition.Competitors)
                    playersNotInCompetition.Remove(player);
            }

            [Fact]
            public async Task Test_GetTeamsThatCanBeAddedToCompetition_OkStatus()
            {
                Setup<GetPlayersNotInCompetition, IEnumerable<CompetitorDisplayDTO>, GetPlayersNotInCompetitionHandler>();
                Seed_GetTeamsThatCanBeAddedToCompetition(out List<Team> teamsThatCanBeAddedToCompetition, out Guid competitionId);

                var requestResult = await _controller.GetTeamsThatCanBeAddedToCompetition(competitionId);

                var result = requestResult as OkObjectResult;
                var response = result?.Value as IEnumerable<CompetitorDisplayDTO>;
                Assert.Equal((int)HttpStatusCode.OK, result!.StatusCode);
                Assert.NotNull(response);
                Assert.Equal(teamsThatCanBeAddedToCompetition.Count, response.Count());
                teamsThatCanBeAddedToCompetition.ForEach(team =>
                {
                    CompetitorDisplayDTO competitorDisplayDTO =
                        response.First(competitorDisplayDTO => competitorDisplayDTO.Id.Equals(team.Id));

                    Assert.Equal(team.Id, competitorDisplayDTO.Id);
                    Assert.Equal(team.Name, competitorDisplayDTO.Name);
                });
            }

            private void Seed_GetTeamsThatCanBeAddedToCompetition(out List<Team> teamsThatCanBeAddedToCompetition, out Guid competitionId)
            {
                teamsThatCanBeAddedToCompetition = [];

                GameFormat gameFormat = APBuilder.CreateBasicGameFormat()
                    .SetCompetitorType(Domain.Enums.CompetitorType.TEAM)
                    .SetTeamSize(3).Get();
                OneVSAllCompetitionBuilder competitionBuilder = APBuilder.CreateBasicOneVSAllCompetition().SetGameFormat(gameFormat);

                for (int i = 0; i < _numberOfModelsInAList; i++)
                {
                    TeamBuilder teamBuilder = APBuilder.CreateBasicTeam();

                    for (int j = 0; j < gameFormat.TeamSize; j++)
                    {
                        Player player = APBuilder.CreateBasicPlayer().Get();
                        teamBuilder.AddPlayer(player);
                        _dbContext.Add(APBuilder.CreateBasicTeamPlayer()
                            .SetTeam(teamBuilder.Get()).SetPlayer(player).SetIsActive(true).Get());
                    }

                    teamsThatCanBeAddedToCompetition.Add(teamBuilder.Get());
                }

                for (int i = 0; i < _numberOfModelsInAList / 2; i++)
                    competitionBuilder.AddCompetitor(teamsThatCanBeAddedToCompetition[i]);

                Competition competition = competitionBuilder.Get();
                competitionId = competition.Id;

                _dbContext.Add(competition);
                _dbContext.SaveChanges();
                teamsThatCanBeAddedToCompetition.ForEach(Detach);

                foreach (Team team in competition.Competitors)
                    teamsThatCanBeAddedToCompetition.Remove(team);
            }
        }
    }
}
