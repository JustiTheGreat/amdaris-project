using AmdarisProject.Application.Dtos.ResponseDTOs;
using AmdarisProject.Application.Dtos.ResponseDTOs.CompetitionResponseDTOs;
using AmdarisProject.Application.Dtos.ResponseDTOs.CompetitorResponseDTOs;
using AmdarisProject.Application.Dtos.ResponseDTOs.DisplayDTOs;
using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Models;
using AmdarisProject.Domain.Models.CompetitionModels;
using AmdarisProject.Domain.Models.CompetitorModels;
using Xunit;

namespace AmdarisProject.TestUtils
{
    public class AssertResponse
    {
        public static void MatchMatchGetDTO(Match model, MatchGetDTO response, bool startMatch = false, bool endMatch = false)
        {
            Assert.Equal(model.Id, response.Id);

            if (startMatch) Assert.NotNull(response.StartTime);
            else Assert.Equal(model.StartTime, response.StartTime);

            if (endMatch) Assert.NotNull(response.EndTime);
            else Assert.Equal(model.EndTime, response.EndTime);

            Assert.Equal(model.Status.ToString(), response.Status);
            Assert.Equal(model.CompetitorOne.Id, response.CompetitorOne.Id);
            Assert.Equal(model.CompetitorOne.Name, response.CompetitorOne.Name);
            Assert.Equal(model.CompetitorTwo.Id, response.CompetitorTwo.Id);
            Assert.Equal(model.CompetitorTwo.Name, response.CompetitorTwo.Name);
            Assert.Equal(model.Competition.Id, response.Competition.Id);
            Assert.Equal(model.Competition.Name, response.Competition.Name);
            Assert.Equal(model.Competition.Status.ToString(), response.Competition.Status);
            Assert.Equal(model.Competition.GameFormat.GameType.Name, response.Competition.GameType);
            Assert.Equal(model.Competition.GameFormat.CompetitorType.ToString(), response.Competition.CompetitorType);
            Assert.Equal(model.CompetitorOnePoints, response.CompetitorOnePoints);
            Assert.Equal(model.CompetitorTwoPoints, response.CompetitorTwoPoints);
            Assert.Equal(model.Winner?.Id, response.Winner?.Id);
            Assert.Equal(model.Winner?.Name, response.Winner?.Name);
            Assert.Equal(model.StageLevel, response.StageLevel);
            Assert.Equal(model.StageIndex, response.StageIndex);
            Assert.Equal(model.Points.Count, response.Points.Count);

            for (int i = 0; i < model.Points.Count; i++)
            {
                Point point = model.Points[i];
                PointDisplayDTO pointDisplayDTO = response.Points[i];

                if (!startMatch)
                    Assert.Equal(point.Id, pointDisplayDTO.Id);

                Assert.Equal((int)point.Value, pointDisplayDTO.Value);
                Assert.Equal(point.Match.Id, pointDisplayDTO.MatchId);
                Assert.Equal(point.Player.Id, pointDisplayDTO.PlayerId);
                Assert.Equal(point.Player.Name, pointDisplayDTO.Player);
            }
        }

        public static void PlayerGetDTO(Player model, PlayerGetDTO response, bool createPlayer = false)
        {
            if (!createPlayer)
                Assert.Equal(model.Id, response.Id);

            Assert.Equal(model.Name, response.Name);
            Assert.Equal(model.Matches.Count, response.Matches.Count);

            for (int i = 0; i < model.Matches.Count; i++)
            {
                Match match = model.Matches[i];
                MatchDisplayDTO matchDisplayDTO = response.Matches[i];

                if (!createPlayer)
                    Assert.Equal(match.Id, matchDisplayDTO.Id);

                Assert.Equal(match.Status.ToString(), matchDisplayDTO.Status);
                Assert.Equal(match.StartTime, matchDisplayDTO.StartTime);
                Assert.Equal($"{match.CompetitorOne.Name} - {match.CompetitorTwo.Name}", matchDisplayDTO.Competitors);
                Assert.Equal($"{match.CompetitorOnePoints} - {match.CompetitorTwoPoints}", matchDisplayDTO.Score);
                Assert.Equal(match.Competition.Name, matchDisplayDTO.Competition);
                Assert.Equal(match.Winner?.Name, matchDisplayDTO.Winner);
            }

            Assert.Equal(model.WonMatches.Count, response.WonMatches.Count);
            model.WonMatches.ForEach(match => Assert.Contains(match.Id, response.WonMatches));
            Assert.Equal(model.Competitions.Count, response.Competitions.Count);

            for (int i = 0; i < model.Competitions.Count; i++)
            {
                Competition competition = model.Competitions[i];
                CompetitionDisplayDTO competitionDisplayDTO = response.Competitions[i];

                if (!createPlayer)
                    Assert.Equal(competition.Id, response.Competitions.ElementAt(i).Id);

                Assert.Equal(competition.Name, competitionDisplayDTO.Name);
                Assert.Equal(competition.Status.ToString(), competitionDisplayDTO.Status);
                Assert.Equal(competition.GameFormat.GameType.Name, competitionDisplayDTO.GameType);
                Assert.Equal(competition.GameFormat.CompetitorType.ToString(), competitionDisplayDTO.CompetitorType);
            }

            Assert.Equal(model.Points.Count, response.Points.Count);
            model.Points.ForEach(point => Assert.Contains(point.Id, response.Points));
            Assert.Equal(model.Competitions.Count, response.Competitions.Count);

            for (int i = 0; i < model.Teams.Count; i++)
            {
                Team team = model.Teams[i];
                CompetitorDisplayDTO competitorDisplayDTO = response.Teams[i];

                if (!createPlayer)
                    Assert.Equal(team.Id, competitorDisplayDTO.Id);

                Assert.Equal(team.Name, competitorDisplayDTO.Name);
                Assert.Equal(CompetitorType.TEAM.ToString(), competitorDisplayDTO.CompetitorType);
                Assert.Equal(team.Competitions.Count(), competitorDisplayDTO.NumberOfCompetitions);
                Assert.Equal(team.Matches.Count(), competitorDisplayDTO.NumberOfMatches);
                Assert.Null(competitorDisplayDTO.NumberOfTeams);
                Assert.Equal(team.TeamPlayers.Count(), competitorDisplayDTO.NumberOfPlayers);
                Assert.Equal(team.TeamPlayers.Count(teamPlayer => teamPlayer.IsActive), competitorDisplayDTO.NumberOfActivePlayers);
            }
        }

        public static void TeamGetDTO(Team team, TeamGetDTO response, bool createTeam = false)
        {
            if (!createTeam)
                Assert.Equal(team.Id, response.Id);

            Assert.Equal(team.Name, response.Name);
            Assert.Equal(team.Matches.Count, response.Matches.Count);

            for (int i = 0; i < team.Matches.Count; i++)
            {
                Match match = team.Matches[i];
                MatchDisplayDTO matchDisplayDTO = response.Matches[i];

                if (!createTeam)
                    Assert.Equal(match.Id, response.Matches.ElementAt(i).Id);

                Assert.Equal(match.Status.ToString(), matchDisplayDTO.Status);
                Assert.Equal(match.StartTime, matchDisplayDTO.StartTime);
                Assert.Equal($"{match.CompetitorOne.Name} - {match.CompetitorTwo.Name}", matchDisplayDTO.Competitors);
                Assert.Equal($"{match.CompetitorOnePoints} - {match.CompetitorTwoPoints}", matchDisplayDTO.Score);
                Assert.Equal(match.Competition.Name, matchDisplayDTO.Competition);
                Assert.Equal(match.Winner?.Name, matchDisplayDTO.Winner);
            }

            Assert.Equal(team.WonMatches.Count, response.WonMatches.Count);
            team.WonMatches.ForEach(match => Assert.Contains(match.Id, response.WonMatches));
            Assert.Equal(team.Competitions.Count, response.Competitions.Count);

            for (int i = 0; i < team.Competitions.Count; i++)
            {
                Competition competition = team.Competitions[i];
                CompetitionDisplayDTO competitionDisplayDTO = response.Competitions[i];

                if (!createTeam)
                    Assert.Equal(competition.Id, competitionDisplayDTO.Id);

                Assert.Equal(competition.Name, competitionDisplayDTO.Name);
                Assert.Equal(competition.Status.ToString(), competitionDisplayDTO.Status);
                Assert.Equal(competition.GameFormat.GameType.Name, competitionDisplayDTO.GameType);
                Assert.Equal(competition.GameFormat.CompetitorType.ToString(), competitionDisplayDTO.CompetitorType);
            }

            Assert.Equal(team.Players.Count, response.Players.Count);

            for (int i = 0; i < team.Players.Count; i++)
            {
                Player player = team.Players[i];
                CompetitorDisplayDTO competitorDisplayDTO = response.Players[i];

                if (!createTeam)
                    Assert.Equal(player.Id, competitorDisplayDTO.Id);

                Assert.Equal(player.Name, competitorDisplayDTO.Name);
                Assert.Equal(CompetitorType.PLAYER.ToString(), competitorDisplayDTO.CompetitorType);
                Assert.Equal(player.Competitions.Count(), competitorDisplayDTO.NumberOfCompetitions);
                Assert.Equal(player.Matches.Count(), competitorDisplayDTO.NumberOfMatches);
                Assert.Equal(player.Teams.Count(), competitorDisplayDTO.NumberOfTeams);
                Assert.Null(competitorDisplayDTO.NumberOfPlayers);
                Assert.Null(competitorDisplayDTO.NumberOfActivePlayers);
            }
        }

        public static void OneVSAllCompetitionGetDTO(OneVSAllCompetition model, OneVSAllCompetitionGetDTO response,
            bool createOneVSAllCompetition = false)
        {
            if (!createOneVSAllCompetition)
                Assert.Equal(model.Id, response.Id);

            Assert.Equal(model.Name, response.Name);
            Assert.Equal(model.Location, response.Location);
            Assert.Equal(model.StartTime, response.StartTime);
            Assert.Equal(model.Status.ToString(), response.Status);
            Assert.Equal(model.BreakInMinutes, response.BreakInMinutes);
            Assert.Equal(model.GameFormat.GameType.Id, response.GameType.Id);
            Assert.Equal(model.GameFormat.GameType.Name, response.GameType.Name);
            Assert.Equal(model.GameFormat.CompetitorType.ToString(), response.CompetitorType);
            Assert.Equal(model.GameFormat.TeamSize, response.TeamSize);
            Assert.Equal(model.GameFormat.WinAt, response.WinAt);
            Assert.Equal(model.GameFormat.DurationInMinutes, response.DurationInMinutes);
            Assert.Equal(model.Matches.Count, response.Matches.Count);

            for (int i = 0; i < model.Matches.Count; i++)
            {
                Match match = model.Matches[i];
                MatchDisplayDTO matchDisplayDTO = response.Matches[i];

                if (!createOneVSAllCompetition)
                    Assert.Equal(match.Id, matchDisplayDTO.Id);

                Assert.Equal(match.Status.ToString(), matchDisplayDTO.Status);
                Assert.Equal(match.StartTime, matchDisplayDTO.StartTime);
                Assert.Equal($"{match.CompetitorOne.Name} - {match.CompetitorTwo.Name}", matchDisplayDTO.Competitors);
                Assert.Equal($"{match.CompetitorOnePoints} - {match.CompetitorTwoPoints}", matchDisplayDTO.Score);
                Assert.Equal(match.Competition.Name, matchDisplayDTO.Competition);
                Assert.Equal(match.Winner?.Name, matchDisplayDTO.Winner);
            }

            for (int i = 0; i < model.Competitors.Count; i++)
            {
                Competitor competitor = model.Competitors[i];
                CompetitorDisplayDTO competitorDisplayDTO = response.Competitors[i];

                if (!createOneVSAllCompetition)
                    Assert.Equal(competitor.Id, competitorDisplayDTO.Id);

                Assert.Equal(competitor.Name, competitorDisplayDTO.Name);
                Assert.Equal(competitor.Competitions.Count(), competitorDisplayDTO.NumberOfCompetitions);
                Assert.Equal(competitor.Matches.Count(), competitorDisplayDTO.NumberOfMatches);
                if (competitor is Player)
                {
                    Assert.Equal(CompetitorType.PLAYER.ToString(), competitorDisplayDTO.CompetitorType);
                    Assert.Equal(competitor.TeamPlayers.Count(), competitorDisplayDTO.NumberOfTeams);
                    Assert.Null(competitorDisplayDTO.NumberOfPlayers);
                    Assert.Null(competitorDisplayDTO.NumberOfActivePlayers);
                }
                if (competitor is Team)
                {
                    Assert.Equal(CompetitorType.TEAM.ToString(), competitorDisplayDTO.CompetitorType);
                    Assert.Null(competitorDisplayDTO.NumberOfTeams);
                    Assert.Equal(competitor.TeamPlayers.Count(), competitorDisplayDTO.NumberOfPlayers);
                    Assert.Equal(competitor.TeamPlayers.Count(teamPlayer => teamPlayer.IsActive), competitorDisplayDTO.NumberOfActivePlayers);
                }
            }
        }

        public static void TournamentCompetitionGetDTO(TournamentCompetition model, TournamentCompetitionGetDTO response,
            bool createTournamentCompetition = false)
        {
            if (!createTournamentCompetition)
                Assert.Equal(model.Id, response.Id);

            Assert.Equal(model.Name, response.Name);
            Assert.Equal(model.Location, response.Location);
            Assert.Equal(model.StartTime, response.StartTime);
            Assert.Equal(model.Status.ToString(), response.Status);
            Assert.Equal(model.BreakInMinutes, response.BreakInMinutes);
            Assert.Equal(model.GameFormat.GameType.Id, response.GameType.Id);
            Assert.Equal(model.GameFormat.GameType.Name, response.GameType.Name);
            Assert.Equal(model.GameFormat.CompetitorType.ToString(), response.CompetitorType);
            Assert.Equal(model.GameFormat.TeamSize, response.TeamSize);
            Assert.Equal(model.GameFormat.WinAt, response.WinAt);
            Assert.Equal(model.GameFormat.DurationInMinutes, response.DurationInMinutes);
            Assert.Equal(model.Matches.Count, response.Matches.Count);

            for (int i = 0; i < model.Matches.Count; i++)
            {
                Match match = model.Matches[i];
                MatchDisplayDTO matchDisplayDTO = response.Matches[i];

                if (!createTournamentCompetition)
                    Assert.Equal(match.Id, matchDisplayDTO.Id);

                Assert.Equal(match.Status.ToString(), matchDisplayDTO.Status);
                Assert.Equal(match.StartTime, matchDisplayDTO.StartTime);
                Assert.Equal($"{match.CompetitorOne.Name} - {match.CompetitorTwo.Name}", matchDisplayDTO.Competitors);
                Assert.Equal($"{match.CompetitorOnePoints} - {match.CompetitorTwoPoints}", matchDisplayDTO.Score);
                Assert.Equal(match.Competition.Name, matchDisplayDTO.Competition);
                Assert.Equal(match.Winner?.Name, matchDisplayDTO.Winner);
            }

            Assert.Equal(model.Competitors.Count, response.Competitors.Count);

            for (int i = 0; i < model.Competitors.Count; i++)
            {
                Competitor competitor = model.Competitors[i];
                CompetitorDisplayDTO competitorDisplayDTO = response.Competitors[i];

                if (!createTournamentCompetition)
                    Assert.Equal(competitor.Id, competitorDisplayDTO.Id);

                Assert.Equal(competitor.Name, competitorDisplayDTO.Name);
                Assert.Equal(competitor.Competitions.Count(), competitorDisplayDTO.NumberOfCompetitions);
                Assert.Equal(competitor.Matches.Count(), competitorDisplayDTO.NumberOfMatches);
                if (competitor is Player)
                {
                    Assert.Equal(CompetitorType.PLAYER.ToString(), competitorDisplayDTO.CompetitorType);
                    Assert.Equal(competitor.TeamPlayers.Count(), competitorDisplayDTO.NumberOfTeams);
                    Assert.Null(competitorDisplayDTO.NumberOfPlayers);
                    Assert.Null(competitorDisplayDTO.NumberOfActivePlayers);
                }
                if (competitor is Team)
                {
                    Assert.Equal(CompetitorType.TEAM.ToString(), competitorDisplayDTO.CompetitorType);
                    Assert.Null(competitorDisplayDTO.NumberOfTeams);
                    Assert.Equal(competitor.TeamPlayers.Count(), competitorDisplayDTO.NumberOfPlayers);
                    Assert.Equal(competitor.TeamPlayers.Count(teamPlayer => teamPlayer.IsActive), competitorDisplayDTO.NumberOfActivePlayers);
                }
            }

            Assert.Equal(model.StageLevel, response.StageLevel);
        }
    }
}
