using AmdarisProject.Application.Dtos.DisplayDTOs.CompetitorDisplayDTOs;
using AmdarisProject.Application.Dtos.RequestDTOs.CreateDTOs;
using AmdarisProject.Application.Dtos.ResponseDTOs.CompetitorResponseDTOs;
using AmdarisProject.Domain.Extensions;
using AmdarisProject.Domain.Models;
using AmdarisProject.Domain.Models.CompetitionModels;
using AmdarisProject.Domain.Models.CompetitorModels;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                .ForMember(dest => dest.Competitions, opt => opt.MapFrom(src => new List<Competition>()));

            CreateMap<CompetitorCreateDTO, Player>()
               .ForMember(dest => dest.Points, opt => opt.MapFrom(src => new List<Point>()))
               .ForMember(dest => dest.Teams, opt => opt.MapFrom(src => new List<Team>()));

            CreateMap<CompetitorCreateDTO, Team>()
               .ForMember(dest => dest.Players, opt => opt.MapFrom(src => new List<Player>()));

            CreateMap<Competitor, CompetitorDisplayDTO>()
                .Include<Player, PlayerDisplayDTO>()
                .Include<Team, TeamDisplayDTO>();

            CreateMap<Player, PlayerDisplayDTO>();

            CreateMap<Team, TeamDisplayDTO>()
                .ForMember(dest => dest.PlayerNames, opt => opt.MapFrom(src => src.Players.Select(player => player.Name)));

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
