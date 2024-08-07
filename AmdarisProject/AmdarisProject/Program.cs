﻿using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Dtos.CreateDTOs;
using AmdarisProject.Application.Dtos.CreateDTOs.CompetitionCreateDTOs;
using AmdarisProject.Application.Dtos.CreateDTOs.CompetitorCreateDTOs;
using AmdarisProject.Application.Dtos.DisplayDTOs.CompetitorDisplayDTOs;
using AmdarisProject.Application.Dtos.ResponseDTOs;
using AmdarisProject.Application.Dtos.ResponseDTOs.CompetitionResponseDTOs;
using AmdarisProject.Application.Dtos.ResponseDTOs.CompetitorResponseDTOs;
using AmdarisProject.Application.Handlers.CompetitionHandlers;
using AmdarisProject.Application.Handlers.CompetitorHandlers;
using AmdarisProject.Application.Handlers.GameFormatHandlers;
using AmdarisProject.Application.Handlers.MatchHandlers;
using AmdarisProject.Application.Handlers.TeamPlayerHandlers;
using AmdarisProject.Application.Services;
using AmdarisProject.Application.Services.CompetitionMatchCreatorServices;
using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models;
using AmdarisProject.handlers.competition;
using AmdarisProject.handlers.point;
using AmdarisProject.Infrastructure;
using AmdarisProject.Infrastructure.Repositories;
using AmdarisProject.Presentation;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

IServiceProvider serviceProvider = new ServiceCollection()
    .AddDbContext<AmdarisProjectDBContext>(ob =>
        ob.UseSqlServer("Server=ROMOB41072;Database=AmdarisProject;Trusted_Connection=True;TrustServerCertificate=True;"))
    .AddScoped<ICompetitionRepository, CompetitionRepository>()
    .AddScoped<ICompetitorRepository, CompetitorRepository>()
    .AddScoped<IGameFormatRepository, GameFormatRepository>()
    .AddScoped<IMatchRepository, MatchRepository>()
    .AddScoped<IPointRepository, PointRepository>()
    .AddScoped<ITeamPlayerRepository, TeamPlayerRepository>()
    .AddScoped<IUnitOfWork, UnitOfWork>()
    .AddScoped<IMapper>(sp => new Mapper(MapsterConfiguration.GetMapsterConfiguration()))
    .AddScoped<ICompetitionMatchCreatorFactoryService, CompetitionMatchCreatorFactoryService>()
    .AddScoped<ICompetitionRankingService, CompetionRankingService>()
    .AddScoped<IEndMatchService, EndMatchService>()
    .AddMediatR(configuration => configuration.RegisterServicesFromAssemblies(typeof(CreateCompetitionHandler).Assembly))
    .BuildServiceProvider();

IMediator mediator = serviceProvider.GetRequiredService<IMediator>();

serviceProvider.GetService<AmdarisProjectDBContext>()!.Database.EnsureDeleted();
serviceProvider.GetService<AmdarisProjectDBContext>()!.Database.EnsureCreated();

ushort myTeamSize = 2;

Guid pingPongPlayer = mediator.Send(new CreateGameFormat(new GameFormatCreateDTO()
{
    Name = "PingPongPlayerWinAt3",
    GameType = GameType.PING_PONG,
    CompetitorType = CompetitorType.PLAYER,
    WinAt = 3,
})).Result.Id;

Guid pingPongTeam = mediator.Send(new CreateGameFormat(new GameFormatCreateDTO()
{
    Name = "PingPongTeamWinAt3",
    GameType = GameType.PING_PONG,
    CompetitorType = CompetitorType.TEAM,
    TeamSize = myTeamSize,
    WinAt = 3,
})).Result.Id;

Guid pingPongTeamTimed = mediator.Send(new CreateGameFormat(new GameFormatCreateDTO()
{
    Name = "PingPongTeamWinAt3Duration5",
    GameType = GameType.PING_PONG,
    CompetitorType = CompetitorType.TEAM,
    TeamSize = myTeamSize,
    WinAt = 3,
    DurationInSeconds = 5,
})).Result.Id;

Guid player1Id = mediator.Send(new CreateCompetitor(new PlayerCreateDTO() { Name = "Player1" })).Result.Id;
Guid player2Id = mediator.Send(new CreateCompetitor(new PlayerCreateDTO() { Name = "Player2" })).Result.Id;
Guid player3Id = mediator.Send(new CreateCompetitor(new PlayerCreateDTO() { Name = "Player3" })).Result.Id;
Guid player4Id = mediator.Send(new CreateCompetitor(new PlayerCreateDTO() { Name = "Player4" })).Result.Id;
Guid player5Id = mediator.Send(new CreateCompetitor(new PlayerCreateDTO() { Name = "Player5" })).Result.Id;
Guid player6Id = mediator.Send(new CreateCompetitor(new PlayerCreateDTO() { Name = "Player6" })).Result.Id;
Guid player7Id = mediator.Send(new CreateCompetitor(new PlayerCreateDTO() { Name = "Player7" })).Result.Id;
Guid player8Id = mediator.Send(new CreateCompetitor(new PlayerCreateDTO() { Name = "Player8" })).Result.Id;

Guid team1Id = mediator.Send(new CreateCompetitor(new TeamCreateDTO() { Name = "Team1" })).Result.Id;
Guid team2Id = mediator.Send(new CreateCompetitor(new TeamCreateDTO() { Name = "Team2" })).Result.Id;
Guid team3Id = mediator.Send(new CreateCompetitor(new TeamCreateDTO() { Name = "Team3" })).Result.Id;
Guid team4Id = mediator.Send(new CreateCompetitor(new TeamCreateDTO() { Name = "Team4" })).Result.Id;

await mediator.Send(new AddPlayerToTeam(player1Id, team1Id));
await mediator.Send(new AddPlayerToTeam(player2Id, team1Id));
await mediator.Send(new AddPlayerToTeam(player3Id, team2Id));
await mediator.Send(new AddPlayerToTeam(player4Id, team2Id));
await mediator.Send(new AddPlayerToTeam(player5Id, team3Id));
await mediator.Send(new AddPlayerToTeam(player6Id, team3Id));
await mediator.Send(new AddPlayerToTeam(player7Id, team4Id));
await mediator.Send(new AddPlayerToTeam(player8Id, team4Id));

await mediator.Send(new ChangeTeamPlayerStatus(team1Id, player1Id, true));
await mediator.Send(new ChangeTeamPlayerStatus(team1Id, player2Id, true));
await mediator.Send(new ChangeTeamPlayerStatus(team2Id, player3Id, true));
await mediator.Send(new ChangeTeamPlayerStatus(team2Id, player4Id, true));
await mediator.Send(new ChangeTeamPlayerStatus(team3Id, player5Id, true));
await mediator.Send(new ChangeTeamPlayerStatus(team3Id, player6Id, true));
await mediator.Send(new ChangeTeamPlayerStatus(team4Id, player7Id, true));
await mediator.Send(new ChangeTeamPlayerStatus(team4Id, player8Id, true));

string location = "Amdaris";

OneVSAllCompetitionCreateDTO createOVAC(string name, Guid gameFormat)
{
    return new OneVSAllCompetitionCreateDTO()
    {
        Name = name,
        Location = location,
        StartTime = DateTime.Now,
        GameFormat = gameFormat,
        BreakInSeconds = null
    };
}
CompetitionCreateDTO createTC(string name, Guid gameFormat)
{
    return new TournamentCompetitionCreateDTO()
    {
        Name = name,
        Location = location,
        StartTime = DateTime.Now,
        GameFormat = gameFormat,
        BreakInSeconds = null,
        StageLevel = 0
    };
};

Guid competition1Id = mediator.Send(new CreateCompetition(createOVAC("c1", pingPongPlayer))).Result.Id;
Guid competition2Id = mediator.Send(new CreateCompetition(createOVAC("c2", pingPongTeam))).Result.Id;
Guid competition3Id = mediator.Send(new CreateCompetition(createTC("c3", pingPongPlayer))).Result.Id;
Guid competition4Id = mediator.Send(new CreateCompetition(createTC("c4", pingPongTeam))).Result.Id;
Guid competition5Id = mediator.Send(new CreateCompetition(createTC("c5", pingPongTeamTimed))).Result.Id;

await mediator.Send(new AddCompetitorToCompetition(player1Id, competition1Id));
await mediator.Send(new AddCompetitorToCompetition(player2Id, competition1Id));
await mediator.Send(new AddCompetitorToCompetition(player3Id, competition1Id));

await mediator.Send(new AddCompetitorToCompetition(team1Id, competition2Id));
await mediator.Send(new AddCompetitorToCompetition(team2Id, competition2Id));
await mediator.Send(new AddCompetitorToCompetition(team3Id, competition2Id));

await mediator.Send(new AddCompetitorToCompetition(player1Id, competition3Id));
await mediator.Send(new AddCompetitorToCompetition(player2Id, competition3Id));
await mediator.Send(new AddCompetitorToCompetition(player3Id, competition3Id));
await mediator.Send(new AddCompetitorToCompetition(player4Id, competition3Id));

await mediator.Send(new AddCompetitorToCompetition(team1Id, competition4Id));
await mediator.Send(new AddCompetitorToCompetition(team2Id, competition4Id));
await mediator.Send(new AddCompetitorToCompetition(team3Id, competition4Id));
await mediator.Send(new AddCompetitorToCompetition(team4Id, competition4Id));

await mediator.Send(new AddCompetitorToCompetition(team1Id, competition5Id));
await mediator.Send(new AddCompetitorToCompetition(team2Id, competition5Id));
await mediator.Send(new AddCompetitorToCompetition(team3Id, competition5Id));
await mediator.Send(new AddCompetitorToCompetition(team4Id, competition5Id));

Guid competitionToTestId = competition4Id;

//await mediator.Send(new ChangeTeamPlayerStatus(team3Id, player6Id, false));
//await mediator.Send(new RemovePlayerFromTeam(player6Id, team3Id));

int i = 0;

await simulateCompetition(competitionToTestId);

async Task simulateCompetition(Guid competitionId)
{
    Console.WriteLine();
    await mediator.Send(new StopCompetitionRegistration(competitionId));
    await mediator.Send(new StartCompetition(competitionId));
    Console.WriteLine();

    async Task<List<Match>> getCompetitionUnfinishedMatches()
        => (await serviceProvider.GetRequiredService<IUnitOfWork>().CompetitionRepository.GetById(competitionId))!.Matches
            .Where(match => match.Status is MatchStatus.NOT_STARTED or MatchStatus.STARTED).ToList();

    while ((await getCompetitionUnfinishedMatches()).Count != 0)
        foreach (Match match in await getCompetitionUnfinishedMatches())
            await SimulateMatch(match.Id, competitionId);

    await mediator.Send(new EndCompetition(competitionId));

    CompetitionGetDTO competitionResponseDTO = await mediator.Send(new GetCompetitionById(competitionId));

    Console.WriteLine($"Matches played: {competitionResponseDTO.Matches.Count}");
    competitionResponseDTO.Matches
        .Select(match => mediator.Send(new GetMatchById(match.Id)).Result)
        .ToList()
        .ForEach(match => Console.WriteLine($"{match.Status}: {match.StartTime}-{match.EndTime}: " +
        $"{match.CompetitorOne.Name}={match.CompetitorOnePoints}-{match.CompetitorTwo.Name}={match.CompetitorTwoPoints}"));

    IEnumerable<CompetitorDisplayDTO> competitionWinners = await mediator.Send(new GetCompetitionWinners(competitionId));

    if (!competitionWinners.Any()) Console.WriteLine($"Competition {competitionResponseDTO.Name} can't continue!");
    else competitionWinners.ToList().ForEach(winner =>
        Console.WriteLine($"The winner of competition {competitionResponseDTO.Name} is competitor {winner.Name}!"));

    Console.WriteLine();

    Console.WriteLine("Competitor's ratings:");
    competitionResponseDTO.Competitors
        .Select(competitor => new
        {
            competitor.Name,
            Rating = mediator.Send(new GetCompetitorWinRatingForGameType(competitor.Id, competitionResponseDTO.GameType)).Result
        })
        .ToList()
        .ForEach(o => Console.WriteLine($"{o.Name}: {o.Rating}"));
}

async Task SimulateMatch(Guid matchId, Guid competitionId)
{
    MatchGetDTO match = await mediator.Send(new StartMatch(matchId));

    if (match.Status is not MatchStatus.STARTED) return;

    CompetitionGetDTO competition = await mediator.Send(new GetCompetitionById(competitionId));

    //if (++i == 2)
    ////if (++i == competition.Competitors.Count / 2 + 1)
    //{
    //    await mediator.Send(new CancelMatch(matchId));
    //    return;
    //}

    while (mediator.Send(new GetMatchById(matchId)).Result.Status is not MatchStatus.FINISHED)
    {
        match = await mediator.Send(new GetMatchById(matchId));
        Guid? scorer = null;

        if (competition.CompetitorType is CompetitorType.PLAYER)
        {
            scorer = new Random().Next(2) == 0 ? match.CompetitorOne.Id : match.CompetitorTwo.Id;
        }
        else if (competition.CompetitorType is CompetitorType.TEAM)
        {
            CompetitorGetDTO competitorOne = await mediator.Send(new GetCompetitorById(match.CompetitorOne.Id));
            CompetitorGetDTO competitorTwo = await mediator.Send(new GetCompetitorById(match.CompetitorTwo.Id));
            int numberOfPlayers = 2 * competition.TeamSize ?? throw new AmdarisProjectException("");
            int random = new Random().Next(numberOfPlayers);
            scorer = random < numberOfPlayers / 2
                ? ((TeamGetDTO)competitorOne).Players[random % (numberOfPlayers / 2)].Id
                : ((TeamGetDTO)competitorTwo).Players[(random - (numberOfPlayers / 2)) % (numberOfPlayers / 2)].Id;
        }

        await mediator.Send(new AddValueToPointValue(scorer ?? throw new AmdarisProjectException(""), match.Id, 1));
    }

    Console.WriteLine($"Winner: {(await mediator.Send(new GetMatchById(matchId))).Winner?.Name}");
    Console.WriteLine();
}