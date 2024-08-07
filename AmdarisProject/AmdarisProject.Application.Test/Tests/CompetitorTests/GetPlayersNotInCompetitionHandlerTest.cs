﻿using AmdarisProject.Application.Dtos.ResponseDTOs.DisplayDTOs;
using AmdarisProject.Application.Handlers.CompetitorHandlers;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models.CompetitionModels;
using AmdarisProject.Domain.Models.CompetitorModels;
using AmdarisProject.TestUtils.ModelBuilders;
using Moq;

namespace AmdarisProject.Application.Test.Tests.CompetitorTests
{
    public class GetPlayersNotInCompetitionHandlerTest : ApplicationTestBase
    {
        [Fact]
        public async Task Test_GetPlayersNotInCompetitionHandler_Success()
        {
            Competition competition = APBuilder.CreateBasicOneVSAllCompetition().Get();
            List<Player> players = [];
            for (int i = 0; i < _numberOfModelsInAList; i++) players.Add(APBuilder.CreateBasicPlayer().Get());
            _competitionRepositoryMock.Setup(o => o.GetById(It.IsAny<Guid>())).Returns(Task.FromResult((Competition?)competition));
            _competitorRepositoryMock.Setup(o => o.GetPlayersNotInCompetition(It.IsAny<Guid>()))
                .Returns(Task.FromResult((IEnumerable<Player>)players));
            _mapperMock.Setup(o => o.Map<IEnumerable<CompetitorDisplayDTO>>(It.IsAny<IEnumerable<Player>>()))
                .Returns(_mapper.Map<IEnumerable<CompetitorDisplayDTO>>(players));
            GetPlayersNotInCompetition command = new(It.IsAny<Guid>());
            GetPlayersNotInCompetitionHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object,
                GetLogger<GetPlayersNotInCompetitionHandler>());

            IEnumerable<CompetitorDisplayDTO> response = await handler.Handle(command, default);

            _competitorRepositoryMock.Verify(o => o.GetPlayersNotInCompetition(It.IsAny<Guid>()), Times.Once);
            Assert.Equal(players.Count, response.Count());
            for (int i = 0; i < players.Count; i++)
            {
                Player player = players[i];
                CompetitorDisplayDTO playerDisplayDTO = response.ElementAt(i);

                Assert.Equal(player.Id, playerDisplayDTO.Id);
                Assert.Equal(player.Name, playerDisplayDTO.Name);
            }
        }

        [Fact]
        public async Task Test_GetPlayersNotInCompetitionHandler_CompetitionNotFound_throws_APNotFoundException()
        {
            _competitionRepositoryMock.Setup(o => o.GetById(It.IsAny<Guid>())).Returns(Task.FromResult((Competition?)null));
            GetPlayersNotInCompetition command = new(It.IsAny<Guid>());
            GetPlayersNotInCompetitionHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object,
                GetLogger<GetPlayersNotInCompetitionHandler>());

            await Assert.ThrowsAsync<APNotFoundException>(async () => await handler.Handle(command, default));

            _competitionRepositoryMock.Verify(o => o.GetById(It.IsAny<Guid>()), Times.Once);
        }
    }
}
