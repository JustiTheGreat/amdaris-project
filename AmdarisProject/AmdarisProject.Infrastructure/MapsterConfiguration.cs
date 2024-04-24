using AmdarisProject.Application.Dtos.CreateDTOs;
using AmdarisProject.Application.Dtos.CreateDTOs.CompetitionCreateDTOs;
using AmdarisProject.Application.Dtos.CreateDTOs.CompetitorCreateDTOs;
using AmdarisProject.Application.Dtos.DisplayDTOs;
using AmdarisProject.Application.Dtos.DisplayDTOs.CompetitorDisplayDTOs;
using AmdarisProject.Application.Dtos.ResponseDTOs;
using AmdarisProject.Application.Dtos.ResponseDTOs.CompetitionResponseDTOs;
using AmdarisProject.Application.Dtos.ResponseDTOs.CompetitorResponseDTOs;
using AmdarisProject.Application.ExtensionMethods;
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
                .Map(dest => dest.GameFormat, src => new GameFormat())
                .Map(dest => dest.Competitors, src => new List<Competitor>())
                .Map(dest => dest.Matches, src => new List<Match>());

            config.ForType<TournamentCompetition, TournamentCompetitionResponseDTO>()
                .Map(dest => dest.GameType, src => src.GameFormat.GameType)
                .Map(dest => dest.CompetitorType, src => src.GameFormat.CompetitorType)
                .Map(dest => dest.TeamSize, src => src.GameFormat.TeamSize)
                .Map(dest => dest.WinAt, src => src.GameFormat.WinAt)
                .Map(dest => dest.DurationInSeconds, src => src.GameFormat.DurationInSeconds)
                .Map(dest => dest.Competitors, src => src.Competitors.Adapt<List<CompetitorDisplayDTO>>())
                .Map(dest => dest.Matches, src => src.Matches.Adapt<List<MatchDisplayDTO>>());
            config.ForType<CompetitionCreateDTO, TournamentCompetition>()
                .Map(dest => dest.GameFormat, src => new GameFormat())
                .Map(dest => dest.Competitors, src => new List<Competitor>())
                .Map(dest => dest.Matches, src => new List<Match>());

            config.ForType<Player, PlayerResponseDTO>()
                .Map(dest => dest.Matches, src => src.Matches.Adapt<List<MatchDisplayDTO>>())
                .Map(dest => dest.WonMatches, src => src.WonMatches.GetIds())
                .Map(dest => dest.Competitions, src => src.Competitions.Adapt<List<CompetitionDisplayDTO>>())
                .Map(dest => dest.Teams, src => src.Teams.Adapt<List<PointDisplayDTO>>())
                .Map(dest => dest.Points, src => src.Points.GetIds());
            config.ForType<PlayerCreateDTO, Player>()
                .Map(dest => dest.Matches, src => new List<Match>())
                .Map(dest => dest.WonMatches, src => new List<Match>())
                .Map(dest => dest.Competitions, src => new List<Competition>())
                .Map(dest => dest.Teams, src => new List<Team>())
                .Map(dest => dest.Points, src => new List<Point>());

            config.ForType<Team, TeamResponseDTO>()
                .Map(dest => dest.Matches, src => src.Matches.Adapt<List<MatchDisplayDTO>>())
                .Map(dest => dest.WonMatches, src => src.WonMatches.GetIds())
                .Map(dest => dest.Competitions, src => src.Competitions.Adapt<List<CompetitionDisplayDTO>>())
                .Map(dest => dest.Players, src => src.Players.Adapt<List<PlayerDisplayDTO>>());
            config.ForType<TeamCreateDTO, Team>()
                .Map(dest => dest.Matches, src => new List<Match>())
                .Map(dest => dest.WonMatches, src => new List<Match>())
                .Map(dest => dest.Competitions, src => new List<Competition>())
                .Map(dest => dest.Players, src => new List<Player>());

            config.ForType<Match, MatchResponseDTO>()
                .Map(dest => dest.CompetitorOne, src => src.CompetitorOne.Adapt<CompetitorDisplayDTO>())
                .Map(dest => dest.CompetitorTwo, src => src.CompetitorTwo.Adapt<CompetitorDisplayDTO>())
                .Map(dest => dest.Winner, src => src.Winner == null ? null : src.Winner.Adapt<CompetitorDisplayDTO>())
                .Map(dest => dest.Competition, src => src.Competition.Adapt<CompetitionDisplayDTO>())
                .Map(dest => dest.Points, src => src.Points.Adapt<List<PointDisplayDTO>>());
            config.ForType<MatchCreateDTO, Match>()
                .Map(dest => dest.CompetitorOne, src => new Player())
                .Map(dest => dest.CompetitorTwo, src => new Player())
                .Map(dest => dest.CompetitorTwo, src => new Player())
                .Map(dest => dest.Competition, src => new OneVSAllCompetition())
                .Map(dest => dest.Points, src => new List<Point>());

            config.ForType<Point, PointResponseDTO>()
                .Map(dest => dest.Player, src => src.Player.Adapt<PlayerDisplayDTO>())
                .Map(dest => dest.Match, src => src.Match.Id);
            config.ForType<PointCreateDTO, Point>()
                .Map(dest => dest.Player, src => new Player())
                .Map(dest => dest.Match, src => new Match());

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
                .Map(dest => dest.GameFormat, src => new GameFormat())
                .Map(dest => dest.Competitors, src => new List<Competitor>())
                .Map(dest => dest.Matches, src => new List<Match>());

            TypeAdapterConfig<TournamentCompetition, TournamentCompetitionResponseDTO>.NewConfig()
                .Map(dest => dest.GameType, src => src.GameFormat.GameType)
                .Map(dest => dest.CompetitorType, src => src.GameFormat.CompetitorType)
                .Map(dest => dest.TeamSize, src => src.GameFormat.TeamSize)
                .Map(dest => dest.WinAt, src => src.GameFormat.WinAt)
                .Map(dest => dest.DurationInSeconds, src => src.GameFormat.DurationInSeconds)
                .Map(dest => dest.Competitors, src => src.Competitors.Adapt<List<CompetitorDisplayDTO>>())
                .Map(dest => dest.Matches, src => src.Matches.Adapt<List<MatchDisplayDTO>>());
            TypeAdapterConfig<CompetitionCreateDTO, TournamentCompetition>.NewConfig()
                .Map(dest => dest.GameFormat, src => new GameFormat())
                .Map(dest => dest.Competitors, src => new List<Competitor>())
                .Map(dest => dest.Matches, src => new List<Match>());

            TypeAdapterConfig<Player, PlayerResponseDTO>.NewConfig()
                .Map(dest => dest.Matches, src => src.Matches.Adapt<List<MatchDisplayDTO>>())
                .Map(dest => dest.WonMatches, src => src.WonMatches.GetIds())
                .Map(dest => dest.Competitions, src => src.Competitions.Adapt<List<CompetitionDisplayDTO>>())
                .Map(dest => dest.Teams, src => src.Teams.Adapt<List<PointDisplayDTO>>())
                .Map(dest => dest.Points, src => src.Points.GetIds());
            TypeAdapterConfig<PlayerCreateDTO, Player>.NewConfig()
                .Map(dest => dest.Matches, src => new List<Match>())
                .Map(dest => dest.WonMatches, src => new List<Match>())
                .Map(dest => dest.Competitions, src => new List<Competition>())
                .Map(dest => dest.Teams, src => new List<Team>())
                .Map(dest => dest.Points, src => new List<Point>());

            TypeAdapterConfig<Team, TeamResponseDTO>.NewConfig()
                .Map(dest => dest.Matches, src => src.Matches.Adapt<List<MatchDisplayDTO>>())
                .Map(dest => dest.WonMatches, src => src.WonMatches.GetIds())
                .Map(dest => dest.Competitions, src => src.Competitions.Adapt<List<CompetitionDisplayDTO>>())
                .Map(dest => dest.Players, src => src.Players.Adapt<List<PlayerDisplayDTO>>());
            TypeAdapterConfig<TeamCreateDTO, Team>.NewConfig()
                .Map(dest => dest.Matches, src => new List<Match>())
                .Map(dest => dest.WonMatches, src => new List<Match>())
                .Map(dest => dest.Competitions, src => new List<Competition>())
                .Map(dest => dest.Players, src => new List<Player>());

            TypeAdapterConfig<Match, MatchResponseDTO>.NewConfig()
                .Map(dest => dest.CompetitorOne, src => src.CompetitorOne.Adapt<CompetitorDisplayDTO>())
                .Map(dest => dest.CompetitorTwo, src => src.CompetitorTwo.Adapt<CompetitorDisplayDTO>())
                .Map(dest => dest.Winner, src => src.Winner == null ? null : src.Winner.Adapt<CompetitorDisplayDTO>())
                .Map(dest => dest.Competition, src => src.Competition.Adapt<CompetitionDisplayDTO>())
                .Map(dest => dest.Points, src => src.Points.Adapt<List<PointDisplayDTO>>());
            TypeAdapterConfig<MatchCreateDTO, Match>.NewConfig()
                .Map(dest => dest.CompetitorOne, src => new Player())
                .Map(dest => dest.CompetitorTwo, src => new Player())
                .Map(dest => dest.CompetitorTwo, src => new Player())
                .Map(dest => dest.Competition, src => new OneVSAllCompetition())
                .Map(dest => dest.Points, src => new List<Point>());

            TypeAdapterConfig<Point, PointResponseDTO>.NewConfig()
                .Map(dest => dest.Player, src => src.Player.Adapt<PlayerDisplayDTO>())
                .Map(dest => dest.Match, src => src.Match.Id);
            TypeAdapterConfig<PointCreateDTO, Point>.NewConfig()
                .Map(dest => dest.Player, src => new Player())
                .Map(dest => dest.Match, src => new Match());

            //HACK added for tests

            TypeAdapterConfig<OneVSAllCompetition, OneVSAllCompetitionCreateDTO>.NewConfig()
                .Map(dest => dest.GameFormat, src => src.GameFormat.Id)
                .Map(dest => dest.Competitors, src => src.Competitors.GetIds())
                .Map(dest => dest.Matches, src => src.Matches.GetIds());

            TypeAdapterConfig<TournamentCompetition, TournamentCompetitionCreateDTO>.NewConfig()
                .Map(dest => dest.GameFormat, src => src.GameFormat.Id)
                .Map(dest => dest.Competitors, src => src.Competitors.GetIds())
                .Map(dest => dest.Matches, src => src.Matches.GetIds());

            TypeAdapterConfig<Player, PlayerCreateDTO>.NewConfig()
                .Map(dest => dest.Matches, src => src.Matches.GetIds())
                .Map(dest => dest.WonMatches, src => src.WonMatches.GetIds())
                .Map(dest => dest.Competitions, src => src.Competitions.GetIds())
                .Map(dest => dest.Teams, src => src.Teams.GetIds())
                .Map(dest => dest.Points, src => src.Points.GetIds());

            TypeAdapterConfig<Team, TeamCreateDTO>.NewConfig()
                .Map(dest => dest.Matches, src => src.Matches.GetIds())
                .Map(dest => dest.WonMatches, src => src.WonMatches.GetIds())
                .Map(dest => dest.Competitions, src => src.Competitions.GetIds())
                .Map(dest => dest.Players, src => src.Players.GetIds());

            TypeAdapterConfig<Match, MatchCreateDTO>.NewConfig()
                .Map(dest => dest.CompetitorOne, src => src.CompetitorOne.Id)
                .Map(dest => dest.CompetitorTwo, src => src.CompetitorTwo.Id)
                .Map(dest => dest.Winner, src => src.Winner == null ? (Guid?)null : src.Winner.Id)
                .Map(dest => dest.Competition, src => src.Competition.Id)
                .Map(dest => dest.Points, src => src.Points.GetIds());

            TypeAdapterConfig<Point, PointCreateDTO>.NewConfig()
                .Map(dest => dest.Player, src => src.Player.Id)
                .Map(dest => dest.Match, src => src.Match.Id);
        }
    }
}
