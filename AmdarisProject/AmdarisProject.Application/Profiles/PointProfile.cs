using AmdarisProject.Application.Dtos.ResponseDTOs;
using AmdarisProject.Application.Dtos.ResponseDTOs.DisplayDTOs;
using AmdarisProject.Domain.Models;
using AutoMapper;

namespace AmdarisProject.Application.Profiles
{
    internal class PointProfile : Profile
    {
        public PointProfile()
        {
            CreateMap<Point, PointDisplayDTO>()
                .ForMember(dest => dest.MatchId, opt => opt.MapFrom(src => src.Match.Id))
                .ForMember(dest => dest.PlayerId, opt => opt.MapFrom(src => src.Player.Id))
                .ForMember(dest => dest.Player, opt => opt.MapFrom(src => src.Player.Name))
                .ForMember(dest => dest.ProfilePicture, opt => opt.MapFrom(src => src.Player.ProfilePictureUri));

            CreateMap<Point, PointGetDTO>()
                .ForMember(dest => dest.Match, opt => opt.MapFrom(src => src.Match.Id))
                .ForMember(dest => dest.ProfilePicture, opt => opt.MapFrom(src => src.Player.ProfilePictureUri));
        }
    }
}
