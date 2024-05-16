using AmdarisProject.Application.Common.Models;
using AmdarisProject.Application.Dtos.DisplayDTOs;
using AmdarisProject.Application.Handlers.CompetitionHandlers;
using AmdarisProject.Domain.Models.CompetitionModels;
using AmdarisProject.TestUtils.ModelBuilders;
using Moq;

namespace AmdarisProject.Application.Test.Tests.CompetitionTests
{
    public class GetPaginatedCompetitionsHandlerTest : ApplicationTestBase
    {
        [Fact]
        public async Task Test_GetPaginatedCompetitionsHandler_Success()
        {

            List<Competition> competitions = [];
            for (int i = 0; i < _numberOfModelsInAList; i++)
            {
                if (i < _numberOfModelsInAList / 2)
                    competitions.Add(APBuilder.CreateBasicOneVSAllCompetition().Get());
                else
                    competitions.Add(APBuilder.CreateBasicTournamentCompetition().Get());
            }
            _competitionRepositoryMock.Setup(o => o.GetPaginatedData(It.IsAny<PagedRequest>()))
                .Returns(Task.FromResult((IEnumerable<Competition>)competitions));
            _mapperMock.Setup(o => o.Map<IEnumerable<CompetitionDisplayDTO>>(It.IsAny<IEnumerable<Competition>>()))
                .Returns(_mapper.Map<IEnumerable<CompetitionDisplayDTO>>(competitions));
            GetPaginatedCompetitions command = new(_pagedRequest);
            GetPaginatedCompetitionsHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object,
                GetLogger<GetPaginatedCompetitionsHandler>());

            PaginatedResult<CompetitionDisplayDTO> response = await handler.Handle(command, default);

            _competitionRepositoryMock.Verify(o => o.GetPaginatedData(It.IsAny<PagedRequest>()), Times.Once);
            Assert.Equal(_pagedRequest.PageIndex, response.PageIndex);
            Assert.Equal(_pagedRequest.PageSize, response.PageSize);
            Assert.Equal(_pagedRequest.PageSize, response.Total);
            Assert.Equal(_pagedRequest.PageSize, response.Items.Count());
            for (int i = 0; i < competitions.Count; i++)
            {
                Competition competition = competitions[i];
                CompetitionDisplayDTO competitionDisplayDTO = response.Items.ElementAt(i);

                Assert.Equal(competition.Id, competitionDisplayDTO.Id);
                Assert.Equal(competition.Name, competitionDisplayDTO.Name);
                Assert.Equal(competition.Status, competitionDisplayDTO.Status);
                Assert.Equal(competition.GameFormat.GameType, competitionDisplayDTO.GameType);
                Assert.Equal(competition.GameFormat.CompetitorType, competitionDisplayDTO.CompetitorType);
            }
        }
    }
}
