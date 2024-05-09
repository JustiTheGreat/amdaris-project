using AmdarisProject.Application.Dtos.DisplayDTOs.CompetitorDisplayDTOs;
using AmdarisProject.Application.Handlers.CompetitorHandlers;
using AmdarisProject.Application.Test.ModelBuilders;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models;
using AmdarisProject.Domain.Models.CompetitionModels;
using AmdarisProject.Domain.Models.CompetitorModels;
using Mapster;
using Moq;

namespace AmdarisProject.Application.Test.Tests.CompetitorTests
{
    public class GetTeamsThatCanBeAddedToCompetitionHandlerTest : MockObjectUser
    {
        [Fact]
        public async Task Test_GetTeamsThatCanBeAddedToCompetitionHandler_Success()
        {
            GameFormat gameFormat = Builder.CreateBasicGameFormat()
                .SetCompetitorType(Domain.Enums.CompetitorType.TEAM).SetTeamSize(2).Get();
            Competition competition = Builder.CreateBasicOneVSAllCompetition().SetGameFormat(gameFormat).Get();
            List<Team> teams = [];
            for (int i = 0; i < _numberOfModelsInAList; i++) teams.Add(Builder.CreateBasicTeam().Get());
            _unitOfWorkMock.Setup(o => o.CompetitionRepository).Returns(_competitionRepositoryMock.Object);
            _unitOfWorkMock.Setup(o => o.CompetitorRepository).Returns(_competitorRepositoryMock.Object);
            _unitOfWorkMock.Setup(o => o.TeamPlayerRepository).Returns(_teamPlayerRepositoryMock.Object);
            _competitionRepositoryMock.Setup(o => o.GetById(It.IsAny<Guid>())).Returns(Task.FromResult((Competition?)competition));
            _competitorRepositoryMock.Setup(o => o.GetTeamsNotInCompetition(It.IsAny<Guid>())).Returns(Task.FromResult((IEnumerable<Team>)teams));
            _teamPlayerRepositoryMock.Setup(o => o.TeamHasTheRequiredNumberOfActivePlayers(It.IsAny<Guid>(), It.IsAny<uint>())).Returns(Task.FromResult(true));
            _mapperMock.Setup(o => o.Map<IEnumerable<TeamDisplayDTO>>(It.IsAny<IEnumerable<Team>>()))
                .Returns(teams.Adapt<IEnumerable<TeamDisplayDTO>>());
            GetTeamsThatCanBeAddedToCompetition command = new(competition.Id);
            GetTeamsThatCanBeAddedToCompetitionHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object,
                GetLogger<GetTeamsThatCanBeAddedToCompetitionHandler>());

            IEnumerable<TeamDisplayDTO> response = await handler.Handle(command, default);

            _competitorRepositoryMock.Verify(o => o.GetTeamsNotInCompetition(It.IsAny<Guid>()), Times.Once);
            _teamPlayerRepositoryMock.Verify(o => o.TeamHasTheRequiredNumberOfActivePlayers(It.IsAny<Guid>(), It.IsAny<uint>()), Times.Exactly(_numberOfModelsInAList));
            for (int i = 0; i < _numberOfModelsInAList; i++)
            {
                Assert.Equal(teams.ElementAt(i).Id, response.ElementAt(i).Id);
                Assert.Equal(teams.ElementAt(i).Name, response.ElementAt(i).Name);
                Assert.Equal(teams.ElementAt(i).Players.Count, response.ElementAt(i).PlayerNames.Count);
                teams.ElementAt(i).Players.ForEach(player =>
                {
                    Assert.Equal(player.Name, response.ElementAt(i).PlayerNames.FirstOrDefault(name => name.Equals(player.Name)));
                });
            }
        }

        [Fact]
        public async Task Test_GetTeamsThatCanBeAddedToCompetitionHandler_CompetitionNotFound_throws_APNotFoundException()
        {
            _unitOfWorkMock.Setup(o => o.CompetitionRepository).Returns(_competitionRepositoryMock.Object);
            _competitionRepositoryMock.Setup(o => o.GetById(It.IsAny<Guid>())).Returns(Task.FromResult((Competition?)null));
            GetTeamsThatCanBeAddedToCompetition command = new(It.IsAny<Guid>());
            GetTeamsThatCanBeAddedToCompetitionHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object,
                GetLogger<GetTeamsThatCanBeAddedToCompetitionHandler>());

            await Assert.ThrowsAsync<APNotFoundException>(async () => await handler.Handle(command, default));
        }
    }
}
