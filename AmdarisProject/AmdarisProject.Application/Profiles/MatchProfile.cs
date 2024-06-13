using AmdarisProject.Application.Dtos.ResponseDTOs;
using AmdarisProject.Application.Dtos.ResponseDTOs.DisplayDTOs;
using AmdarisProject.Domain.Models;
using AutoMapper;

namespace AmdarisProject.Application.Profiles
{
    internal class MatchProfile : Profile
    {
        public MatchProfile()
        {
            CreateMap<Match, MatchDisplayDTO>()
                .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.ActualizedStartTime))
                .ForMember(dest => dest.Competitors, opt => opt.MapFrom(src => $"{src.CompetitorOne.Name} - {src.CompetitorTwo.Name}"))
                .ForMember(dest => dest.Score, opt => opt.MapFrom(src => $"{src.CompetitorOnePoints.ToString() ?? ""} - {src.CompetitorTwoPoints.ToString() ?? ""}"))
                .ForMember(dest => dest.Competition, opt => opt.MapFrom(src => src.Competition.Name))
                .ForMember(dest => dest.Winner, opt => opt.MapFrom(src => src.Winner == null ? null : src.Winner.Name));

            CreateMap<Match, MatchGetDTO>();
        }
    }
}
