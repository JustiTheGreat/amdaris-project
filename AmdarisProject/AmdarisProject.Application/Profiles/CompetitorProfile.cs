using AmdarisProject.Application.Dtos.RequestDTOs.CreateDTOs;
using AmdarisProject.Application.Dtos.ResponseDTOs.CompetitorResponseDTOs;
using AmdarisProject.Application.Dtos.ResponseDTOs.DisplayDTOs;
using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Extensions;
using AmdarisProject.Domain.Models;
using AmdarisProject.Domain.Models.CompetitionModels;
using AmdarisProject.Domain.Models.CompetitorModels;
using AutoMapper;

namespace AmdarisProject.Application.Profiles
{
    internal class CompetitorProfile : Profile
    {
        public CompetitorProfile()
        {
            CreateMap<CompetitorCreateDTO, Competitor>()
                .Include<CompetitorCreateDTO, Player>()
                .Include<CompetitorCreateDTO, Team>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.Empty))
                .ForMember(dest => dest.Matches, opt => opt.MapFrom(src => new List<Match>()))
                .ForMember(dest => dest.WonMatches, opt => opt.MapFrom(src => new List<Match>()))
                .ForMember(dest => dest.Competitions, opt => opt.MapFrom(src => new List<Competition>()))
                .ForMember(dest => dest.TeamPlayers, opt => opt.MapFrom(src => new List<TeamPlayer>()));

            CreateMap<CompetitorCreateDTO, Player>()
               .ForMember(dest => dest.Points, opt => opt.MapFrom(src => new List<Point>()))
               .ForMember(dest => dest.Teams, opt => opt.MapFrom(src => new List<Team>()));

            CreateMap<CompetitorCreateDTO, Team>()
               .ForMember(dest => dest.Players, opt => opt.MapFrom(src => new List<Player>()));

            CreateMap<Competitor, CompetitorDisplayDTO>()
                .Include<Player, CompetitorDisplayDTO>()
                .Include<Team, CompetitorDisplayDTO>()
                .ForMember(dest => dest.CompetitorType, opt => opt.MapFrom(src =>
                    src is Player ? CompetitorType.PLAYER.ToString()
                    : src is Team ? CompetitorType.TEAM.ToString()
                    : "UNKNOWN"))
                .ForMember(dest => dest.NumberOfCompetitions, opt => opt.MapFrom(src => src.Competitions.Count()))
                .ForMember(dest => dest.NumberOfMatches, opt => opt.MapFrom(src => src.Matches.Count()));

            CreateMap<Player, CompetitorDisplayDTO>()
                .ForMember(dest => dest.NumberOfTeams, opt => opt.MapFrom(src => src.Teams.Count()));

            CreateMap<Team, CompetitorDisplayDTO>()
                .ForMember(dest => dest.NumberOfPlayers, opt => opt.MapFrom(src => src.TeamPlayers.Count()))
                .ForMember(dest => dest.NumberOfActivePlayers, opt => opt.MapFrom(src => src.TeamPlayers.Count(teamPlayer => teamPlayer.IsActive)));

            CreateMap<Competitor, CompetitorGetDTO>()
                .Include<Player, PlayerGetDTO>()
                .Include<Team, TeamGetDTO>()
                .ForMember(dest => dest.WonMatches, opt => opt.MapFrom(src => src.WonMatches.GetIds()));

            CreateMap<Player, PlayerGetDTO>()
                .ForMember(dest => dest.Points, opt => opt.MapFrom(src => src.Points.GetIds()));

            CreateMap<Team, TeamGetDTO>();
        }
    }
}
