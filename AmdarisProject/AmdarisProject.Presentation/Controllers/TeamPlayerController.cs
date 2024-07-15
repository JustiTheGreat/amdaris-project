using AmdarisProject.Application.Dtos.ResponseDTOs.DisplayDTOs;
using AmdarisProject.Application.Handlers.TeamPlayerHandlers;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models.CompetitorModels;
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
    public class TeamPlayerController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [Authorize(Roles = nameof(UserRole.User))]
        [HttpPost(nameof(AddPlayerToTeam) + $"/{nameof(Team)}" + "/{teamId}")]
        [ValidateGuid]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> AddPlayerToTeam([FromRoute] Guid teamId)
        {
            Guid userPlayerId = Guid.Parse(User.FindFirstValue(ClaimIndetifiers.PlayerId)
                    ?? throw new APException(nameof(User.Claims)));

            await _mediator.Send(new AddPlayerToTeam(teamId, userPlayerId));
            return Created();
        }

        [Authorize(Roles = nameof(UserRole.Administrator))]
        [HttpPost(nameof(AddPlayerToTeam) + $"/{nameof(Team)}" + "/{teamId}" + $"/{nameof(Player)}" + "/{playerId}")]
        [ValidateGuid]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> AddPlayerToTeam([FromRoute] Guid teamId, [FromRoute] Guid playerId)
        {
            await _mediator.Send(new AddPlayerToTeam(teamId, playerId));
            return Created();
        }

        [Authorize(Roles = nameof(UserRole.User))]
        [HttpPut(nameof(ChangeTeamPlayerStatus) + $"/{nameof(Team)}" + "/{teamId}")]
        [ValidateGuid]
        [ProducesResponseType(typeof(TeamPlayerDisplayDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> ChangeTeamPlayerStatus([FromRoute] Guid teamId)
        {
            Guid userPlayerId = Guid.Parse(User.FindFirstValue(ClaimIndetifiers.PlayerId)
                    ?? throw new APException(nameof(User.Claims)));

            TeamPlayerDisplayDTO response = await _mediator.Send(new ChangeTeamPlayerStatus(teamId, userPlayerId));
            return Ok(response);
        }

        [Authorize(Roles = nameof(UserRole.Administrator))]
        [HttpPut(nameof(ChangeTeamPlayerStatus) + $"/{nameof(Team)}" + "/{teamId}" + $"/{nameof(Player)}" + "/{playerId}")]
        [ValidateGuid]
        [ProducesResponseType(typeof(TeamPlayerDisplayDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> ChangeTeamPlayerStatus([FromRoute] Guid teamId, [FromRoute] Guid playerId)
        {
            TeamPlayerDisplayDTO response = await _mediator.Send(new ChangeTeamPlayerStatus(teamId, playerId));
            return Ok(response);
        }

        [Authorize(Roles = nameof(UserRole.User))]
        [HttpDelete(nameof(RemovePlayerFromTeam) + $"/{nameof(Team)}" + "/{teamId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> RemovePlayerFromTeam([FromRoute] Guid teamId)
        {
            Guid userPlayerId = Guid.Parse(User.FindFirstValue(ClaimIndetifiers.PlayerId)
                    ?? throw new APException(nameof(User.Claims)));

            await _mediator.Send(new RemovePlayerFromTeam(teamId, userPlayerId));
            return NoContent();
        }

        [Authorize(Roles = nameof(UserRole.Administrator))]
        [HttpDelete(nameof(RemovePlayerFromTeam) + $"/{nameof(Team)}" + "/{teamId}" + $"/{nameof(Player)}" + "/{playerId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> RemovePlayerFromTeam([FromRoute] Guid teamId, [FromRoute] Guid playerId)
        {
            await _mediator.Send(new RemovePlayerFromTeam(teamId, playerId));
            return NoContent();
        }
    }
}
