using AmdarisProject.Application.Dtos.ResponseDTOs.GetDTOs;
using AmdarisProject.Application.Handlers.TeamPlayerHandlers;
using AmdarisProject.Presentation.Filters;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AmdarisProject.Presentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Roles = nameof(UserRole.Administrator))]
    public class TeamPlayerController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost(nameof(AddPlayerToTeam) + "{playerId}/{teamId}")]
        [ValidateGuid]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> AddPlayerToTeam([FromRoute] Guid teamId, [FromRoute] Guid playerId)
        {
            await _mediator.Send(new AddPlayerToTeam(teamId, playerId));
            return Created();
        }

        [HttpPut(nameof(ChangeTeamPlayerStatus) + "{playerId}/{teamId}")]
        [ValidateGuid]
        [ProducesResponseType(typeof(TeamPlayerGetDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> ChangeTeamPlayerStatus([FromRoute] Guid teamId, [FromRoute] Guid playerId, [FromBody] bool active)
        {
            TeamPlayerGetDTO response = await _mediator.Send(new ChangeTeamPlayerStatus(teamId, playerId, active));
            return Ok(response);
        }

        [HttpDelete(nameof(RemovePlayerFromTeam) + "{playerId}/{teamId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> RemovePlayerFromTeam([FromRoute] Guid teamId, [FromRoute] Guid playerId)
        {
            await _mediator.Send(new RemovePlayerFromTeam(teamId, playerId));
            return NoContent();
        }
    }
}
