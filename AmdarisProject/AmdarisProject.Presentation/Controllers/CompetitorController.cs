﻿using AmdarisProject.Application.Dtos.DisplayDTOs.CompetitorDisplayDTOs;
using AmdarisProject.Application.Dtos.RequestDTOs.CreateDTOs;
using AmdarisProject.Application.Dtos.ResponseDTOs.CompetitorResponseDTOs;
using AmdarisProject.Application.Handlers.CompetitorHandlers;
using AmdarisProject.Domain.Models.CompetitorModels;
using AmdarisProject.Presentation.Filters;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AmdarisProject.Presentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class CompetitorController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet("{competitorId}")]
        [ValidateGuid]
        [ProducesResponseType(typeof(CompetitorGetDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetCompetitorById([FromRoute] Guid competitorId)
        {
            CompetitorGetDTO response = await _mediator.Send(new GetCompetitorById(competitorId));
            return Ok(response);
        }

        [HttpPut(nameof(Player))]
        [ValidateModelState]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> CreatePlayer([FromBody] CompetitorCreateDTO create)
        {
            await _mediator.Send(new CreatePlayer(create));
            return Created();
        }

        [HttpGet(nameof(Player))]
        [ProducesResponseType(typeof(IEnumerable<PlayerDisplayDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetAllPlayers()
        {
            IEnumerable<PlayerDisplayDTO> response = await _mediator.Send(new GetAllPlayers());
            return Ok(response);
        }

        [HttpGet(nameof(Player) + "/not_in_team/{teamId}")]
        [ValidateGuid]
        [ProducesResponseType(typeof(IEnumerable<PlayerDisplayDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetPlayersNotInTeam([FromRoute] Guid teamId)
        {
            IEnumerable<PlayerDisplayDTO> response = await _mediator.Send(new GetPlayersNotInTeam(teamId));
            return Ok(response);
        }

        [HttpGet(nameof(Player) + "/not_in_competition/{competitionId}")]
        [ValidateGuid]
        [ProducesResponseType(typeof(IEnumerable<PlayerDisplayDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetPlayersNotInCompetition([FromRoute] Guid competitionId)
        {
            IEnumerable<PlayerDisplayDTO> response = await _mediator.Send(new GetPlayersNotInCompetition(competitionId));
            return Ok(response);
        }

        [HttpPut(nameof(Team))]
        [ValidateModelState]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> CreateTeam([FromBody] CompetitorCreateDTO create)
        {
            await _mediator.Send(new CreateTeam(create));
            return Created();
        }

        [HttpGet(nameof(Team))]
        [ProducesResponseType(typeof(IEnumerable<TeamDisplayDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetAllTeams()
        {
            IEnumerable<TeamDisplayDTO> response = await _mediator.Send(new GetPagedTeams());
            return Ok(response);
        }

        [HttpGet(nameof(Team) + "/can_add_to_competition/{competitionId}")]
        [ValidateGuid]
        [ProducesResponseType(typeof(IEnumerable<TeamDisplayDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetTeamsThatCanBeAddedToCompetition([FromRoute] Guid competitionId)
        {
            IEnumerable<TeamDisplayDTO> response = await _mediator.Send(new GetTeamsThatCanBeAddedToCompetition(competitionId));
            return Ok(response);
        }
    }
}
