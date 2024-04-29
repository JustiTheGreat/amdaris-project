using AmdarisProject.Application.Handlers.TeamPlayerHandlers;
using AmdarisProject.Domain.Models.CompetitorModels;
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> AddPlayerToTeam([FromRoute] Guid playerId, [FromRoute] Guid teamId)
        {
            try
            {
                TeamPlayer response = await _mediator.Send(new AddPlayerToTeam(playerId, teamId));
                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpPut("{playerId}/{teamId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> ChangeTeamPlayerStatus([FromRoute] Guid playerId, [FromRoute] Guid teamId, [FromBody] bool active)
        {
            try
            {
                TeamPlayer response = await _mediator.Send(new ChangeTeamPlayerStatus(playerId, teamId, active));
                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpDelete("{playerId}/{teamId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> RemovePlayerFromTeam([FromRoute] Guid playerId, [FromRoute] Guid teamId)
        {
            try
            {
                await _mediator.Send(new RemovePlayerFromTeam(playerId, teamId));
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
    }
}
