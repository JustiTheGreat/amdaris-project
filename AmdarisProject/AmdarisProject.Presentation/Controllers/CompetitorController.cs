using AmdarisProject.Application.Common.Models;
using AmdarisProject.Application.Dtos.RequestDTOs.CreateDTOs;
using AmdarisProject.Application.Dtos.ResponseDTOs.CompetitorResponseDTOs;
using AmdarisProject.Application.Dtos.ResponseDTOs.DisplayDTOs;
using AmdarisProject.Application.Dtos.ResponseDTOs.GetDTOs;
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
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class CompetitorController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [Authorize(Roles = $"{nameof(UserRole.Administrator)}, {nameof(UserRole.User)}")]
        [HttpGet("{competitorId}")]
        [ValidateGuid]
        [ProducesResponseType(typeof(CompetitorGetDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetCompetitorById([FromRoute] Guid competitorId)
        {
            CompetitorGetDTO response = await _mediator.Send(new GetCompetitorById(competitorId));
            return Ok(response);
        }

        [Authorize(Roles = $"{nameof(UserRole.Administrator)}, {nameof(UserRole.User)}")]
        [HttpPost(nameof(Player))]
        [ValidateModelState]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult> CreatePlayer([FromBody] CompetitorCreateDTO create)
        {
            await _mediator.Send(new CreatePlayer(create));
            return Created();
        }

        [Authorize(Roles = $"{nameof(UserRole.Administrator)}, {nameof(UserRole.User)}")]
        [HttpPost(nameof(GetPaginatedPlayers))]
        [ValidateModelState]
        [ProducesResponseType(typeof(PaginatedResult<CompetitorDisplayDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult> GetPaginatedPlayers([FromBody] PagedRequest pagedRequest)
        {
            PaginatedResult<CompetitorDisplayDTO> response = await _mediator.Send(new GetPaginatedPlayers(pagedRequest));
            return Ok(response);
        }

        [Authorize(Roles = nameof(UserRole.Administrator))]
        [HttpGet(nameof(GetPlayersNotInTeam) + "/{teamId}")]
        [ValidateGuid]
        [ProducesResponseType(typeof(IEnumerable<CompetitorDisplayDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetPlayersNotInTeam([FromRoute] Guid teamId)
        {
            IEnumerable<CompetitorDisplayDTO> response = await _mediator.Send(new GetPlayersNotInTeam(teamId));
            return Ok(response);
        }

        [Authorize(Roles = nameof(UserRole.Administrator))]
        [HttpGet(nameof(GetPlayersNotInCompetition) + "/{competitionId}")]
        [ValidateGuid]
        [ProducesResponseType(typeof(IEnumerable<CompetitorDisplayDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetPlayersNotInCompetition([FromRoute] Guid competitionId)
        {
            IEnumerable<CompetitorDisplayDTO> response = await _mediator.Send(new GetPlayersNotInCompetition(competitionId));
            return Ok(response);
        }

        [Authorize(Roles = nameof(UserRole.Administrator))]
        [HttpPost(nameof(Team))]
        [ValidateModelState]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult> CreateTeam([FromBody] CompetitorCreateDTO create)
        {
            await _mediator.Send(new CreateTeam(create));
            return Created();
        }

        [Authorize(Roles = $"{nameof(UserRole.Administrator)}, {nameof(UserRole.User)}")]
        [HttpPost(nameof(GetPaginatedTeams))]
        [ValidateModelState]
        [ProducesResponseType(typeof(PaginatedResult<CompetitorDisplayDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult> GetPaginatedTeams([FromBody] PagedRequest pagedRequest)
        {
            PaginatedResult<CompetitorDisplayDTO> response = await _mediator.Send(new GetPaginatedTeams(pagedRequest));
            return Ok(response);
        }

        [Authorize(Roles = nameof(UserRole.Administrator))]
        [HttpGet(nameof(GetTeamsThatCanBeAddedToCompetition) + "/{competitionId}")]
        [ValidateGuid]
        [ProducesResponseType(typeof(IEnumerable<CompetitorDisplayDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetTeamsThatCanBeAddedToCompetition([FromRoute] Guid competitionId)
        {
            IEnumerable<CompetitorDisplayDTO> response = await _mediator.Send(new GetTeamsThatCanBeAddedToCompetition(competitionId));
            return Ok(response);
        }

        [Authorize(Roles = $"{nameof(UserRole.Administrator)}, {nameof(UserRole.User)}")]
        [HttpGet(nameof(GetCompetitorWinRatings) + "/{competitorId}")]
        [ValidateGuid]
        [ProducesResponseType(typeof(Dictionary<GameTypeGetDTO, double>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetCompetitorWinRatings([FromRoute] Guid competitorId)
        {
            Dictionary<string, double> response = await _mediator.Send(new GetCompetitorWinRatings(competitorId));
            return Ok(response);
        }
    }
}
