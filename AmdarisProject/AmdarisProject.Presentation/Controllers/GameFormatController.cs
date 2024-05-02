using AmdarisProject.Application.Dtos.CreateDTOs;
using AmdarisProject.Application.Dtos.ResponseDTOs;
using AmdarisProject.Application.Handlers.GameFormatHandlers;
using AmdarisProject.Presentation.Filters;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AmdarisProject.Presentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GameFormatController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost]
        [ValidateModelState]
        [ProducesResponseType(typeof(GameFormatGetDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> CreateGameFormat([FromBody] GameFormatCreateDTO create)
        {
            GameFormatGetDTO response = await _mediator.Send(new CreateGameFormat(create));
            return Ok(response);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<GameFormatGetDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetAllGameFormats()
        {
            IEnumerable<GameFormatGetDTO> response = await _mediator.Send(new GetAllGameFormats());
            return Ok(response);
        }
    }
}
