﻿using AmdarisProject.Application.Dtos.ResponseDTOs;
using AmdarisProject.Application.Handlers.MatchHandlers;
using AmdarisProject.Domain.Enums;
using AmdarisProject.Presentation.Filters;
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
        [ValidateGuid]
        [ProducesResponseType(typeof(MatchGetDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetMatchById([FromRoute] Guid matchId)
        {
            MatchGetDTO response = await _mediator.Send(new GetMatchById(matchId));
            return Ok(response);
        }

        [HttpPut("start/{matchId}")]
        [ValidateGuid]
        [ProducesResponseType(typeof(MatchGetDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> StartMatch([FromRoute] Guid matchId)
        {
            MatchGetDTO response = await _mediator.Send(new StartMatch(matchId));
            return Ok(response);
        }

        [HttpPut("end/{matchId}")]
        [ValidateGuid]
        [ProducesResponseType(typeof(MatchGetDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> EndMatch([FromRoute] Guid matchId, [FromBody] MatchStatus matchStatus)
        {
            MatchGetDTO response = await _mediator.Send(new EndMatch(matchId, matchStatus));
            return Ok(response);
        }

        [HttpPut("cancel/{matchId}")]
        [ValidateGuid]
        [ProducesResponseType(typeof(MatchGetDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> CancelMatch([FromRoute] Guid matchId)
        {
            MatchGetDTO response = await _mediator.Send(new CancelMatch(matchId));
            return Ok(response);
        }
    }
}
