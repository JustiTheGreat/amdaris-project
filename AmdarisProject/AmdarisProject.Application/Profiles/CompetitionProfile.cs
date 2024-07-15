using AmdarisProject.Application.Dtos.RequestDTOs.CreateDTOs;
using AmdarisProject.Application.Dtos.ResponseDTOs.CompetitionResponseDTOs;
using AmdarisProject.Application.Dtos.ResponseDTOs.DisplayDTOs;
using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models;
using AmdarisProject.Domain.Models.CompetitionModels;
using AmdarisProject.Domain.Models.CompetitorModels;
using AutoMapper;

namespace AmdarisProject.Application.Profiles
{
    internal class CompetitionProfile : Profile
    {
        public CompetitionProfile()
        {
            CreateMap<CompetitionCreateDTO, Competition>()
                .Include<CompetitionCreateDTO, OneVSAllCompetition>()
                .Include<CompetitionCreateDTO, TournamentCompetition>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.Empty))
                .ForMember(dest => dest.InitialStartTime, opt => opt.MapFrom(src => src.StartTime))
                .ForMember(dest => dest.ActualizedStartTime, opt => opt.MapFrom(src => src.StartTime))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => CompetitionStatus.ORGANIZING))
                .ForMember(dest => dest.GameFormat, opt => opt.MapFrom(src => (GameFormat)null!))
                .ForMember(dest => dest.Competitors, opt => opt.MapFrom(src => new List<Competitor>()))
                .ForMember(dest => dest.Matches, opt => opt.MapFrom(src => new List<Match>()));

            CreateMap<CompetitionCreateDTO, OneVSAllCompetition>();

            CreateMap<CompetitionCreateDTO, TournamentCompetition>()
                .ForMember(dest => dest.StageLevel, opt => opt.MapFrom(src => 0));

            CreateMap<Competition, CompetitionDisplayDTO>()
                .Include<OneVSAllCompetition, CompetitionDisplayDTO>()
                .Include<TournamentCompetition, CompetitionDisplayDTO>()
                .ForMember(dest => dest.CompetitionType, opt => opt.MapFrom(src =>
                    src is OneVSAllCompetition ? CompetitionType.ONE_VS_ALL.ToString()
                    : src is TournamentCompetition ? CompetitionType.TOURNAMENT.ToString()
                    : "UNKNOWN"))
                .ForMember(dest => dest.GameType, opt => opt.MapFrom(src => src.GameFormat.GameType.Name))
                .ForMember(dest => dest.CompetitorType, opt => opt.MapFrom(src => src.GameFormat.CompetitorType));

            CreateMap<OneVSAllCompetition, CompetitionDisplayDTO>();

            CreateMap<TournamentCompetition, CompetitionDisplayDTO>();

            CreateMap<Competition, CompetitionGetDTO>()
                .Include<OneVSAllCompetition, OneVSAllCompetitionGetDTO>()
                .Include<TournamentCompetition, TournamentCompetitionGetDTO>()
                .ForMember(dest => dest.GameType, opt => opt.MapFrom(src => src.GameFormat.GameType))
                .ForMember(dest => dest.CompetitorType, opt => opt.MapFrom(src => src.GameFormat.CompetitorType))
                .ForMember(dest => dest.TeamSize, opt => opt.MapFrom(src => src.GameFormat.TeamSize))
                .ForMember(dest => dest.WinAt, opt => opt.MapFrom(src => src.GameFormat.WinAt))
                .ForMember(dest => dest.DurationInMinutes, opt => opt.MapFrom(src => src.GameFormat.DurationInMinutes))
                .ForMember(dest => dest.CompetitionType, opt => opt.MapFrom(src =>
                    src is OneVSAllCompetition ? CompetitionType.ONE_VS_ALL.ToString()
                    : src is TournamentCompetition ? CompetitionType.TOURNAMENT.ToString()
                    : "UNKNOWN"));

            CreateMap<OneVSAllCompetition, OneVSAllCompetitionGetDTO>();

            CreateMap<TournamentCompetition, TournamentCompetitionGetDTO>();
        }
    }
}
