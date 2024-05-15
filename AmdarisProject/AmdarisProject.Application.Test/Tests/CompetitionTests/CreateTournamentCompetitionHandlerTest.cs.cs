using AmdarisProject.Application.Dtos.RequestDTOs.CreateDTOs;
using AmdarisProject.Application.Dtos.ResponseDTOs.CompetitionResponseDTOs;
using AmdarisProject.Application.Handlers.CompetitionHandlers;
using AmdarisProject.Application.Test.ModelBuilders;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models;
using AmdarisProject.Domain.Models.CompetitionModels;
using AmdarisProject.Presentation.Test.Tests;
using Mapster;
using Moq;

namespace AmdarisProject.Application.Test.Tests.CompetitionTests
{
    public class CreateTournamentCompetitionHandlerTest : MockObjectUser
    {
        [Fact]
        public async Task Test_CreateTournamentCompetitionHandler_Success()
        {
            TournamentCompetition model = APBuilder.CreateBasicTournamentCompetition().Get();
            _unitOfWorkMock.Setup(o => o.CompetitionRepository).Returns(_competitionRepositoryMock.Object);
            _unitOfWorkMock.Setup(o => o.GameFormatRepository).Returns(_gameFormatRepositoryMock.Object);
            _mapperMock.Setup(o => o.Map<TournamentCompetition>(It.IsAny<CompetitionCreateDTO>())).Returns(model);
            _gameFormatRepositoryMock.Setup(o => o.GetById(It.IsAny<Guid>())).Returns(Task.FromResult((GameFormat?)model.GameFormat));
            _competitionRepositoryMock.Setup(o => o.Create(It.IsAny<TournamentCompetition>())).Returns(Task.FromResult((Competition)model));
            _mapperMock.Setup(o => o.Map<TournamentCompetitionGetDTO>(It.IsAny<TournamentCompetition>()))
                .Returns(model.Adapt<TournamentCompetitionGetDTO>());
            CreateTournamentCompetition command = new(model.Adapt<CompetitionCreateDTO>());
            CreateTournamentCompetitionHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object,
                GetLogger<CreateTournamentCompetitionHandler>());

            TournamentCompetitionGetDTO response = await handler.Handle(command, default);

            AssertResponse.TournamentCompetitionGetDTO(model, response, createTournamentCompetition: true);
        }

        [Fact]
        public async Task Test_CreateTournamentCompetitionHandler_GameFormatNotFound_throws_APNotFoundException()
        {
            TournamentCompetition model = APBuilder.CreateBasicTournamentCompetition().Get();
            _unitOfWorkMock.Setup(o => o.GameFormatRepository).Returns(_gameFormatRepositoryMock.Object);
            _mapperMock.Setup(o => o.Map<TournamentCompetition>(It.IsAny<CompetitionCreateDTO>())).Returns(model);
            _gameFormatRepositoryMock.Setup(o => o.GetById(It.IsAny<Guid>())).Returns(Task.FromResult((GameFormat?)null));
            CreateTournamentCompetition command = new(model.Adapt<CompetitionCreateDTO>());
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
        public async Task Test_CreateTournamentCompetitionHandler_TimeConstraints_throws_APArgumentException(TournamentCompetition model)
        {
            _unitOfWorkMock.Setup(o => o.GameFormatRepository).Returns(_gameFormatRepositoryMock.Object);
            _mapperMock.Setup(o => o.Map<TournamentCompetition>(It.IsAny<CompetitionCreateDTO>())).Returns(model);
            _gameFormatRepositoryMock.Setup(o => o.GetById(It.IsAny<Guid>())).Returns(Task.FromResult((GameFormat?)model.GameFormat));
            CreateTournamentCompetition command = new(model.Adapt<CompetitionCreateDTO>());
            CreateTournamentCompetitionHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object,
                GetLogger<CreateTournamentCompetitionHandler>());

            await Assert.ThrowsAsync<APArgumentException>(async () => await handler.Handle(command, default));
        }

        [Fact]
        public async Task Test_CreateTournamentCompetitionHandler_RollbackIsCalled_throws_Exception()
        {
            TournamentCompetition model = APBuilder.CreateBasicTournamentCompetition().Get();
            _unitOfWorkMock.Setup(o => o.CompetitionRepository).Returns(_competitionRepositoryMock.Object);
            _unitOfWorkMock.Setup(o => o.GameFormatRepository).Returns(_gameFormatRepositoryMock.Object);
            _mapperMock.Setup(o => o.Map<TournamentCompetition>(It.IsAny<CompetitionCreateDTO>())).Returns(model);
            _gameFormatRepositoryMock.Setup(o => o.GetById(It.IsAny<Guid>())).Returns(Task.FromResult((GameFormat?)model.GameFormat));
            _competitionRepositoryMock.Setup(o => o.Create(It.IsAny<TournamentCompetition>())).Throws<Exception>();
            CreateTournamentCompetition command = new(model.Adapt<CompetitionCreateDTO>());
            CreateTournamentCompetitionHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object,
                GetLogger<CreateTournamentCompetitionHandler>());

            await Assert.ThrowsAsync<Exception>(async () => await handler.Handle(command, default));

            _unitOfWorkMock.Verify(o => o.RollbackTransactionAsync(), Times.Once);
        }
    }
}
