using AmdarisProject.Application.Dtos.RequestDTOs;
using AmdarisProject.Application.Dtos.ResponseDTOs.GetDTOs;
using AmdarisProject.Application.Handlers.TeamPlayerHandlers;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Infrastructure.Identity;
using AmdarisProject.Presentation.Filters;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AmdarisProject.Presentation.Controllers
{
    [Authorize(Roles = $"{nameof(UserRole.Administrator)}, {nameof(UserRole.User)}")]
    [ApiController]
    [Route("[controller]")]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class TeamPlayerController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost(nameof(AddPlayerToTeam) + "/{playerId}/{teamId}")]
        [ValidateGuid]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> AddPlayerToTeam([FromRoute] Guid teamId, [FromRoute] Guid playerId)
        {
            await _mediator.Send(new AddPlayerToTeam(teamId, playerId));
            return Created();
        }

        [HttpPut(nameof(ChangeTeamPlayerStatus) + "/{playerId}/{teamId}")]
        [ValidateGuid]
        [ProducesResponseType(typeof(TeamPlayerGetDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> ChangeTeamPlayerStatus([FromRoute] Guid teamId, [FromRoute] Guid playerId)
        {
            TeamPlayerGetDTO response = await _mediator.Send(new ChangeTeamPlayerStatus(teamId, playerId));
            return Ok(response);
        }

        [HttpDelete(nameof(RemovePlayerFromTeam))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> RemovePlayerFromTeam([FromBody] UserRequestDTO userRequestDTO)
        {
            if (User.IsInRole(nameof(UserRole.Administrator)) && userRequestDTO.PlayerId is null)
                throw new APArgumentException(nameof(userRequestDTO.PlayerId));

            Guid playerId = User.IsInRole(nameof(UserRole.Administrator))
                ? (Guid)userRequestDTO.PlayerId!
                : Guid.Parse(User.FindFirstValue(ClaimIndetifiers.PlayerId) ?? throw new APArgumentException(nameof(User.Claims)));

            await _mediator.Send(new RemovePlayerFromTeam(userRequestDTO.OtherId, playerId));
            return NoContent();
        }

        //[HttpDelete(nameof(RemovePlayerFromTeam) + "/{playerId}/{teamId}")]
        //[ProducesResponseType(StatusCodes.Status204NoContent)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //public async Task<ActionResult> RemovePlayerFromTeam([FromRoute] Guid teamId, [FromRoute] Guid playerId)
        //{
        //    await _mediator.Send(new RemovePlayerFromTeam(teamId, playerId));
        //    return NoContent();
        //}
    }
}
