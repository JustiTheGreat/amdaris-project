using AmdarisProject.Application.Dtos.ResponseDTOs.DisplayDTOs;
using AmdarisProject.Domain.Models;
using AutoMapper;

namespace AmdarisProject.Application.Profiles
{
    internal class TeamPlayerProfile : Profile
    {
        public TeamPlayerProfile()
        {
            CreateMap<TeamPlayer, TeamPlayerDisplayDTO>()
                .ForMember(dest => dest.TeamId, opt => opt.MapFrom(src => src.Team.Id))
                .ForMember(dest => dest.Team, opt => opt.MapFrom(src => src.Team.Name))
                .ForMember(dest => dest.PlayerId, opt => opt.MapFrom(src => src.Player.Id))
                .ForMember(dest => dest.Player, opt => opt.MapFrom(src => src.Player.Name))
                .ForMember(dest => dest.NumberOfCompetitions, opt => opt.MapFrom(src => src.Player.Competitions.Count()))
                .ForMember(dest => dest.NumberOfMatches, opt => opt.MapFrom(src => src.Player.Matches.Count()))
                .ForMember(dest => dest.NumberOfTeams, opt => opt.MapFrom(src => src.Player.Teams.Count()))
                .ForMember(dest => dest.ProfilePicture, opt => opt.MapFrom(src => src.Player.ProfilePictureUri));
        }
    }
}
