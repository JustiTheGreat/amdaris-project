using AmdarisProject.Application.Dtos.ResponseDTOs.DisplayDTOs;
using AmdarisProject.Domain.Models;
using AutoMapper;

namespace AmdarisProject.Application.Profiles
{
    internal class TeamPlayerProfile : Profile
    {
        public TeamPlayerProfile()
        {
            CreateMap<TeamPlayer, TeamPlayerDisplayDTO>()
                .ForMember(dest => dest.TeamId, opt => opt.MapFrom(src => src.Team.Id))
                .ForMember(dest => dest.Team, opt => opt.MapFrom(src => src.Team.Name))
                .ForMember(dest => dest.PlayerId, opt => opt.MapFrom(src => src.Player.Id))
                .ForMember(dest => dest.Player, opt => opt.MapFrom(src => src.Player.Name));
        }
    }
}
