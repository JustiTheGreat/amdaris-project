using AmdarisProject.Application.Dtos.CreateDTOs.CompetitionCreateDTOs;
using AmdarisProject.Application.Dtos.DisplayDTOs;
using AmdarisProject.Application.Dtos.DisplayDTOs.CompetitorDisplayDTOs;
using AmdarisProject.Application.Dtos.ResponseDTOs;
using AmdarisProject.Application.Dtos.ResponseDTOs.CompetitionResponseDTOs;
using AmdarisProject.Application.Dtos.ResponseDTOs.CompetitorResponseDTOs;
using AmdarisProject.Application.Dtos.ResponseDTOs.GetDTOs;
using AmdarisProject.Domain.Extensions;
using AmdarisProject.Domain.Models;
using AmdarisProject.Domain.Models.CompetitionModels;
using AmdarisProject.Domain.Models.CompetitorModels;
using Mapster;

namespace AmdarisProject.Presentation
{
    public static class MapsterConfiguration
    {
        public static TypeAdapterConfig GetMapsterConfiguration()
        {
            TypeAdapterConfig config = new();

            config.ForType<Team, TeamDisplayDTO>()
                .Map(dest => dest.PlayerNames, src => src.Players.Select(player => player.Name));

            config.ForType<Match, MatchDisplayDTO>()
                .Map(dest => dest.CompetitorOneName, src => src.CompetitorOne.Name)
                .Map(dest => dest.CompetitorTwoName, src => src.CompetitorTwo.Name)
                .Map(dest => dest.CompetitionName, src => src.Competition.Name)
                .Map(dest => dest.WinnerName, src => src.Winner == null ? null : src.Winner.Name);

            config.ForType<Point, PointDisplayDTO>()
                .Map(dest => dest.PlayerName, src => src.Player.Name);

            config.ForType<OneVSAllCompetition, OneVSAllCompetitionResponseDTO>()
                .Map(dest => dest.GameType, src => src.GameFormat.GameType)
                .Map(dest => dest.CompetitorType, src => src.GameFormat.CompetitorType)
                .Map(dest => dest.TeamSize, src => src.GameFormat.TeamSize)
                .Map(dest => dest.WinAt, src => src.GameFormat.WinAt)
                .Map(dest => dest.DurationInSeconds, src => src.GameFormat.DurationInSeconds)
                .Map(dest => dest.Competitors, src => src.Competitors.Adapt<List<CompetitorDisplayDTO>>())
                .Map(dest => dest.Matches, src => src.Matches.Adapt<List<MatchDisplayDTO>>());
            config.ForType<OneVSAllCompetitionCreateDTO, OneVSAllCompetition>()
                .Map(dest => dest.GameFormat, src => (GameFormat)null!);

            config.ForType<TournamentCompetition, TournamentCompetitionResponseDTO>()
                .Map(dest => dest.GameType, src => src.GameFormat.GameType)
                .Map(dest => dest.CompetitorType, src => src.GameFormat.CompetitorType)
                .Map(dest => dest.TeamSize, src => src.GameFormat.TeamSize)
                .Map(dest => dest.WinAt, src => src.GameFormat.WinAt)
                .Map(dest => dest.DurationInSeconds, src => src.GameFormat.DurationInSeconds)
                .Map(dest => dest.Competitors, src => src.Competitors.Adapt<List<CompetitorDisplayDTO>>())
                .Map(dest => dest.Matches, src => src.Matches.Adapt<List<MatchDisplayDTO>>());
            config.ForType<CompetitionCreateDTO, TournamentCompetition>()
                .Map(dest => dest.GameFormat, src => (GameFormat)null!);

            config.ForType<Player, PlayerGetDTO>()
                .Map(dest => dest.Matches, src => src.Matches.Adapt<List<MatchDisplayDTO>>())
                .Map(dest => dest.WonMatches, src => src.WonMatches.GetIds())
                .Map(dest => dest.Competitions, src => src.Competitions.Adapt<List<CompetitionDisplayDTO>>())
                .Map(dest => dest.Teams, src => src.Teams.Adapt<List<PointDisplayDTO>>())
                .Map(dest => dest.Points, src => src.Points.GetIds());

            config.ForType<Team, TeamGetDTO>()
                .Map(dest => dest.Matches, src => src.Matches.Adapt<List<MatchDisplayDTO>>())
                .Map(dest => dest.WonMatches, src => src.WonMatches.GetIds())
                .Map(dest => dest.Competitions, src => src.Competitions.Adapt<List<CompetitionDisplayDTO>>())
                .Map(dest => dest.Players, src => src.Players.Adapt<List<PlayerDisplayDTO>>());

            config.ForType<Match, MatchGetDTO>()
                .Map(dest => dest.CompetitorOne, src => src.CompetitorOne.Adapt<CompetitorDisplayDTO>())
                .Map(dest => dest.CompetitorTwo, src => src.CompetitorTwo.Adapt<CompetitorDisplayDTO>())
                .Map(dest => dest.Winner, src => src.Winner == null ? null : src.Winner.Adapt<CompetitorDisplayDTO>())
                .Map(dest => dest.Competition, src => src.Competition.Adapt<CompetitionDisplayDTO>())
                .Map(dest => dest.Points, src => src.Points.Adapt<List<PointDisplayDTO>>());

            config.ForType<Point, PointGetDTO>()
                .Map(dest => dest.Player, src => src.Player.Adapt<PlayerDisplayDTO>())
                .Map(dest => dest.Match, src => src.Match.Id);

            config.ForType<TeamPlayer, TeamPlayerGetDTO>()
                .Map(dest => dest.TeamId, src => src.Team.Id)
                .Map(dest => dest.PlayerId, src => src.Player.Id);

            return config;
        }

        //HACK for tests
        public static void ConfigureMapster()
        {
            TypeAdapterConfig<Team, TeamDisplayDTO>.NewConfig()
                .Map(dest => dest.PlayerNames, src => src.Players.Select(player => player.Name));

            TypeAdapterConfig<Match, MatchDisplayDTO>.NewConfig()
                .Map(dest => dest.CompetitorOneName, src => src.CompetitorOne.Name)
                .Map(dest => dest.CompetitorTwoName, src => src.CompetitorTwo.Name)
                .Map(dest => dest.CompetitionName, src => src.Competition.Name)
                .Map(dest => dest.WinnerName, src => src.Winner == null ? null : src.Winner.Name);

            TypeAdapterConfig<Point, PointDisplayDTO>.NewConfig()
                .Map(dest => dest.PlayerName, src => src.Player.Name);

            TypeAdapterConfig<OneVSAllCompetition, OneVSAllCompetitionResponseDTO>.NewConfig()
                .Map(dest => dest.GameType, src => src.GameFormat.GameType)
                .Map(dest => dest.CompetitorType, src => src.GameFormat.CompetitorType)
                .Map(dest => dest.TeamSize, src => src.GameFormat.TeamSize)
                .Map(dest => dest.WinAt, src => src.GameFormat.WinAt)
                .Map(dest => dest.DurationInSeconds, src => src.GameFormat.DurationInSeconds)
                .Map(dest => dest.Competitors, src => src.Competitors.Adapt<List<CompetitorDisplayDTO>>())
                .Map(dest => dest.Matches, src => src.Matches.Adapt<List<MatchDisplayDTO>>());
            TypeAdapterConfig<OneVSAllCompetitionCreateDTO, OneVSAllCompetition>.NewConfig()
                .Map(dest => dest.GameFormat, src => (GameFormat)null!);

            TypeAdapterConfig<TournamentCompetition, TournamentCompetitionResponseDTO>.NewConfig()
                .Map(dest => dest.GameType, src => src.GameFormat.GameType)
                .Map(dest => dest.CompetitorType, src => src.GameFormat.CompetitorType)
                .Map(dest => dest.TeamSize, src => src.GameFormat.TeamSize)
                .Map(dest => dest.WinAt, src => src.GameFormat.WinAt)
                .Map(dest => dest.DurationInSeconds, src => src.GameFormat.DurationInSeconds)
                .Map(dest => dest.Competitors, src => src.Competitors.Adapt<List<CompetitorDisplayDTO>>())
                .Map(dest => dest.Matches, src => src.Matches.Adapt<List<MatchDisplayDTO>>());
            TypeAdapterConfig<CompetitionCreateDTO, TournamentCompetition>.NewConfig()
                .Map(dest => dest.GameFormat, src => (GameFormat)null!);

            TypeAdapterConfig<Player, PlayerGetDTO>.NewConfig()
                .Map(dest => dest.Matches, src => src.Matches.Adapt<List<MatchDisplayDTO>>())
                .Map(dest => dest.WonMatches, src => src.WonMatches.GetIds())
                .Map(dest => dest.Competitions, src => src.Competitions.Adapt<List<CompetitionDisplayDTO>>())
                .Map(dest => dest.Teams, src => src.Teams.Adapt<List<PointDisplayDTO>>())
                .Map(dest => dest.Points, src => src.Points.GetIds());

            TypeAdapterConfig<Team, TeamGetDTO>.NewConfig()
                .Map(dest => dest.Matches, src => src.Matches.Adapt<List<MatchDisplayDTO>>())
                .Map(dest => dest.WonMatches, src => src.WonMatches.GetIds())
                .Map(dest => dest.Competitions, src => src.Competitions.Adapt<List<CompetitionDisplayDTO>>())
                .Map(dest => dest.Players, src => src.Players.Adapt<List<PlayerDisplayDTO>>());

            TypeAdapterConfig<Match, MatchGetDTO>.NewConfig()
                .Map(dest => dest.CompetitorOne, src => src.CompetitorOne.Adapt<CompetitorDisplayDTO>())
                .Map(dest => dest.CompetitorTwo, src => src.CompetitorTwo.Adapt<CompetitorDisplayDTO>())
                .Map(dest => dest.Winner, src => src.Winner == null ? null : src.Winner.Adapt<CompetitorDisplayDTO>())
                .Map(dest => dest.Competition, src => src.Competition.Adapt<CompetitionDisplayDTO>())
                .Map(dest => dest.Points, src => src.Points.Adapt<List<PointDisplayDTO>>());

            TypeAdapterConfig<Point, PointGetDTO>.NewConfig()
                .Map(dest => dest.Player, src => src.Player.Adapt<PlayerDisplayDTO>())
                .Map(dest => dest.Match, src => src.Match.Id);

            TypeAdapterConfig<TeamPlayer, TeamPlayerGetDTO>.NewConfig()
                .Map(dest => dest.TeamId, src => src.Team.Id)
                .Map(dest => dest.PlayerId, src => src.Player.Id);

            //HACK added for tests

            TypeAdapterConfig<OneVSAllCompetition, OneVSAllCompetitionCreateDTO>.NewConfig()
                .Map(dest => dest.GameFormat, src => src.GameFormat.Id);

            TypeAdapterConfig<TournamentCompetition, TournamentCompetitionCreateDTO>.NewConfig()
                .Map(dest => dest.GameFormat, src => src.GameFormat.Id);
        }
    }
}
