using AmdarisProject.Application.Dtos.ResponseDTOs.DisplayDTOs;
using AmdarisProject.Application.Handlers.CompetitorHandlers;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models;
using AmdarisProject.Domain.Models.CompetitionModels;
using AmdarisProject.Domain.Models.CompetitorModels;
using AmdarisProject.TestUtils.ModelBuilders;
using Moq;

namespace AmdarisProject.Application.Test.Tests.CompetitorTests
{
    public class GetTeamsThatCanBeAddedToCompetitionHandlerTest : ApplicationTestBase
    {
        [Fact]
        public async Task Test_GetTeamsThatCanBeAddedToCompetitionHandler_Success()
        {
            GameFormat gameFormat = APBuilder.CreateBasicGameFormat()
                .SetCompetitorType(Domain.Enums.CompetitorType.TEAM).SetTeamSize(2).Get();
            Competition competition = APBuilder.CreateBasicOneVSAllCompetition().SetGameFormat(gameFormat).Get();
            List<Team> teams = [];
            for (int i = 0; i < _numberOfModelsInAList; i++) teams.Add(APBuilder.CreateBasicTeam().Get());
            _competitionRepositoryMock.Setup(o => o.GetById(It.IsAny<Guid>())).Returns(Task.FromResult((Competition?)competition));
            _competitorRepositoryMock.Setup(o => o.GetTeamsThatCanBeAddedToCompetition(It.IsAny<Guid>(), It.IsAny<uint>()))
                .Returns(Task.FromResult((IEnumerable<Team>)teams));
            _mapperMock.Setup(o => o.Map<IEnumerable<CompetitorDisplayDTO>>(It.IsAny<IEnumerable<Team>>()))
                .Returns(_mapper.Map<IEnumerable<CompetitorDisplayDTO>>(teams));
            GetTeamsThatCanBeAddedToCompetition command = new(competition.Id);
            GetTeamsThatCanBeAddedToCompetitionHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object,
                GetLogger<GetTeamsThatCanBeAddedToCompetitionHandler>());

            IEnumerable<CompetitorDisplayDTO> response = await handler.Handle(command, default);

            _competitorRepositoryMock.Verify(o => o.GetTeamsThatCanBeAddedToCompetition(It.IsAny<Guid>(), It.IsAny<uint>()), Times.Once);
            Assert.Equal(teams.Count, response.Count());
            for (int i = 0; i < teams.Count; i++)
            {
                Team team = teams[i];
                CompetitorDisplayDTO competitorDisplayDTO = response.ElementAt(i);

                Assert.Equal(team.Id, competitorDisplayDTO.Id);
                Assert.Equal(team.Name, competitorDisplayDTO.Name);
            }
        }

        [Fact]
        public async Task Test_GetTeamsThatCanBeAddedToCompetitionHandler_CompetitionNotFound_throws_APNotFoundException()
        {
            _competitionRepositoryMock.Setup(o => o.GetById(It.IsAny<Guid>())).Returns(Task.FromResult((Competition?)null));
            GetTeamsThatCanBeAddedToCompetition command = new(It.IsAny<Guid>());
            GetTeamsThatCanBeAddedToCompetitionHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object,
                GetLogger<GetTeamsThatCanBeAddedToCompetitionHandler>());

            await Assert.ThrowsAsync<APNotFoundException>(async () => await handler.Handle(command, default));
        }
    }
}
