using AmdarisProject.Application.Dtos.RequestDTOs;
using AmdarisProject.Presentation.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AmdarisProject.Application.Abstractions;
using AmdarisProject.Infrastructure.Persistance.Contexts;
using MediatR;

namespace AmdarisProject.Presentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [AllowAnonymous]
    [ValidateModelState]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class UserController(IMediator mediator, IAuthenticationService authenticationService, AmdarisProjectDBContext dbContext)
        : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        private readonly IAuthenticationService _authenticationService = authenticationService;
        private readonly AmdarisProjectDBContext _dbContext = dbContext;

        [HttpPost]
        [Route(nameof(Register))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Register(UserRegisterDTO userRegisterDTO)
        {
            string token = await _authenticationService.Register(userRegisterDTO);
            return Ok(token);
        }

        [HttpPost]
        [Route(nameof(Login))]
        [ValidateModelState]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Login(UserLoginDTO userLoginDTO)
        {
            string token = await _authenticationService.Login(userLoginDTO);
            return Ok(token);
        }
    }
}
