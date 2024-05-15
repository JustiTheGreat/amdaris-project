using AmdarisProject.Application.Common.Abstractions;
using AmdarisProject.Application.Dtos.RequestDTOs;
using AmdarisProject.Presentation.Filters;
using Microsoft.AspNetCore.Mvc;

namespace AmdarisProject.Presentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController(IAuthenticationService authenticationService)
        : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService = authenticationService;

        [HttpPost]
        [Route(nameof(Register))]
        [ValidateModelState]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register(UserRegisterDTO userRegisterDTO)
        {
            var token = await _authenticationService.Register(userRegisterDTO);
            return Ok(token);
        }

        [HttpPost]
        [Route(nameof(Login))]
        [ValidateModelState]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login(UserLoginDTO userLoginDTO)
        {
            var token = await _authenticationService.Login(userLoginDTO);
            return Ok(token);
        }
    }
}
