using AmdarisProject.Application.Dtos.ResponseDTOs.GetDTOs;
using AmdarisProject.Domain.Models.CompetitorModels;
using AutoMapper;

namespace AmdarisProject.Application.Profiles
{
    internal class TeamPlayerProfile : Profile
    {
        public TeamPlayerProfile()
        {
            CreateMap<TeamPlayer, TeamPlayerGetDTO>()
                    .ForMember(dest => dest.TeamId, opt => opt.MapFrom(src => src.Team.Id))
                    .ForMember(dest => dest.PlayerId, opt => opt.MapFrom(src => src.Player.Id));
        }
    }
}
