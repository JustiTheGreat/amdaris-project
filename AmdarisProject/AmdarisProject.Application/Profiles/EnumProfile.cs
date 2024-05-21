using AmdarisProject.Domain.Enums;
using AutoMapper;

namespace AmdarisProject.Application.Profiles
{
    internal class EnumProfile : Profile
    {
        public EnumProfile()
        {
            CreateMap<CompetitorType, CompetitorType>();
            CreateMap<CompetitionStatus, CompetitionStatus>();
            CreateMap<MatchStatus, MatchStatus>();
        }
    }
}
