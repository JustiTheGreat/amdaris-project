using AmdarisProject.Application.Dtos.ResponseDTOs.GetDTOs;
using AmdarisProject.Domain.Models;
using AutoMapper;
namespace AmdarisProject.Application.Profiles
{
    internal class GameTypeProfile : Profile
    {
        public GameTypeProfile()
        {
            CreateMap<GameType, GameTypeGetDTO>();
        }
    }
}
