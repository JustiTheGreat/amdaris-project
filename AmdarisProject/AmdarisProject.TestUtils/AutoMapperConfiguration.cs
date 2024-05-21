using AmdarisProject.Application;
using AmdarisProject.Application.Dtos.RequestDTOs.CreateDTOs;
using AmdarisProject.Domain.Models;
using AmdarisProject.Domain.Models.CompetitionModels;
using AmdarisProject.Domain.Models.CompetitorModels;
using AutoMapper;

namespace AmdarisProject.TestUtils
{
    public class AutoMapperConfiguration
    {
        public static IMapper GetMapper()
            => new MapperConfiguration(cfg =>
            {
                cfg.AddMaps(typeof(AutoMapperProfileAssemblyMarker).Assembly);

                cfg.CreateMap<OneVSAllCompetition, CompetitionCreateDTO>()
                    .ForMember(dest => dest.GameFormat, opt => opt.MapFrom(src => src.GameFormat.Id));
                cfg.CreateMap<TournamentCompetition, CompetitionCreateDTO>()
                    .ForMember(dest => dest.GameFormat, opt => opt.MapFrom(src => src.GameFormat.Id));
                cfg.CreateMap<Player, CompetitorCreateDTO>();
                cfg.CreateMap<Team, CompetitorCreateDTO>();
                cfg.CreateMap<GameFormat, GameFormatCreateDTO>()
                    .ForMember(dest => dest.GameType, opt => opt.MapFrom(src => src.GameType.Id)); ;
            }).CreateMapper();
    }
}
