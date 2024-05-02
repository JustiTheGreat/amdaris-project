using AmdarisProject.Application.Dtos.CreateDTOs.CompetitorCreateDTOs;
using AmdarisProject.Application.Dtos.DisplayDTOs.CompetitorDisplayDTOs;
using AmdarisProject.Application.Dtos.ResponseDTOs.CompetitorResponseDTOs;
using AmdarisProject.Application.Handlers.CompetitorHandlers;
using AmdarisProject.Presentation.Filters;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AmdarisProject.Presentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CompetitorController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPut]
        [ValidateModelState]
        [ProducesResponseType(typeof(CompetitorGetDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> CreateCompetitor([FromBody] CompetitorCreateDTO create)
        {
            CompetitorGetDTO response = await _mediator.Send(new CreateCompetitor(create));
            return Ok(response);
        }

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

        [HttpGet("player")]
        [ProducesResponseType(typeof(IEnumerable<PlayerDisplayDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetAllPlayers()
        {
            IEnumerable<PlayerDisplayDTO> response = await _mediator.Send(new GetAllPlayers());
            return Ok(response);
        }

        [HttpGet("player/not_in_team/{teamId}")]
        [ValidateGuid]
        [ProducesResponseType(typeof(IEnumerable<PlayerDisplayDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetPlayersNotInTeam([FromRoute] Guid teamId)
        {
            IEnumerable<PlayerDisplayDTO> response = await _mediator.Send(new GetPlayersNotInTeam(teamId));
            return Ok(response);
        }

        [HttpGet("player/not_in_competition/{competitionId}")]
        [ValidateGuid]
        [ProducesResponseType(typeof(IEnumerable<PlayerDisplayDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetPlayersNotInCompetition([FromRoute] Guid competitionId)
        {
            IEnumerable<PlayerDisplayDTO> response = await _mediator.Send(new GetPlayersNotInCompetition(competitionId));
            return Ok(response);
        }

        [HttpGet("team")]
        [ProducesResponseType(typeof(IEnumerable<TeamDisplayDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetAllTeams()
        {
            IEnumerable<TeamDisplayDTO> response = await _mediator.Send(new GetAllTeams());
            return Ok(response);
        }

        [HttpGet("team/can_add_to_competition/{competitionId}")]
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
