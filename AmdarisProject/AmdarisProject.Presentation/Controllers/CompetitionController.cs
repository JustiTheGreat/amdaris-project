using AmdarisProject.Application.Common.Models;
using AmdarisProject.Application.Dtos.DisplayDTOs;
using AmdarisProject.Application.Dtos.DisplayDTOs.CompetitorDisplayDTOs;
using AmdarisProject.Application.Dtos.RequestDTOs.CreateDTOs;
using AmdarisProject.Application.Dtos.ResponseDTOs;
using AmdarisProject.Application.Dtos.ResponseDTOs.CompetitionResponseDTOs;
using AmdarisProject.Application.Handlers.CompetitionHandlers;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models.CompetitionModels;
using AmdarisProject.Domain.Models.CompetitorModels;
using AmdarisProject.handlers.competition;
using AmdarisProject.Infrastructure.Identity;
using AmdarisProject.Presentation.Filters;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AmdarisProject.Presentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class CompetitionController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [Authorize(Roles = nameof(UserRole.Administrator))]
        [HttpPost(nameof(OneVSAllCompetition))]
        [ValidateModelState]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CreateOneVSAllCompetition([FromBody] CompetitionCreateDTO create)
        {
            await _mediator.Send(new CreateOneVSAllCompetition(create));
            return Created();
        }

        [Authorize(Roles = nameof(UserRole.Administrator))]
        [HttpPost(nameof(TournamentCompetition))]
        [ValidateModelState]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CreateTournamentCompetition([FromBody] CompetitionCreateDTO create)
        {
            await _mediator.Send(new CreateTournamentCompetition(create));
            return Created();
        }

        [Authorize(Roles = $"{nameof(UserRole.Administrator)}, {nameof(UserRole.User)}")]
        [HttpPost(nameof(GetPaginatedCompetitions))]
        [ValidateModelState]
        [ProducesResponseType(typeof(PaginatedResult<CompetitionDisplayDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult> GetPaginatedCompetitions([FromBody] PagedRequest pagedRequest)
        {
            PaginatedResult<CompetitionDisplayDTO> response = await _mediator.Send(new GetPaginatedCompetitions(pagedRequest));
            return Ok(response);
        }

        [Authorize(Roles = $"{nameof(UserRole.Administrator)}, {nameof(UserRole.User)}")]
        [HttpGet("{competitionId}")]
        [ValidateGuid]
        [ProducesResponseType(typeof(CompetitionGetDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetCompetitionById([FromRoute] Guid competitionId)
        {
            CompetitionGetDTO response = await _mediator.Send(new GetCompetitionById(competitionId));
            return Ok(response);
        }

        [Authorize(Roles = $"{nameof(UserRole.Administrator)}, {nameof(UserRole.User)}")]
        [HttpGet(nameof(GetCompetitionRanking) + "/{competitionId}")]
        [ValidateGuid]
        [ProducesResponseType(typeof(IEnumerable<RankingItemDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetCompetitionRanking([FromRoute] Guid competitionId)
        {
            IEnumerable<RankingItemDTO> response = await _mediator.Send(new GetCompetitionRanking(competitionId));
            return Ok(response);
        }

        [Authorize(Roles = $"{nameof(UserRole.Administrator)}, {nameof(UserRole.User)}")]
        [HttpGet(nameof(GetCompetitionWinners) + "/{competitionId}")]
        [ValidateGuid]
        [ProducesResponseType(typeof(IEnumerable<CompetitorDisplayDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetCompetitionWinners([FromRoute] Guid competitionId)
        {
            IEnumerable<CompetitorDisplayDTO> response = await _mediator.Send(new GetCompetitionWinners(competitionId));
            return Ok(response);
        }

        [Authorize(Roles = nameof(UserRole.Administrator))]
        [HttpPut(nameof(StopCompetitionRegistration) + "stop_registration/{competitionId}")]
        [ValidateGuid]
        [ProducesResponseType(typeof(CompetitionGetDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> StopCompetitionRegistration([FromRoute] Guid competitionId)
        {
            CompetitionGetDTO response = await _mediator.Send(new StopCompetitionRegistration(competitionId));
            return Ok(response);
        }

        [Authorize(Roles = nameof(UserRole.Administrator))]
        [HttpPut(nameof(StartCompetition) + "/{competitionId}")]
        [ValidateGuid]
        [ProducesResponseType(typeof(CompetitionGetDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> StartCompetition([FromRoute] Guid competitionId)
        {
            CompetitionGetDTO response = await _mediator.Send(new StartCompetition(competitionId));
            return Ok(response);
        }

        [Authorize(Roles = nameof(UserRole.Administrator))]
        [HttpPut(nameof(EndCompetition) + "/{competitionId}")]
        [ValidateGuid]
        [ProducesResponseType(typeof(CompetitionGetDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> EndCompetition([FromRoute] Guid competitionId)
        {
            CompetitionGetDTO response = await _mediator.Send(new EndCompetition(competitionId));
            return Ok(response);
        }

        [Authorize(Roles = nameof(UserRole.Administrator))]
        [HttpPut(nameof(CancelCompetition) + "/{competitionId}")]
        [ValidateGuid]
        [ProducesResponseType(typeof(CompetitionGetDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> CancelCompetition([FromRoute] Guid competitionId)
        {
            CompetitionGetDTO response = await _mediator.Send(new CancelCompetition(competitionId));
            return Ok(response);
        }

        [Authorize(Roles = nameof(UserRole.User))]
        [HttpPut(nameof(AddCompetitorToCompetition) + $"/{nameof(Competition)}" + "/{competitionId}")]
        [ValidateGuid]
        [ProducesResponseType(typeof(CompetitionGetDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> AddCompetitorToCompetition([FromRoute] Guid competitionId)
        {
            Guid userPlayerId = Guid.Parse(User.FindFirstValue(ClaimIndetifiers.PlayerId)
                    ?? throw new APException(nameof(User.Claims)));

            CompetitionGetDTO response = await _mediator.Send(new AddCompetitorToCompetition(competitionId, userPlayerId));
            return Ok(response);
        }

        [Authorize(Roles = nameof(UserRole.Administrator))]
        [HttpPut(nameof(AddCompetitorToCompetition) + $"/{nameof(Competition)}" + "/{competitionId}" + $"{nameof(Competitor)}" + "/{competitorId}")]
        [ValidateGuid]
        [ProducesResponseType(typeof(CompetitionGetDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> AddCompetitorToCompetition([FromRoute] Guid competitionId, [FromRoute] Guid competitorId)
        {
            CompetitionGetDTO response = await _mediator.Send(new AddCompetitorToCompetition(competitorId, competitionId));
            return Ok(response);
        }

        [Authorize(Roles = nameof(UserRole.User))]
        [HttpPut(nameof(RemoveCompetitorFromCompetition) + $"/{nameof(Competition)}" + "/{competitionId}")]
        [ValidateGuid]
        [ProducesResponseType(typeof(CompetitionGetDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> RemoveCompetitorFromCompetition([FromRoute] Guid competitionId)
        {
            Guid userPlayerId = Guid.Parse(User.FindFirstValue(ClaimIndetifiers.PlayerId)
                    ?? throw new APException(nameof(User.Claims)));

            CompetitionGetDTO response = await _mediator.Send(new RemoveCompetitorFromCompetition(competitionId, userPlayerId));
            return Ok(response);
        }

        [Authorize(Roles = nameof(UserRole.Administrator))]
        [HttpPut(nameof(RemoveCompetitorFromCompetition) + $"/{nameof(Competition)}" + "/{competitionId}" + $"{nameof(Competitor)}" + "/{competitorId}")]
        [ValidateGuid]
        [ProducesResponseType(typeof(CompetitionGetDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> RemoveCompetitorFromCompetition([FromRoute] Guid competitionId, [FromRoute] Guid competitorId)
        {
            CompetitionGetDTO response = await _mediator.Send(new RemoveCompetitorFromCompetition(competitorId, competitionId));
            return Ok(response);
        }
    }
}
