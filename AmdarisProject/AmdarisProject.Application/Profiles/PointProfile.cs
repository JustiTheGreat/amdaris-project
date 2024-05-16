using AmdarisProject.Application.Dtos.DisplayDTOs;
using AmdarisProject.Application.Dtos.ResponseDTOs;
using AmdarisProject.Domain.Models;
using AutoMapper;

namespace AmdarisProject.Application.Profiles
{
    internal class PointProfile : Profile
    {
        public PointProfile()
        {
            CreateMap<Point, PointDisplayDTO>()
               .ForMember(dest => dest.PlayerName, opt => opt.MapFrom(src => src.Player.Name));

            CreateMap<Point, PointGetDTO>()
                .ForMember(dest => dest.Match, opt => opt.MapFrom(src => src.Match.Id));
        }
    }
}
