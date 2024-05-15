using AmdarisProject.Application.Dtos.RequestDTOs;

namespace AmdarisProject.Application.Common.Abstractions
{
    public interface IAuthenticationService
    {
        Task<string> Register(UserRegisterDTO userRegisterDTO);

        Task<string> Login(UserLoginDTO userLoginDTO);
    }
}