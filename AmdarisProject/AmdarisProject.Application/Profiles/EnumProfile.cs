using AmdarisProject.Domain.Enums;
using AutoMapper;

namespace AmdarisProject.Application.Profiles
{
    internal class EnumProfile : Profile
    {
        public EnumProfile()
        {
            CreateMap<CompetitorType, string>()
                .ConvertUsing(src => src.ToString());
            CreateMap<CompetitionStatus, string>()
                .ConvertUsing(src => src.ToString());
            CreateMap<MatchStatus, string>()
                .ConvertUsing(src => src.ToString());
        }
    }
}
