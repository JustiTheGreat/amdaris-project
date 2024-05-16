using AmdarisProject.Application.Dtos.DisplayDTOs;
using AmdarisProject.Application.Dtos.RequestDTOs.CreateDTOs;
using AmdarisProject.Application.Dtos.ResponseDTOs.CompetitionResponseDTOs;
using AmdarisProject.Application.Handlers.CompetitionHandlers;
using AmdarisProject.Application.Test.ModelBuilders;
using AmdarisProject.Domain.Models;
using AmdarisProject.Domain.Models.CompetitionModels;
using AmdarisProject.handlers.competition;
using AmdarisProject.Presentation.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace AmdarisProject.Presentation.Test.Tests
{
    //public class CompetitionControllerTests : PresentationTestBase<CompetitionController>
    //{
    //    [Fact]
    //    public async Task Test_CreateOneVSAllCompetition_OkStatus()
    //    {
    //        Setup<CreateOneVSAllCompetition, OneVSAllCompetitionGetDTO, CreateOneVSAllCompetitionHandler>();
    //        Seed_CreateOneVSAllCompetition(out OneVSAllCompetition oneVSAllCompetition);

    //        var requestResult = await _controller.CreateOneVSAllCompetition(oneVSAllCompetition.Adapt<CompetitionCreateDTO>());

    //        var result = requestResult as OkObjectResult;
    //        var response = result?.Value as OneVSAllCompetitionGetDTO;
    //        Assert.Equal((int)HttpStatusCode.OK, result!.StatusCode);
    //        Assert.NotNull(response);
    //        AssertResponse.OneVSAllCompetitionGetDTO(oneVSAllCompetition, response, createOneVSAllCompetition: true);
    //    }

    //    private void Seed_CreateOneVSAllCompetition(out OneVSAllCompetition oneVSAllCompetition)
    //    {
    //        GameFormat gameFormat = APBuilder.CreateBasicGameFormat().Get();
    //        _dbContext.Add(gameFormat);
    //        _dbContext.SaveChanges();
    //        oneVSAllCompetition = APBuilder.CreateBasicOneVSAllCompetition().SetGameFormat(gameFormat).Get();
    //    }

    //    [Fact]
    //    public async Task Test_CreateTournamentCompetition_OkStatus()
    //    {
    //        Setup<CreateTournamentCompetition, TournamentCompetitionGetDTO, CreateTournamentCompetitionHandler>();
    //        Seed_CreateTournamentCompetition(out TournamentCompetition tournamentCompetition);

    //        var requestResult = await _controller.CreateTournamentCompetition(tournamentCompetition.Adapt<CompetitionCreateDTO>());

    //        var result = requestResult as OkObjectResult;
    //        var response = result?.Value as TournamentCompetitionGetDTO;
    //        Assert.Equal((int)HttpStatusCode.OK, result!.StatusCode);
    //        Assert.NotNull(response);
    //        AssertResponse.TournamentCompetitionGetDTO(tournamentCompetition, response, createTournamentCompetition: true);
    //    }

    //    private void Seed_CreateTournamentCompetition(out TournamentCompetition tournamentCompetition)
    //    {
    //        GameFormat gameFormat = APBuilder.CreateBasicGameFormat().Get();
    //        _dbContext.Add(gameFormat);
    //        _dbContext.SaveChanges();
    //        tournamentCompetition = APBuilder.CreateBasicTournamentCompetition().SetGameFormat(gameFormat).Get();
    //    }

    //    [Fact]
    //    public async Task Test_GetCompetitionById_OneVSAllCompetition_OkStatus()
    //    {
    //        Setup<GetCompetitionById, CompetitionGetDTO, GetCompetitionByIdHandler>();
    //        Seed_GetCompetitionById(out OneVSAllCompetition oneVSAllCompetition);

    //        var requestResult = await _controller.GetCompetitionById(oneVSAllCompetition.Id);

    //        var result = requestResult as OkObjectResult;
    //        var response = result?.Value as OneVSAllCompetitionGetDTO;
    //        Assert.Equal((int)HttpStatusCode.OK, result!.StatusCode);
    //        Assert.NotNull(response);
    //        AssertResponse.OneVSAllCompetitionGetDTO(oneVSAllCompetition, response);
    //    }

    //    private void Seed_GetCompetitionById(out OneVSAllCompetition oneVSAllCompetition)
    //    {
    //        oneVSAllCompetition = APBuilder.CreateBasicOneVSAllCompetition().Get();
    //        _dbContext.Add(oneVSAllCompetition);
    //        _dbContext.SaveChanges();
    //        Detach(oneVSAllCompetition);
    //    }

    //    [Fact]
    //    public async Task Test_GetCompetitionById_TournamentCompetition_OkStatus()
    //    {
    //        Setup<GetCompetitionById, CompetitionGetDTO, GetCompetitionByIdHandler>();
    //        Seed_GetCompetitionById(out TournamentCompetition tournamentCompetition);

    //        var requestResult = await _controller.GetCompetitionById(tournamentCompetition.Id);

    //        var result = requestResult as OkObjectResult;
    //        var response = result?.Value as TournamentCompetitionGetDTO;
    //        Assert.Equal((int)HttpStatusCode.OK, result!.StatusCode);
    //        Assert.NotNull(response);
    //        AssertResponse.TournamentCompetitionGetDTO(tournamentCompetition, response);
    //    }

    //    private void Seed_GetCompetitionById(out TournamentCompetition tournamentCompetition)
    //    {
    //        tournamentCompetition = APBuilder.CreateBasicTournamentCompetition().Get();
    //        _dbContext.Add(tournamentCompetition);
    //        _dbContext.SaveChanges();
    //        Detach(tournamentCompetition);
    //    }

    //    [Fact]
    //    public async Task Test_GetAllCompetitions_OkStatus()
    //    {
    //        //Setup<GetPaginatedCompetitions, IEnumerable<CompetitionDisplayDTO>, GetPagedCompetitionsHandler>();
    //        //Seed_GetAllCompetitions(out List<Competition> competitions);

    //        //var requestResult = await _controller.GetAllCompetitions();

    //        //var result = requestResult as OkObjectResult;
    //        //var response = result?.Value as IEnumerable<CompetitionDisplayDTO>;
    //        //Assert.Equal((int)HttpStatusCode.OK, result!.StatusCode);
    //        //Assert.NotNull(response);
    //        //Assert.Equal(competitions.Count, response.Count());
    //        //competitions.ForEach(competition =>
    //        //{
    //        //    CompetitionDisplayDTO competitionDisplayDTO =
    //        //        response.First(competitionDisplayDTO => competitionDisplayDTO.Id.Equals(competition.Id));

    //        //    Assert.Equal(competition.Id, competitionDisplayDTO.Id);
    //        //    Assert.Equal(competition.Name, competitionDisplayDTO.Name);
    //        //    Assert.Equal(competition.Status, competitionDisplayDTO.Status);
    //        //    Assert.Equal(competition.GameFormat.GameType, competitionDisplayDTO.GameType);
    //        //    Assert.Equal(competition.GameFormat.CompetitorType, competitionDisplayDTO.CompetitorType);
    //        //});
    //    }

    //    private void Seed_GetAllCompetitions(out List<Competition> competitions)
    //    {
    //        competitions = [];
    //        for (int i = 0; i < _numberOfModelsInAList; i++)
    //        {
    //            Competition competition = APBuilder.CreateBasicOneVSAllCompetition().Get();
    //            _dbContext.Add(competition);
    //            competitions.Add(competition);
    //            competition = APBuilder.CreateBasicTournamentCompetition().Get();
    //            _dbContext.Add(competition);
    //            competitions.Add(competition);
    //        }
    //        _dbContext.SaveChanges();
    //        competitions.ForEach(Detach);
    //    }
    //}
}
