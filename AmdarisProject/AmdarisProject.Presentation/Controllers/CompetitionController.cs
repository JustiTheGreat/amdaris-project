using AmdarisProject.Application.Dtos.CreateDTOs;
using AmdarisProject.Application.Dtos.DisplayDTOs;
using AmdarisProject.Application.Dtos.DisplayDTOs.CompetitorDisplayDTOs;
using AmdarisProject.Application.Dtos.ResponseDTOs;
using AmdarisProject.Application.Dtos.ResponseDTOs.CompetitionResponseDTOs;
using AmdarisProject.Application.Handlers.CompetitionHandlers;
using AmdarisProject.Domain.Models.CompetitionModels;
using AmdarisProject.handlers.competition;
using AmdarisProject.Presentation.Filters;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AmdarisProject.Presentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CompetitionController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost(nameof(OneVSAllCompetition))]
        [ValidateModelState]
        [ProducesResponseType(typeof(OneVSAllCompetitionGetDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> CreateOneVSAllCompetition([FromBody] CompetitionCreateDTO create)
        {
            OneVSAllCompetitionGetDTO response = await _mediator.Send(new CreateOneVSAllCompetition(create));
            return Ok(response);
        }

        [HttpPost(nameof(TournamentCompetition))]
        [ValidateModelState]
        [ProducesResponseType(typeof(TournamentCompetitionGetDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> CreateTournamentCompetition([FromBody] CompetitionCreateDTO create)
        {
            TournamentCompetitionGetDTO response = await _mediator.Send(new CreateTournamentCompetition(create));
            return Ok(response);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CompetitionDisplayDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetAllCompetitions()
        {
            IEnumerable<CompetitionDisplayDTO> response = await _mediator.Send(new GetAllCompetitions());
            return Ok(response);
        }

        [HttpGet("{competitionId}")]
        [ValidateGuid]
        [ProducesResponseType(typeof(CompetitionGetDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetCompetitionById([FromRoute] Guid competitionId)
        {
            CompetitionGetDTO response = await _mediator.Send(new GetCompetitionById(competitionId));
            return Ok(response);
        }

        [HttpGet("ranking/{competitionId}")]
        [ValidateGuid]
        [ProducesResponseType(typeof(IEnumerable<RankingItemDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetCompetitionRanking([FromRoute] Guid competitionId)
        {
            IEnumerable<RankingItemDTO> response = await _mediator.Send(new GetCompetitionRanking(competitionId));
            return Ok(response);
        }

        [HttpGet("winners/{competitionId}")]
        [ValidateGuid]
        [ProducesResponseType(typeof(IEnumerable<CompetitorDisplayDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetCompetitionWinners([FromRoute] Guid competitionId)
        {
            IEnumerable<CompetitorDisplayDTO> response = await _mediator.Send(new GetCompetitionWinners(competitionId));
            return Ok(response);
        }

        [HttpPut("stop_registration/{competitionId}")]
        [ValidateGuid]
        [ProducesResponseType(typeof(CompetitionGetDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> StopCompetitionRegistration([FromRoute] Guid competitionId)
        {
            CompetitionGetDTO response = await _mediator.Send(new StopCompetitionRegistration(competitionId));
            return Ok(response);
        }

        [HttpPut("start/{competitionId}")]
        [ValidateGuid]
        [ProducesResponseType(typeof(CompetitionGetDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> StartCompetition([FromRoute] Guid competitionId)
        {
            CompetitionGetDTO response = await _mediator.Send(new StartCompetition(competitionId));
            return Ok(response);
        }

        [HttpPut("end/{competitionId}")]
        [ValidateGuid]
        [ProducesResponseType(typeof(CompetitionGetDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> EndCompetition([FromRoute] Guid competitionId)
        {
            CompetitionGetDTO response = await _mediator.Send(new EndCompetition(competitionId));
            return Ok(response);
        }


        [HttpPut("cancel/{competitionId}")]
        [ValidateGuid]
        [ProducesResponseType(typeof(CompetitionGetDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> CancelCompetition([FromRoute] Guid competitionId)
        {
            CompetitionGetDTO response = await _mediator.Send(new CancelCompetition(competitionId));
            return Ok(response);
        }

        [HttpPut("add/{competitionId}/{competitorId}")]
        [ValidateGuid]
        [ProducesResponseType(typeof(CompetitionGetDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> AddCompetitorToCompetition([FromRoute] Guid competitionId, [FromRoute] Guid competitorId)
        {
            CompetitionGetDTO response = await _mediator.Send(new AddCompetitorToCompetition(competitorId, competitionId));
            return Ok(response);
        }

        [HttpPut("remove/{competitionId}/{competitorId}")]
        [ValidateGuid]
        [ProducesResponseType(typeof(CompetitionGetDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> RemoveCompetitorFromCompetition([FromRoute] Guid competitionId, [FromRoute] Guid competitorId)
        {
            CompetitionGetDTO response = await _mediator.Send(new RemoveCompetitorFromCompetition(competitorId, competitionId));
            return Ok(response);
        }
    }
}
