using AmdarisProject.Application.Common.Models;
using AmdarisProject.Application.Dtos.ResponseDTOs.GetDTOs;
using AmdarisProject.Application.Handlers.GameTypesHandlers;
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
    public class GameTypeController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost(nameof(GetPaginatedGameTypes))]
        [ValidateModelState]
        [ProducesResponseType(typeof(PaginatedResult<GameTypeGetDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult> GetPaginatedGameTypes([FromBody] PagedRequest pagedRequest)
        {
            PaginatedResult<GameTypeGetDTO> response = await _mediator.Send(new GetPaginatedGameTypes(pagedRequest));
            return Ok(response);
        }
    }
}
