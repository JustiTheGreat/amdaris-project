using AmdarisProject.Application.Dtos;
using AmdarisProject.Application.Dtos.CreateDTOs.CompetitionCreateDTOs;
using AmdarisProject.Application.Dtos.DisplayDTOs.CompetitorDisplayDTOs;
using AmdarisProject.Application.Dtos.ResponseDTOs.CompetitionResponseDTOs;
using AmdarisProject.Application.Handlers.CompetitionHandlers;
using AmdarisProject.handlers.competition;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AmdarisProject.Presentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CompetitionController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> CreateCompetition([FromBody] CompetitionCreateDTO create)
        {
            try
            {
                CompetitionResponseDTO response = await _mediator.Send(new CreateCompetition(create));
                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpGet("{competitionId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetCompetitionById([FromRoute] Guid competitionId)
        {
            try
            {
                CompetitionResponseDTO response = await _mediator.Send(new GetCompetitionById(competitionId));
                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpGet("ranking/{competitionId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetCompetitionRanking([FromRoute] Guid competitionId)
        {
            try
            {
                IEnumerable<RankingItemDTO> response = await _mediator.Send(new GetCompetitionRanking(competitionId));
                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpGet("winners/{competitionId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetCompetitionWinners([FromRoute] Guid competitionId)
        {
            try
            {
                IEnumerable<CompetitorDisplayDTO> response = await _mediator.Send(new GetCompetitionWinners(competitionId));
                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpPut("stop_registration/{competitionId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> StopCompetitionRegistration([FromRoute] Guid competitionId)
        {
            try
            {
                CompetitionResponseDTO response = await _mediator.Send(new StopCompetitionRegistration(competitionId));
                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpPut("start/{competitionId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> StartCompetition([FromRoute] Guid competitionId)
        {
            try
            {
                CompetitionResponseDTO response = await _mediator.Send(new StartCompetition(competitionId));
                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpPut("end/{competitionId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> EndCompetition([FromRoute] Guid competitionId)
        {
            try
            {
                CompetitionResponseDTO response = await _mediator.Send(new EndCompetition(competitionId));
                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }


        [HttpPut("cancel/{competitionId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> CancelCompetition([FromRoute] Guid competitionId)
        {
            try
            {
                CompetitionResponseDTO response = await _mediator.Send(new CancelCompetition(competitionId));
                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpPut("add/{competitionId}/{competitorId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> CancelCompetition([FromRoute] Guid competitionId, [FromRoute] Guid competitorId)
        {
            try
            {
                CompetitionResponseDTO response = await _mediator.Send(new AddCompetitorToCompetition(competitorId, competitionId));
                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpPut("remove/{competitionId}/{competitorId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> RemoveCompetitorFromCompetition([FromRoute] Guid competitionId, [FromRoute] Guid competitorId)
        {
            try
            {
                CompetitionResponseDTO response = await _mediator.Send(new RemoveCompetitorFromCompetition(competitorId, competitionId));
                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
    }
}
