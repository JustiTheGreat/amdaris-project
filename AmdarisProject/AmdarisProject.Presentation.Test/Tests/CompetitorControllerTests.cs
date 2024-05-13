using AmdarisProject.Application.Dtos.CreateDTOs;
using AmdarisProject.Application.Dtos.DisplayDTOs.CompetitorDisplayDTOs;
using AmdarisProject.Application.Dtos.ResponseDTOs;
using AmdarisProject.Application.Dtos.ResponseDTOs.CompetitorResponseDTOs;
using AmdarisProject.Application.Handlers.CompetitorHandlers;
using AmdarisProject.Application.Handlers.GameFormatHandlers;
using AmdarisProject.Application.Test.ModelBuilders;
using AmdarisProject.Application.Test.ModelBuilders.CompetitionBuilders;
using AmdarisProject.Application.Test.ModelBuilders.CompetitorBuilders;
using AmdarisProject.Domain.Models;
using AmdarisProject.Domain.Models.CompetitionModels;
using AmdarisProject.Domain.Models.CompetitorModels;
using AmdarisProject.Presentation.Controllers;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Numerics;

namespace AmdarisProject.Presentation.Test.Tests
{
    public class CompetitorControllerTests : ControllerTests<CompetitorController>
    {
        public class PointControllerTests : ControllerTests<CompetitorController>
        {
            [Fact]
            public async Task Test_GetCompetitorById_Player_Success()
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
            public async Task Test_GetCompetitorById_Team_Success()
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
            public async Task Test_CreatePlayer_Success()
            {
                Setup<CreatePlayer, PlayerGetDTO, CreatePlayerHandler>();
                Player player = APBuilder.CreateBasicPlayer().Get();

                var requestResult = await _controller.CreatePlayer(player.Adapt<CompetitorCreateDTO>());

                var result = requestResult as OkObjectResult;
                var response = result?.Value as PlayerGetDTO;
                Assert.Equal((int)HttpStatusCode.OK, result!.StatusCode);
                Assert.NotNull(response);
                AssertResponse.PlayerGetDTO(player, response, createPlayer: true);
            }

            [Fact]
            public async Task Test_CreateTeam_Success()
            {
                Setup<CreateTeam, TeamGetDTO, CreateTeamHandler>();
                Team team = APBuilder.CreateBasicTeam().Get();

                var requestResult = await _controller.CreateTeam(team.Adapt<CompetitorCreateDTO>());

                var result = requestResult as OkObjectResult;
                var response = result?.Value as TeamGetDTO;
                Assert.Equal((int)HttpStatusCode.OK, result!.StatusCode);
                Assert.NotNull(response);
                AssertResponse.TeamGetDTO(team, response, createTeam: true);
            }

            [Fact]
            public async Task Test_GetAllPlayers_Success()
            {
                Setup<GetAllPlayers, IEnumerable<PlayerDisplayDTO>, GetAllPlayersHandler>();
                Seed_GetAllPlayers(out List<Player> players);

                var requestResult = await _controller.GetAllPlayers();

                var result = requestResult as OkObjectResult;
                var response = result?.Value as IEnumerable<PlayerDisplayDTO>;
                Assert.Equal((int)HttpStatusCode.OK, result!.StatusCode);
                Assert.NotNull(response);
                for (int i = 0; i < _numberOfModelsInAList; i++)
                {
                    Assert.Equal(players.ElementAt(i).Id, response.ElementAt(i).Id);
                    Assert.Equal(players.ElementAt(i).Name, response.ElementAt(i).Name);
                }
            }

            private void Seed_GetAllPlayers(out List<Player> players)
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
            public async Task Test_GetAllTeams_Success()
            {
                Setup<GetAllTeams, IEnumerable<TeamDisplayDTO>, GetAllTeamsHandler>();
                Seed_GetAllTeams(out List<Team> teams);

                var requestResult = await _controller.GetAllTeams();

                var result = requestResult as OkObjectResult;
                var response = result?.Value as IEnumerable<TeamDisplayDTO>;
                Assert.Equal((int)HttpStatusCode.OK, result!.StatusCode);
                Assert.NotNull(response);
                for (int i = 0; i < _numberOfModelsInAList; i++)
                {
                    Assert.Equal(teams.ElementAt(i).Id, response.ElementAt(i).Id);
                    Assert.Equal(teams.ElementAt(i).Name, response.ElementAt(i).Name);
                    teams.ElementAt(i).Players.ForEach(player => Assert.Contains(player.Name, response.ElementAt(i).PlayerNames));
                }
            }

            private void Seed_GetAllTeams(out List<Team> teams)
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
            public async Task Test_GetPlayersNotInTeam_Success()
            {
                Setup<GetPlayersNotInTeam, IEnumerable<PlayerDisplayDTO>, GetPlayersNotInTeamHandler>();
                Seed_GetPlayersNotInTeam(out List<Player> playersNotInTeam, out Guid teamId);

                var requestResult = await _controller.GetPlayersNotInTeam(teamId);

                var result = requestResult as OkObjectResult;
                var response = result?.Value as IEnumerable<PlayerDisplayDTO>;
                Assert.Equal((int)HttpStatusCode.OK, result!.StatusCode);
                Assert.NotNull(response);
                for (int i = 0; i < playersNotInTeam.Count; i++)
                {
                    Assert.Equal(playersNotInTeam.ElementAt(i).Id, response.ElementAt(i).Id);
                    Assert.Equal(playersNotInTeam.ElementAt(i).Name, response.ElementAt(i).Name);
                }
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
            public async Task Test_GetPlayersNotInCompetition_Success()
            {
                Setup<GetPlayersNotInCompetition, IEnumerable<PlayerDisplayDTO>, GetPlayersNotInCompetitionHandler>();
                Seed_GetPlayersNotInCompetition(out List<Player> playersNotInCompetitions, out Guid teamId);

                var requestResult = await _controller.GetPlayersNotInCompetition(teamId);

                var result = requestResult as OkObjectResult;
                var response = result?.Value as IEnumerable<PlayerDisplayDTO>;
                Assert.Equal((int)HttpStatusCode.OK, result!.StatusCode);
                Assert.NotNull(response);
                for (int i = 0; i < playersNotInCompetitions.Count; i++)
                {
                    Assert.Equal(playersNotInCompetitions.ElementAt(i).Id, response.ElementAt(i).Id);
                    Assert.Equal(playersNotInCompetitions.ElementAt(i).Name, response.ElementAt(i).Name);
                }
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
            public async Task Test_GetTeamsThatCanBeAddedToCompetition_Success()
            {
                Setup<GetPlayersNotInCompetition, IEnumerable<PlayerDisplayDTO>, GetPlayersNotInCompetitionHandler>();
                Seed_GetTeamsThatCanBeAddedToCompetition(out List<Team> teamsThatCanBeAddedToCompetition, out Guid competitionId);

                var requestResult = await _controller.GetTeamsThatCanBeAddedToCompetition(competitionId);

                var result = requestResult as OkObjectResult;
                var response = result?.Value as IEnumerable<TeamDisplayDTO>;
                Assert.Equal((int)HttpStatusCode.OK, result!.StatusCode);
                Assert.NotNull(response);
                for (int i = 0; i < teamsThatCanBeAddedToCompetition.Count; i++)
                {
                    Assert.Equal(teamsThatCanBeAddedToCompetition.ElementAt(i).Id, response.ElementAt(i).Id);
                    Assert.Equal(teamsThatCanBeAddedToCompetition.ElementAt(i).Name, response.ElementAt(i).Name);
                }
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
