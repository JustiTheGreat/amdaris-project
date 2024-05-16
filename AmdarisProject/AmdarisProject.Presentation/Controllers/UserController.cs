using AmdarisProject.Application.Dtos.RequestDTOs;
using AmdarisProject.Application.Handlers.AuthenticationHandlers;
using AmdarisProject.Presentation.Filters;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AmdarisProject.Presentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [AllowAnonymous]
    [ValidateModelState]
    public class UserController(IMediator mediator)
        : ControllerBase
    {
        private readonly IMediator _mediator = mediator;


        [HttpPost]
        [Route(nameof(Register))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register(UserRegisterDTO userRegisterDTO)
        {
            var token = await _mediator.Send(new Register(userRegisterDTO));
            return Ok(token);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route(nameof(Login))]
        [ValidateModelState]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login(UserLoginDTO userLoginDTO)
        {
            var token = await _mediator.Send(new Login(userLoginDTO));
            return Ok(token);
        }
    }
}
