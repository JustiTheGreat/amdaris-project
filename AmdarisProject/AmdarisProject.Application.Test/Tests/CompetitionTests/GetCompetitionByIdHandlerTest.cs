using AmdarisProject.Application.Dtos.ResponseDTOs.CompetitionResponseDTOs;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models.CompetitionModels;
using AmdarisProject.handlers.competition;
using AmdarisProject.TestUtils;
using AmdarisProject.TestUtils.ModelBuilders;
using AutoMapper;
using Moq;

namespace AmdarisProject.Application.Test.Tests.CompetitionTests
{
    public class GetCompetitionByIdHandlerTest : ApplicationTestBase
    {
        [Fact]
        public async Task Test_GetCompetitionByIdHandler_OneVSAllCompetition_Success()
        {
            OneVSAllCompetition oneVSAllCompetition = APBuilder.CreateBasicOneVSAllCompetition().Get();
            _competitionRepositoryMock.Setup(o => o.GetById(It.IsAny<Guid>()))
                .Returns(Task.FromResult((Competition?)oneVSAllCompetition));
            _mapperMock.Setup(o => o.Map<OneVSAllCompetitionGetDTO>(It.IsAny<OneVSAllCompetition>()))
                .Returns(_mapper.Map<OneVSAllCompetitionGetDTO>(oneVSAllCompetition));
            GetCompetitionById command = new(oneVSAllCompetition.Id);
            GetCompetitionByIdHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object,
                GetLogger<GetCompetitionByIdHandler>());

            CompetitionGetDTO response = await handler.Handle(command, default);

            Assert.True(response is OneVSAllCompetitionGetDTO);
            AssertResponse.OneVSAllCompetitionGetDTO(oneVSAllCompetition, (OneVSAllCompetitionGetDTO)response);
        }

        [Fact]
        public async Task Test_GetCompetitionByIdHandler_TournamentCompetition_Success()
        {
            TournamentCompetition tournamentCompetition = APBuilder.CreateBasicTournamentCompetition().Get();
            _competitionRepositoryMock.Setup(o => o.GetById(It.IsAny<Guid>()))
                .Returns(Task.FromResult((Competition?)tournamentCompetition));
            _mapperMock.Setup(o => o.Map<TournamentCompetitionGetDTO>(It.IsAny<TournamentCompetition>()))
                .Returns(_mapper.Map<TournamentCompetitionGetDTO>(tournamentCompetition));
            GetCompetitionById command = new(tournamentCompetition.Id);
            GetCompetitionByIdHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object,
                GetLogger<GetCompetitionByIdHandler>());

            CompetitionGetDTO response = await handler.Handle(command, default);

            Assert.True(response is TournamentCompetitionGetDTO);
            AssertResponse.TournamentCompetitionGetDTO(tournamentCompetition, (TournamentCompetitionGetDTO)response);
        }

        [Fact]
        public async Task Test_GetCompetitionByIdHandler_CompetitionNotFound_throws_APNotFoundException()
        {
            _competitionRepositoryMock.Setup(o => o.GetById(It.IsAny<Guid>())).Returns(Task.FromResult((Competition?)null));
            GetCompetitionById command = new(It.IsAny<Guid>());
            GetCompetitionByIdHandler handler = new(_unitOfWorkMock.Object, It.IsAny<IMapper>(), GetLogger<GetCompetitionByIdHandler>());

            await Assert.ThrowsAsync<APNotFoundException>(async () => await handler.Handle(command, default));
        }
    }
}
