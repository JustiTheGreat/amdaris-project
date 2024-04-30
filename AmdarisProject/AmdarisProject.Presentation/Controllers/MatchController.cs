using AmdarisProject.Application.Dtos.ResponseDTOs;
using AmdarisProject.Application.Handlers.MatchHandlers;
using AmdarisProject.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AmdarisProject.Presentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MatchController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet("{matchId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetMatchById([FromRoute] Guid matchId)
        {
            try
            {
                MatchGetDTO response = await _mediator.Send(new GetMatchById(matchId));
                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpPut("start/{matchId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> StartMatch([FromRoute] Guid matchId)
        {
            try
            {
                MatchGetDTO response = await _mediator.Send(new StartMatch(matchId));
                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpPut("end/{matchId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> EndMatch([FromRoute] Guid matchId, [FromBody] MatchStatus matchStatus)
        {
            try
            {
                MatchGetDTO response = await _mediator.Send(new EndMatch(matchId, matchStatus));
                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpPut("cancel/{matchId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> CancelMatch([FromRoute] Guid matchId)
        {
            try
            {
                MatchGetDTO response = await _mediator.Send(new CancelMatch(matchId));
                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
    }
}
