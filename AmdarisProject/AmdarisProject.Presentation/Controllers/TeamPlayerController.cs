using AmdarisProject.Application.Dtos.ResponseDTOs.GetDTOs;
using AmdarisProject.Application.Handlers.TeamPlayerHandlers;
using AmdarisProject.Presentation.Filters;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AmdarisProject.Presentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TeamPlayerController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost("{playerId}/{teamId}")]
        [ValidateGuid]
        [ProducesResponseType(typeof(TeamPlayerGetDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> AddPlayerToTeam([FromRoute] Guid playerId, [FromRoute] Guid teamId)
        {
            TeamPlayerGetDTO response = await _mediator.Send(new AddPlayerToTeam(playerId, teamId));
            return Ok(response);
        }

        [HttpPut("{playerId}/{teamId}")]
        [ValidateGuid]
        [ProducesResponseType(typeof(TeamPlayerGetDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> ChangeTeamPlayerStatus([FromRoute] Guid playerId, [FromRoute] Guid teamId, [FromBody] bool active)
        {
            TeamPlayerGetDTO response = await _mediator.Send(new ChangeTeamPlayerStatus(playerId, teamId, active));
            return Ok(response);
        }

        [HttpDelete("{playerId}/{teamId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> RemovePlayerFromTeam([FromRoute] Guid playerId, [FromRoute] Guid teamId)
        {
            await _mediator.Send(new RemovePlayerFromTeam(playerId, teamId));
            return Ok();
        }
    }
}
