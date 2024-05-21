using AmdarisProject.Application.Dtos.RequestDTOs.CreateDTOs;
using AmdarisProject.Application.Dtos.ResponseDTOs;
using AmdarisProject.Domain.Models;
using AutoMapper;

namespace AmdarisProject.Application.Profiles
{
    internal class GameFormatProfile : Profile
    {
        public GameFormatProfile()
        {
            CreateMap<GameFormatCreateDTO, GameFormat>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.Empty))
                .ForMember(dest => dest.GameType, opt => opt.MapFrom(src => (GameType?)null));

            CreateMap<GameFormat, GameFormatGetDTO>();
        }
    }
}
