using AmdarisProject.Application.Dtos.DisplayDTOs.CompetitorDisplayDTOs;
using AmdarisProject.Application.Dtos.RequestDTOs.CreateDTOs;
using AmdarisProject.Application.Dtos.ResponseDTOs.CompetitionResponseDTOs;
using AmdarisProject.Application.Dtos.ResponseDTOs.CompetitorResponseDTOs;
using AmdarisProject.Domain.Extensions;
using AmdarisProject.Domain.Models;
using AmdarisProject.Domain.Models.CompetitionModels;
using AmdarisProject.Domain.Models.CompetitorModels;
using AutoMapper;

namespace AmdarisProject.Application.Profiles
{
    internal class PlayerProfile : Profile
    {
        public PlayerProfile()
        {
            //CreateMap<CompetitorCreateDTO, Player>()
            //    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.Empty))
            //    .ForMember(dest => dest.Matches, opt => opt.MapFrom(src => new List<Match>()))
            //    .ForMember(dest => dest.WonMatches, opt => opt.MapFrom(src => new List<Match>()))
            //    .ForMember(dest => dest.Competitions, opt => opt.MapFrom(src => new List<Competition>()))
            //    .ForMember(dest => dest.Points, opt => opt.MapFrom(src => new List<Point>()))
            //    .ForMember(dest => dest.Teams, opt => opt.MapFrom(src => new List<Team>()));

            //CreateMap<Player, PlayerDisplayDTO>()
            //    .IncludeBase<Player, CompetitorDisplayDTO>();

            //CreateMap<Player, PlayerGetDTO>()
            //    .IncludeBase<Player, CompetitorGetDTO>()
            //    .ForMember(dest => dest.WonMatches, opt => opt.MapFrom(src => src.WonMatches.GetIds()))
            //    .ForMember(dest => dest.Points, opt => opt.MapFrom(src => src.Points.GetIds()));
        }
    }
}
