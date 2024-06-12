using AmdarisProject.Application.Dtos.ResponseDTOs;
using AmdarisProject.Domain.Models;
using AmdarisProject.Domain.Models.CompetitorModels;
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
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class PointController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPut($"{nameof(Match)}" + "/{matchId}/" + $"{nameof(Player)}" + "/{playerId}")]
        [ValidateGuid]
        [ProducesResponseType(typeof(PointGetDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> AddValueToPointValue([FromRoute] Guid matchId, [FromRoute] Guid playerId, [FromBody] uint points)
        {
            PointGetDTO response = await _mediator.Send(new AddValueToPointValue(matchId, playerId, points));
            return Ok(response);
        }
    }
}
