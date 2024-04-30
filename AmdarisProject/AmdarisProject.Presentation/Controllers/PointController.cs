using AmdarisProject.Application.Dtos.ResponseDTOs;
using AmdarisProject.handlers.point;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AmdarisProject.Presentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PointController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPut("{playerId}/{matchId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> AddValueToPointValue([FromRoute] Guid playerId, [FromRoute] Guid matchId, [FromBody] ushort points)
        {
            try
            {
                PointGetDTO response = await _mediator.Send(new AddValueToPointValue(playerId, matchId, points));
                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
    }
}
