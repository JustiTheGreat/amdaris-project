using AmdarisProject.Application.Dtos.DisplayDTOs;
using AmdarisProject.Application.Dtos.RequestDTOs.CreateDTOs;
using AmdarisProject.Application.Dtos.ResponseDTOs.CompetitionResponseDTOs;
using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Models;
using AmdarisProject.Domain.Models.CompetitionModels;
using AmdarisProject.Domain.Models.CompetitorModels;
using AutoMapper;

namespace AmdarisProject.Application.Profiles
{
    internal class OneVSAllCompetitionProfile : Profile
    {
        public OneVSAllCompetitionProfile()
        {
            CreateMap<OneVSAllCompetition, CompetitionDisplayDTO>()
                .ForMember(dest => dest.GameType, opt => opt.MapFrom(src => src.GameFormat.GameType))
                .ForMember(dest => dest.CompetitorType, opt => opt.MapFrom(src => src.GameFormat.CompetitorType));

            CreateMap<OneVSAllCompetition, OneVSAllCompetitionGetDTO>()
                .ForMember(dest => dest.GameType, opt => opt.MapFrom(src => src.GameFormat.GameType))
                .ForMember(dest => dest.CompetitorType, opt => opt.MapFrom(src => src.GameFormat.CompetitorType))
                .ForMember(dest => dest.TeamSize, opt => opt.MapFrom(src => src.GameFormat.TeamSize))
                .ForMember(dest => dest.WinAt, opt => opt.MapFrom(src => src.GameFormat.WinAt))
                .ForMember(dest => dest.DurationInMinutes, opt => opt.MapFrom(src => src.GameFormat.DurationInMinutes));

            CreateMap<CompetitionCreateDTO, OneVSAllCompetition>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.Empty))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => CompetitionStatus.ORGANIZING))
                .ForMember(dest => dest.GameFormat, opt => opt.MapFrom(src => (GameFormat)null!))
                .ForMember(dest => dest.Competitors, opt => opt.MapFrom(src => new List<Competitor>()))
                .ForMember(dest => dest.Matches, opt => opt.MapFrom(src => new List<Match>()));
        }
    }
}
