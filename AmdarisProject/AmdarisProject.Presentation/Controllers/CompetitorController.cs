using AmdarisProject.Application.Common.Models;
using AmdarisProject.Application.Dtos.DisplayDTOs.CompetitorDisplayDTOs;
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

        [HttpPost(nameof(Player))]
        [ValidateModelState]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> CreatePlayer([FromBody] CompetitorCreateDTO create)
        {
            await _mediator.Send(new CreatePlayer(create));
            return Created();
        }

        [HttpPost(nameof(GetPaginatedPlayers))]
        [ProducesResponseType(typeof(PaginatedResult<PlayerDisplayDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetPaginatedPlayers([FromBody] PagedRequest pagedRequest)
        {
            PaginatedResult<PlayerDisplayDTO> response = await _mediator.Send(new GetPaginatedPlayers(pagedRequest));
            return Ok(response);
        }

        [HttpGet(nameof(GetPlayersNotInTeam) + "/{teamId}")]
        [ValidateGuid]
        [ProducesResponseType(typeof(IEnumerable<PlayerDisplayDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetPlayersNotInTeam([FromRoute] Guid teamId)
        {
            IEnumerable<PlayerDisplayDTO> response = await _mediator.Send(new GetPlayersNotInTeam(teamId));
            return Ok(response);
        }

        [HttpGet(nameof(GetPlayersNotInCompetition) + "/{competitionId}")]
        [ValidateGuid]
        [ProducesResponseType(typeof(IEnumerable<PlayerDisplayDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetPlayersNotInCompetition([FromRoute] Guid competitionId)
        {
            IEnumerable<PlayerDisplayDTO> response = await _mediator.Send(new GetPlayersNotInCompetition(competitionId));
            return Ok(response);
        }

        [HttpPost(nameof(Team))]
        [ValidateModelState]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> CreateTeam([FromBody] CompetitorCreateDTO create)
        {
            await _mediator.Send(new CreateTeam(create));
            return Created();
        }

        [HttpPost(nameof(GetPaginatedTeams))]
        [ProducesResponseType(typeof(PaginatedResult<TeamDisplayDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetPaginatedTeams([FromBody] PagedRequest pagedRequest)
        {
            PaginatedResult<TeamDisplayDTO> response = await _mediator.Send(new GetPaginatedTeams(pagedRequest));
            return Ok(response);
        }

        [HttpGet(nameof(GetTeamsThatCanBeAddedToCompetition) + "/{competitionId}")]
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
