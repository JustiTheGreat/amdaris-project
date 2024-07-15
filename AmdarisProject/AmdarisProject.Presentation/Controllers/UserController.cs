using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Dtos.RequestDTOs;
using AmdarisProject.Infrastructure.Identity;
using AmdarisProject.Infrastructure.Persistance.Contexts;
using AmdarisProject.Presentation.Filters;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using AmdarisProject.Domain.Exceptions;

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
        public async Task<IActionResult> Register([FromForm] UserRegisterDTO userRegisterDTO)
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

        [Authorize(Roles = nameof(UserRole.User))]
        [HttpPost]
        [Route(nameof(UpdateProfile))]
        [ValidateModelState]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateProfile([FromForm] UpdateProfileDTO updateProfileDTO)
        {
            string email = User.FindFirstValue(ClaimIndetifiers.Email)
                ?? throw new APException("Email not present in token!");

            string token = await _authenticationService.UpdateProfile(email, updateProfileDTO);
            return Ok(token);
        }
    }
}
