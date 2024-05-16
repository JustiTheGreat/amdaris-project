using AmdarisProject.Application.Dtos.RequestDTOs;

namespace AmdarisProject.Application.Abstractions
{
    public interface IAuthenticationService
    {
        Task<string> Register(UserRegisterDTO userRegisterDTO);

        Task<string> Login(UserLoginDTO userLoginDTO);
    }
}