using AmdarisProject.Application.Dtos.DisplayDTOs.CompetitorDisplayDTOs;
using AmdarisProject.Application.Dtos.RequestDTOs.CreateDTOs;
using AmdarisProject.Application.Dtos.ResponseDTOs.CompetitorResponseDTOs;
using AmdarisProject.Domain.Extensions;
using AmdarisProject.Domain.Models;
using AmdarisProject.Domain.Models.CompetitionModels;
using AmdarisProject.Domain.Models.CompetitorModels;
using AutoMapper;

namespace AmdarisProject.Application.Profiles
{
    internal class TeamProfile : Profile
    {
        public TeamProfile()
        {
            //CreateMap<CompetitorCreateDTO, Team>()
            //    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.Empty))
            //    .ForMember(dest => dest.Matches, opt => opt.MapFrom(src => new List<Match>()))
            //    .ForMember(dest => dest.WonMatches, opt => opt.MapFrom(src => new List<Match>()))
            //    .ForMember(dest => dest.Competitions, opt => opt.MapFrom(src => new List<Competition>()))
            //    .ForMember(dest => dest.Players, opt => opt.MapFrom(src => new List<Player>()));

            //CreateMap<Team, TeamDisplayDTO>()
            //    .IncludeBase<Team, CompetitorDisplayDTO>()
            //    .ForMember(dest => dest.PlayerNames, opt => opt.MapFrom(src => src.Players.Select(player => player.Name)));

            //CreateMap<Team, TeamGetDTO>()
            //    .IncludeBase<Team, CompetitorGetDTO>()
            //    .ForMember(dest => dest.WonMatches, opt => opt.MapFrom(src => src.WonMatches.GetIds()));
        }
    }
}
