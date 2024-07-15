using AmdarisProject.Application.Dtos.RequestDTOs.CreateDTOs;
using AmdarisProject.Application.Dtos.ResponseDTOs.CompetitionResponseDTOs;
using AmdarisProject.Application.Handlers.CompetitionHandlers;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models;
using AmdarisProject.Domain.Models.CompetitionModels;
using AmdarisProject.TestUtils;
using AmdarisProject.TestUtils.ModelBuilders;
using Moq;

namespace AmdarisProject.Application.Test.Tests.CompetitionTests
{
    public class CreateTournamentCompetitionHandlerTest : ApplicationTestBase
    {
        [Fact]
        public async Task Test_CreateTournamentCompetitionHandler_Success()
        {
            TournamentCompetition tournamentCompetition = APBuilder.CreateBasicTournamentCompetition().Get();
            _mapperMock.Setup(o => o.Map<TournamentCompetition>(It.IsAny<CompetitionCreateDTO>())).Returns(tournamentCompetition);
            _gameFormatRepositoryMock.Setup(o => o.GetById(It.IsAny<Guid>())).Returns(Task.FromResult((GameFormat?)tournamentCompetition.GameFormat));
            _competitionRepositoryMock.Setup(o => o.Create(It.IsAny<TournamentCompetition>())).Returns(Task.FromResult((Competition)tournamentCompetition));
            _mapperMock.Setup(o => o.Map<TournamentCompetitionGetDTO>(It.IsAny<TournamentCompetition>()))
                .Returns(_mapper.Map<TournamentCompetitionGetDTO>(tournamentCompetition));
            CreateTournamentCompetition command = new(_mapper.Map<CompetitionCreateDTO>(tournamentCompetition));
            CreateTournamentCompetitionHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object,
                GetLogger<CreateTournamentCompetitionHandler>());

            TournamentCompetitionGetDTO response = await handler.Handle(command, default);

            AssertResponse.TournamentCompetitionGetDTO(tournamentCompetition, response, createTournamentCompetition: true);
        }

        [Fact]
        public async Task Test_CreateTournamentCompetitionHandler_GameFormatNotFound_throws_APNotFoundException()
        {
            TournamentCompetition tournamentCompetition = APBuilder.CreateBasicTournamentCompetition().Get();
            _mapperMock.Setup(o => o.Map<TournamentCompetition>(It.IsAny<CompetitionCreateDTO>())).Returns(tournamentCompetition);
            _gameFormatRepositoryMock.Setup(o => o.GetById(It.IsAny<Guid>())).Returns(Task.FromResult((GameFormat?)null));
            CreateTournamentCompetition command = new(_mapper.Map<CompetitionCreateDTO>(tournamentCompetition));
            CreateTournamentCompetitionHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object,
                GetLogger<CreateTournamentCompetitionHandler>());

            await Assert.ThrowsAsync<APNotFoundException>(async () => await handler.Handle(command, default));
        }

        public static TheoryData<TournamentCompetition> TimeConstraints => new()
        {
            APBuilder.CreateBasicTournamentCompetition().SetBreakInMinutes(null)
                .SetGameFormat(APBuilder.CreateBasicGameFormat().SetDurationInMinutes(5).Get()).Get(),
            APBuilder.CreateBasicTournamentCompetition().SetBreakInMinutes(5)
                .SetGameFormat(APBuilder.CreateBasicGameFormat().SetDurationInMinutes(null).Get()).Get()
        };

        [Theory]
        [MemberData(nameof(TimeConstraints))]
        public async Task Test_CreateTournamentCompetitionHandler_TimeConstraints_throws_APArgumentException(TournamentCompetition tournamentCompetition)
        {
            _mapperMock.Setup(o => o.Map<TournamentCompetition>(It.IsAny<CompetitionCreateDTO>())).Returns(tournamentCompetition);
            _gameFormatRepositoryMock.Setup(o => o.GetById(It.IsAny<Guid>())).Returns(Task.FromResult((GameFormat?)tournamentCompetition.GameFormat));
            CreateTournamentCompetition command = new(_mapper.Map<CompetitionCreateDTO>(tournamentCompetition));
            CreateTournamentCompetitionHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object,
                GetLogger<CreateTournamentCompetitionHandler>());

            await Assert.ThrowsAsync<APArgumentException>(async () => await handler.Handle(command, default));
        }

        [Fact]
        public async Task Test_CreateTournamentCompetitionHandler_RollbackIsCalled_throws_Exception()
        {
            TournamentCompetition tournamentCompetition = APBuilder.CreateBasicTournamentCompetition().Get();
            _mapperMock.Setup(o => o.Map<TournamentCompetition>(It.IsAny<CompetitionCreateDTO>())).Returns(tournamentCompetition);
            _gameFormatRepositoryMock.Setup(o => o.GetById(It.IsAny<Guid>())).Returns(Task.FromResult((GameFormat?)tournamentCompetition.GameFormat));
            _competitionRepositoryMock.Setup(o => o.Create(It.IsAny<TournamentCompetition>())).Throws<Exception>();
            CreateTournamentCompetition command = new(_mapper.Map<CompetitionCreateDTO>(tournamentCompetition));
            CreateTournamentCompetitionHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object,
                GetLogger<CreateTournamentCompetitionHandler>());

            await Assert.ThrowsAsync<Exception>(async () => await handler.Handle(command, default));

            _unitOfWorkMock.Verify(o => o.RollbackTransactionAsync(), Times.Once);
        }
    }
}
