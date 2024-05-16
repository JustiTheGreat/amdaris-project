using AmdarisProject.Domain.Enums;
using AutoMapper;

namespace AmdarisProject.Application.Profiles
{
    internal class EnumProfile : Profile
    {
        public EnumProfile()
        {
            CreateMap<GameType, GameType>();
            CreateMap<CompetitorType, CompetitorType>();
            CreateMap<CompetitionStatus, CompetitionStatus>();
            CreateMap<MatchStatus, MatchStatus>();
        }
    }
}
