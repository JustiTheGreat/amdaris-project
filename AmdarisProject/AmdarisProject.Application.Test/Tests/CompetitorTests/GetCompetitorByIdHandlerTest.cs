﻿using AmdarisProject.Application.Dtos.ResponseDTOs.CompetitorResponseDTOs;
using AmdarisProject.Application.Handlers.CompetitorHandlers;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models.CompetitorModels;
using AmdarisProject.TestUtils;
using AmdarisProject.TestUtils.ModelBuilders;
using AutoMapper;
using Moq;

namespace AmdarisProject.Application.Test.Tests.CompetitorTests
{
    public class GetCompetitorByIdHandlerTest : ApplicationTestBase
    {
        [Fact]
        public async Task Test_GetCompetitorByIdHandler_Player_Success()
        {
            Player player = APBuilder.CreateBasicPlayer().Get();
            _competitorRepositoryMock.Setup(o => o.GetById(It.IsAny<Guid>())).Returns(Task.FromResult((Competitor?)player));
            _mapperMock.Setup(o => o.Map<PlayerGetDTO>(It.IsAny<Player>())).Returns(_mapper.Map<PlayerGetDTO>(player));
            GetCompetitorById command = new(player.Id);
            GetCompetitorByIdHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object, GetLogger<GetCompetitorByIdHandler>());

            CompetitorGetDTO response = await handler.Handle(command, default);

            Assert.True(response is PlayerGetDTO);
            AssertResponse.PlayerGetDTO(player, (PlayerGetDTO)response);
        }

        [Fact]
        public async Task Test_GetCompetitorByIdHandler_Team_Success()
        {
            Team team = APBuilder.CreateBasicTeam().Get();
            _competitorRepositoryMock.Setup(o => o.GetById(It.IsAny<Guid>())).Returns(Task.FromResult((Competitor?)team));
            _mapperMock.Setup(o => o.Map<TeamGetDTO>(It.IsAny<Team>())).Returns(_mapper.Map<TeamGetDTO>(team));
            GetCompetitorById command = new(team.Id);
            GetCompetitorByIdHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object, GetLogger<GetCompetitorByIdHandler>());

            CompetitorGetDTO response = await handler.Handle(command, default);

            Assert.True(response is TeamGetDTO);
            AssertResponse.TeamGetDTO(team, (TeamGetDTO)response);
        }

        [Fact]
        public async Task Test_GetCompetitorByIdHandler_CompetitorNotFound_throws_APNotFoundException()
        {
            _competitorRepositoryMock.Setup(o => o.GetById(It.IsAny<Guid>())).Returns(Task.FromResult((Competitor?)null));
            GetCompetitorById command = new(It.IsAny<Guid>());
            GetCompetitorByIdHandler handler = new(_unitOfWorkMock.Object, It.IsAny<IMapper>(), GetLogger<GetCompetitorByIdHandler>());

            await Assert.ThrowsAsync<APNotFoundException>(async () => await handler.Handle(command, default));

            _competitorRepositoryMock.Verify(o => o.GetById(It.IsAny<Guid>()), Times.Once);
        }
    }
}
