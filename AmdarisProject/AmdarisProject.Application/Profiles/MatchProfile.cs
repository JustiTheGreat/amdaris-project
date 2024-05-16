using AmdarisProject.Application.Dtos.DisplayDTOs;
using AmdarisProject.Application.Dtos.ResponseDTOs;
using AmdarisProject.Domain.Models;
using AutoMapper;

namespace AmdarisProject.Application.Profiles
{
    internal class MatchProfile : Profile
    {
        public MatchProfile()
        {
            CreateMap<Match, MatchDisplayDTO>()
                    .ForMember(dest => dest.CompetitorOneName, opt => opt.MapFrom(src => src.CompetitorOne.Name))
                    .ForMember(dest => dest.CompetitorTwoName, opt => opt.MapFrom(src => src.CompetitorTwo.Name))
                    .ForMember(dest => dest.CompetitionName, opt => opt.MapFrom(src => src.Competition.Name))
                    .ForMember(dest => dest.WinnerName, opt => opt.MapFrom(src => src.Winner == null ? null : src.Winner.Name));

            CreateMap<Match, MatchGetDTO>();
        }
    }
}
