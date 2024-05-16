using AmdarisProject.Application.Dtos.ResponseDTOs;
using AmdarisProject.handlers.point;
using AmdarisProject.Presentation.Filters;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AmdarisProject.Presentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Roles = nameof(UserRole.Administrator))]
    public class PointController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPut("{playerId}/{matchId}")]
        [ValidateGuid]
        [ProducesResponseType(typeof(PointGetDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> AddValueToPointValue([FromRoute] Guid playerId, [FromRoute] Guid matchId, [FromBody] uint points)
        {
            PointGetDTO response = await _mediator.Send(new AddValueToPointValue(playerId, matchId, points));
            return Ok(response);
        }
    }
}
