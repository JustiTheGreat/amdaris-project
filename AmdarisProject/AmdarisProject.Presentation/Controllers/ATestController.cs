using AmdarisProject.Application.Dtos.DisplayDTOs;
using AmdarisProject.Application.Dtos.DisplayDTOs.CompetitorDisplayDTOs;
using AmdarisProject.Application.Dtos.RequestDTOs.CreateDTOs;
using AmdarisProject.Application.Dtos.ResponseDTOs;
using AmdarisProject.Application.Dtos.ResponseDTOs.CompetitionResponseDTOs;
using AmdarisProject.Application.Dtos.ResponseDTOs.CompetitorResponseDTOs;
using AmdarisProject.Application.Handlers.CompetitionHandlers;
using AmdarisProject.Application.Handlers.CompetitorHandlers;
using AmdarisProject.Application.Handlers.GameFormatHandlers;
using AmdarisProject.Application.Handlers.MatchHandlers;
using AmdarisProject.Application.Handlers.TeamPlayerHandlers;
using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.handlers.competition;
using AmdarisProject.handlers.point;
using AmdarisProject.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AmdarisProject.Presentation.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("[controller]")]
    public class ATestController(IMediator mediator, AmdarisProjectDBContext amdarisProjectDBContext) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        private readonly AmdarisProjectDBContext _amdarisProjectDBContext = amdarisProjectDBContext;

        [HttpGet]
        [ProducesResponseType(typeof(CompetitionGetDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Test()
        {
            await _amdarisProjectDBContext.Database.EnsureDeletedAsync();
            await _amdarisProjectDBContext.Database.EnsureCreatedAsync();

            uint myTeamSize = 2;

            Guid pingPongPlayer = _mediator.Send(new CreateGameFormat(new GameFormatCreateDTO()
            {
                Name = "PingPongPlayerWinAt3",
                GameType = GameType.PING_PONG,
                CompetitorType = CompetitorType.PLAYER,
                WinAt = 3,
            })).Result.Id;

            Guid pingPongTeam = _mediator.Send(new CreateGameFormat(new GameFormatCreateDTO()
            {
                Name = "PingPongTeamWinAt3",
                GameType = GameType.PING_PONG,
                CompetitorType = CompetitorType.TEAM,
                TeamSize = myTeamSize,
                WinAt = 3,
            })).Result.Id;

            Guid pingPongTeamTimed = _mediator.Send(new CreateGameFormat(new GameFormatCreateDTO()
            {
                Name = "PingPongTeamWinAt3Duration5",
                GameType = GameType.PING_PONG,
                CompetitorType = CompetitorType.TEAM,
                TeamSize = myTeamSize,
                WinAt = 3,
                DurationInMinutes = 5,
            })).Result.Id;

            Guid player1Id = _mediator.Send(new CreatePlayer(new CompetitorCreateDTO() { Name = "Player1" })).Result.Id;
            Guid player2Id = _mediator.Send(new CreatePlayer(new CompetitorCreateDTO() { Name = "Player2" })).Result.Id;
            Guid player3Id = _mediator.Send(new CreatePlayer(new CompetitorCreateDTO() { Name = "Player3" })).Result.Id;
            Guid player4Id = _mediator.Send(new CreatePlayer(new CompetitorCreateDTO() { Name = "Player4" })).Result.Id;
            Guid player5Id = _mediator.Send(new CreatePlayer(new CompetitorCreateDTO() { Name = "Player5" })).Result.Id;
            Guid player6Id = _mediator.Send(new CreatePlayer(new CompetitorCreateDTO() { Name = "Player6" })).Result.Id;
            Guid player7Id = _mediator.Send(new CreatePlayer(new CompetitorCreateDTO() { Name = "Player7" })).Result.Id;
            Guid player8Id = _mediator.Send(new CreatePlayer(new CompetitorCreateDTO() { Name = "Player8" })).Result.Id;

            Guid team1Id = _mediator.Send(new CreateTeam(new CompetitorCreateDTO() { Name = "Team1" })).Result.Id;
            Guid team2Id = _mediator.Send(new CreateTeam(new CompetitorCreateDTO() { Name = "Team2" })).Result.Id;
            Guid team3Id = _mediator.Send(new CreateTeam(new CompetitorCreateDTO() { Name = "Team3" })).Result.Id;
            Guid team4Id = _mediator.Send(new CreateTeam(new CompetitorCreateDTO() { Name = "Team4" })).Result.Id;

            await _mediator.Send(new AddPlayerToTeam(player1Id, team1Id));
            await _mediator.Send(new AddPlayerToTeam(player2Id, team1Id));
            await _mediator.Send(new AddPlayerToTeam(player3Id, team2Id));
            await _mediator.Send(new AddPlayerToTeam(player4Id, team2Id));
            await _mediator.Send(new AddPlayerToTeam(player5Id, team3Id));
            await _mediator.Send(new AddPlayerToTeam(player6Id, team3Id));
            await _mediator.Send(new AddPlayerToTeam(player7Id, team4Id));
            await _mediator.Send(new AddPlayerToTeam(player8Id, team4Id));

            await _mediator.Send(new ChangeTeamPlayerStatus(team1Id, player1Id, true));
            await _mediator.Send(new ChangeTeamPlayerStatus(team1Id, player2Id, true));
            await _mediator.Send(new ChangeTeamPlayerStatus(team2Id, player3Id, true));
            await _mediator.Send(new ChangeTeamPlayerStatus(team2Id, player4Id, true));
            await _mediator.Send(new ChangeTeamPlayerStatus(team3Id, player5Id, true));
            await _mediator.Send(new ChangeTeamPlayerStatus(team3Id, player6Id, true));
            await _mediator.Send(new ChangeTeamPlayerStatus(team4Id, player7Id, true));
            await _mediator.Send(new ChangeTeamPlayerStatus(team4Id, player8Id, true));

            string location = "Amdaris";

            CompetitionCreateDTO createCompetition(string name, Guid gameFormat, ulong? breakInMinutes = null)
            {
                return new CompetitionCreateDTO()
                {
                    Name = name,
                    Location = location,
                    StartTime = DateTime.UtcNow,
                    GameFormat = gameFormat,
                    BreakInMinutes = breakInMinutes
                };
            }

            Guid competition1Id = _mediator.Send(new CreateOneVSAllCompetition(createCompetition("c1", pingPongPlayer))).Result.Id;
            Guid competition2Id = _mediator.Send(new CreateOneVSAllCompetition(createCompetition("c2", pingPongTeam))).Result.Id;
            Guid competition3Id = _mediator.Send(new CreateTournamentCompetition(createCompetition("c3", pingPongPlayer))).Result.Id;
            Guid competition4Id = _mediator.Send(new CreateTournamentCompetition(createCompetition("c4", pingPongTeam))).Result.Id;
            Guid competition5Id = _mediator.Send(new CreateTournamentCompetition(createCompetition("c5", pingPongTeamTimed, 5))).Result.Id;

            await _mediator.Send(new AddCompetitorToCompetition(player1Id, competition1Id));
            await _mediator.Send(new AddCompetitorToCompetition(player2Id, competition1Id));
            await _mediator.Send(new AddCompetitorToCompetition(player3Id, competition1Id));

            await _mediator.Send(new AddCompetitorToCompetition(team1Id, competition2Id));
            await _mediator.Send(new AddCompetitorToCompetition(team2Id, competition2Id));
            await _mediator.Send(new AddCompetitorToCompetition(team3Id, competition2Id));

            await _mediator.Send(new AddCompetitorToCompetition(player1Id, competition3Id));
            await _mediator.Send(new AddCompetitorToCompetition(player2Id, competition3Id));
            await _mediator.Send(new AddCompetitorToCompetition(player3Id, competition3Id));
            await _mediator.Send(new AddCompetitorToCompetition(player4Id, competition3Id));

            await _mediator.Send(new AddCompetitorToCompetition(team1Id, competition4Id));
            await _mediator.Send(new AddCompetitorToCompetition(team2Id, competition4Id));
            await _mediator.Send(new AddCompetitorToCompetition(team3Id, competition4Id));
            await _mediator.Send(new AddCompetitorToCompetition(team4Id, competition4Id));

            await _mediator.Send(new AddCompetitorToCompetition(team1Id, competition5Id));
            await _mediator.Send(new AddCompetitorToCompetition(team2Id, competition5Id));
            await _mediator.Send(new AddCompetitorToCompetition(team3Id, competition5Id));
            await _mediator.Send(new AddCompetitorToCompetition(team4Id, competition5Id));

            Guid competitionToTestId = competition4Id;

            //await mediator.Send(new ChangeTeamPlayerStatus(team3Id, player6Id, false));
            //await mediator.Send(new RemovePlayerFromTeam(player6Id, team3Id));

            int i = 0;

            await simulateCompetition(competitionToTestId);

            async Task simulateCompetition(Guid competitionId)
            {
                await _mediator.Send(new StopCompetitionRegistration(competitionId));
                await _mediator.Send(new StartCompetition(competitionId));

                async Task<List<MatchDisplayDTO>> getCompetitionUnfinishedMatches()
                    => (await _mediator.Send(new GetCompetitionById(competitionId))).Matches
                        .Where(match => match.Status is MatchStatus.NOT_STARTED or MatchStatus.STARTED).ToList();

                while ((await getCompetitionUnfinishedMatches()).Count != 0)
                    foreach (MatchDisplayDTO match in await getCompetitionUnfinishedMatches())
                        await SimulateMatch(match.Id, competitionId);

                await _mediator.Send(new EndCompetition(competitionId));

                CompetitionGetDTO competitionResponseDTO = await _mediator.Send(new GetCompetitionById(competitionId));

                Console.WriteLine($"Matches played: {competitionResponseDTO.Matches.Count}");
                competitionResponseDTO.Matches
                    .Select(match => _mediator.Send(new GetMatchById(match.Id)).Result)
                    .ToList()
                    .ForEach(match => Console.WriteLine($"{match.Status}: {match.StartTime}-{match.EndTime}: " +
                    $"{match.CompetitorOne.Name}={match.CompetitorOnePoints}-{match.CompetitorTwo.Name}={match.CompetitorTwoPoints}"));

                IEnumerable<CompetitorDisplayDTO> competitionWinners = await _mediator.Send(new GetCompetitionWinners(competitionId));

                if (!competitionWinners.Any()) Console.WriteLine($"Competition {competitionResponseDTO.Name} can't continue!");
                else competitionWinners.ToList().ForEach(winner =>
                    Console.WriteLine($"The winner of competition {competitionResponseDTO.Name} is competitor {winner.Name}!"));

                Console.WriteLine();

                Console.WriteLine("Competitor's ratings:");
                competitionResponseDTO.Competitors
                    .Select(competitor => new
                    {
                        competitor.Name,
                        Rating = _mediator.Send(new GetCompetitorWinRatingForGameType(competitor.Id, competitionResponseDTO.GameType)).Result
                    })
                    .ToList()
                    .ForEach(o => Console.WriteLine($"{o.Name}: {o.Rating}"));
            }

            async Task SimulateMatch(Guid matchId, Guid competitionId)
            {
                MatchGetDTO match = await _mediator.Send(new StartMatch(matchId));

                if (match.Status is not MatchStatus.STARTED) return;

                CompetitionGetDTO competition = await _mediator.Send(new GetCompetitionById(competitionId));

                //if (++i == 2)
                ////if (++i == competition.Competitors.Count / 2 + 1)
                //{
                //    await mediator.Send(new CancelMatch(matchId));
                //    return;
                //}

                while (_mediator.Send(new GetMatchById(matchId)).Result.Status is not MatchStatus.FINISHED)
                {
                    match = await _mediator.Send(new GetMatchById(matchId));
                    Guid? scorer = null;

                    if (competition.CompetitorType is CompetitorType.PLAYER)
                    {
                        scorer = new Random().Next(2) == 0 ? match.CompetitorOne.Id : match.CompetitorTwo.Id;
                    }
                    else if (competition.CompetitorType is CompetitorType.TEAM)
                    {
                        CompetitorGetDTO competitorOne = await _mediator.Send(new GetCompetitorById(match.CompetitorOne.Id));
                        CompetitorGetDTO competitorTwo = await _mediator.Send(new GetCompetitorById(match.CompetitorTwo.Id));
                        int numberOfPlayers = (int)(2 * competition.TeamSize ?? throw new AmdarisProjectException(""));
                        int random = new Random().Next(numberOfPlayers);
                        scorer = random < numberOfPlayers / 2
                            ? ((TeamGetDTO)competitorOne).Players[random % (numberOfPlayers / 2)].Id
                            : ((TeamGetDTO)competitorTwo).Players[(random - (numberOfPlayers / 2)) % (numberOfPlayers / 2)].Id;
                    }

                    await _mediator.Send(new AddValueToPointValue(scorer ?? throw new AmdarisProjectException(""), match.Id, 1));
                }
            }

            return Ok(await _mediator.Send(new GetCompetitionById(competitionToTestId)));
        }
    }
}
