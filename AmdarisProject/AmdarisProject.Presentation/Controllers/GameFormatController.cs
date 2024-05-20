using AmdarisProject.Application.Common.Models;
using AmdarisProject.Application.Dtos.RequestDTOs.CreateDTOs;
using AmdarisProject.Application.Dtos.ResponseDTOs;
using AmdarisProject.Application.Handlers.GameFormatHandlers;
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
    public class GameFormatController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost]
        [ValidateModelState]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> CreateGameFormat([FromBody] GameFormatCreateDTO create)
        {
            await _mediator.Send(new CreateGameFormat(create));
            return Created();
        }

        [HttpPost(nameof(GetPaginatedGameFormats))]
        [ValidateModelState]
        [ProducesResponseType(typeof(PaginatedResult<GameFormatGetDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult> GetPaginatedGameFormats([FromBody] PagedRequest pagedRequest)
        {
            PaginatedResult<GameFormatGetDTO> response = await _mediator.Send(new GetPaginatedGameFormats(pagedRequest));
            return Ok(response);
        }
    }
}
