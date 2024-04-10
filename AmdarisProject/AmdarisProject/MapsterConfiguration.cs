using AmdarisProject.Application.Dtos.CreateDTOs;
using AmdarisProject.Application.Dtos.CreateDTOs.CompetitionCreateDTOs;
using AmdarisProject.Application.Dtos.CreateDTOs.CompetitorCreateDTOs;
using AmdarisProject.Application.Dtos.ResponseDTOs;
using AmdarisProject.Application.Dtos.ResponseDTOs.CompetitionResponseDTOs;
using AmdarisProject.Application.Dtos.ResponseDTOs.CompetitorResponseDTOs;
using AmdarisProject.Application.Utils.ExtensionMethods;
using AmdarisProject.Domain.Models;
using AmdarisProject.Domain.Models.CompetitionModels;
using AmdarisProject.Domain.Models.CompetitorModels;
using Mapster;

namespace AmdarisProject.Presentation
{
    public static class MapsterConfiguration
    {
        public static void ConfigureMapster()
        {
            TypeAdapterConfig<OneVSAllCompetition, OneVSAllCompetitionResponseDTO>.NewConfig()
                .Map(dest => dest.Competitors, src => src.Competitors.GetIds())
                .Map(dest => dest.Matches, src => src.Matches.GetIds());
            TypeAdapterConfig<OneVSAllCompetitionCreateDTO, OneVSAllCompetition>.NewConfig()
                .Map(dest => dest.Competitors, src => new List<Competitor>())
                .Map(dest => dest.Matches, src => new List<Match>());

            TypeAdapterConfig<TournamentCompetition, TournamentCompetitionResponseDTO>.NewConfig()
                .Map(dest => dest.Competitors, src => src.Competitors.GetIds())
                .Map(dest => dest.Matches, src => src.Matches.GetIds())
                .Map(dest => dest.Stages, src => src.Stages.GetIds());
            TypeAdapterConfig<CompetitionCreateDTO, TournamentCompetition>.NewConfig()
                .Map(dest => dest.Competitors, src => new List<Competitor>())
                .Map(dest => dest.Matches, src => new List<Match>())
                .Map(dest => dest.Stages, src => new List<Stage>());

            TypeAdapterConfig<Player, PlayerResponseDTO>.NewConfig()
                .Map(dest => dest.Matches, src => src.Matches.GetIds())
                .Map(dest => dest.Competitions, src => src.Competitions.GetIds())
                .Map(dest => dest.Teams, src => src.Teams.GetIds())
                .Map(dest => dest.Points, src => src.Points.GetIds());
            TypeAdapterConfig<PlayerCreateDTO, Player>.NewConfig()
                .Map(dest => dest.Matches, src => new List<Match>())
                .Map(dest => dest.Competitions, src => new List<Competition>())
                .Map(dest => dest.Teams, src => new List<Team>())
                .Map(dest => dest.Points, src => new List<Point>());

            TypeAdapterConfig<Team, TeamResponseDTO>.NewConfig()
                .Map(dest => dest.Matches, src => src.Matches.GetIds())
                .Map(dest => dest.Competitions, src => src.Competitions.GetIds())
                .Map(dest => dest.Players, src => src.Players.GetIds());
            TypeAdapterConfig<TeamCreateDTO, Team>.NewConfig()
                .Map(dest => dest.Matches, src => new List<Match>())
                .Map(dest => dest.Competitions, src => new List<Competition>())
                .Map(dest => dest.Players, src => new List<Player>());

            TypeAdapterConfig<Match, MatchResponseDTO>.NewConfig()
                .Map(dest => dest.CompetitorOne, src => src.CompetitorOne.Id)
                .Map(dest => dest.CompetitorTwo, src => src.CompetitorTwo.Id)
                .Map(dest => dest.Competition, src => src.Competition.Id)
                .Map(dest => dest.Stage, src => src.Stage == null ? (ulong?)null : src.Stage.Id)
                .Map(dest => dest.Points, src => src.Points.GetIds());
            TypeAdapterConfig<MatchCreateDTO, Match>.NewConfig()
                .Map(dest => dest.CompetitorOne, src => new Player())
                .Map(dest => dest.CompetitorTwo, src => new Player())
                .Map(dest => dest.Competition, src => new OneVSAllCompetition())
                .Map(dest => dest.Stage, src => (Stage?)null)
                .Map(dest => dest.Points, src => new List<Point>());

            TypeAdapterConfig<Point, PointResponseDTO>.NewConfig()
                .Map(dest => dest.Player, src => src.Player.Id)
                .Map(dest => dest.Match, src => src.Match.Id);
            TypeAdapterConfig<PointCreateDTO, Point>.NewConfig()
                .Map(dest => dest.Player, src => new Player())
                .Map(dest => dest.Match, src => new Match());

            TypeAdapterConfig<Stage, StageResponseDTO>.NewConfig()
                .Map(dest => dest.Matches, src => src.Matches.GetIds())
                .Map(dest => dest.TournamentCompetition, src => src.TournamentCompetition.Id);
            TypeAdapterConfig<StageCreateDTO, Stage>.NewConfig()
                .Map(dest => dest.Matches, src => new List<Match>())
                .Map(dest => dest.TournamentCompetition, src => new TournamentCompetition());
        }
    }
}
