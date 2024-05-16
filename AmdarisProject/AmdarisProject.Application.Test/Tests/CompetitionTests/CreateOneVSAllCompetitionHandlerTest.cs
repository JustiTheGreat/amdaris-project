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
    public class CreateOneVSAllCompetitionHandlerTest : ApplicationTestBase
    {
        [Fact]
        public async Task Test_CreateOneVSAllCompetitionHandler_Success()
        {
            OneVSAllCompetition oneVSAllCompetition = APBuilder.CreateBasicOneVSAllCompetition().Get();
            _mapperMock.Setup(o => o.Map<OneVSAllCompetition>(It.IsAny<CompetitionCreateDTO>())).Returns(oneVSAllCompetition);
            _gameFormatRepositoryMock.Setup(o => o.GetById(It.IsAny<Guid>())).Returns(Task.FromResult((GameFormat?)oneVSAllCompetition.GameFormat));
            _competitionRepositoryMock.Setup(o => o.Create(It.IsAny<OneVSAllCompetition>())).Returns(Task.FromResult((Competition)oneVSAllCompetition));
            _mapperMock.Setup(o => o.Map<OneVSAllCompetitionGetDTO>(It.IsAny<OneVSAllCompetition>()))
                .Returns(_mapper.Map<OneVSAllCompetitionGetDTO>(oneVSAllCompetition));
            CreateOneVSAllCompetition command = new(_mapper.Map<CompetitionCreateDTO>(oneVSAllCompetition));
            CreateOneVSAllCompetitionHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object,
                GetLogger<CreateOneVSAllCompetitionHandler>());

            OneVSAllCompetitionGetDTO response = await handler.Handle(command, default);

            AssertResponse.OneVSAllCompetitionGetDTO(oneVSAllCompetition, response, createOneVSAllCompetition: true);
        }

        [Fact]
        public async Task Test_CreateOneVSAllCompetitionHandler_GameFormatNotFound_throws_APNotFoundException()
        {
            OneVSAllCompetition oneVSAllCompetition = APBuilder.CreateBasicOneVSAllCompetition().Get();
            _mapperMock.Setup(o => o.Map<OneVSAllCompetition>(It.IsAny<CompetitionCreateDTO>())).Returns(oneVSAllCompetition);
            _gameFormatRepositoryMock.Setup(o => o.GetById(It.IsAny<Guid>())).Returns(Task.FromResult((GameFormat?)null));
            CreateOneVSAllCompetition command = new(_mapper.Map<CompetitionCreateDTO>(oneVSAllCompetition));
            CreateOneVSAllCompetitionHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object,
                GetLogger<CreateOneVSAllCompetitionHandler>());

            await Assert.ThrowsAsync<APNotFoundException>(async () => await handler.Handle(command, default));
        }

        public static TheoryData<OneVSAllCompetition> TimeConstraints => new()
        {
            APBuilder.CreateBasicOneVSAllCompetition().SetBreakInMinutes(null)
                .SetGameFormat(APBuilder.CreateBasicGameFormat().SetDurationInMinutes(5).Get()).Get(),
            APBuilder.CreateBasicOneVSAllCompetition().SetBreakInMinutes(5)
                .SetGameFormat(APBuilder.CreateBasicGameFormat().SetDurationInMinutes(null).Get()).Get()
        };

        [Theory]
        [MemberData(nameof(TimeConstraints))]
        public async Task Test_CreateOneVSAllCompetitionHandler_TimeConstraints_throws_APArgumentException(OneVSAllCompetition oneVSAllCompetition)
        {
            _mapperMock.Setup(o => o.Map<OneVSAllCompetition>(It.IsAny<CompetitionCreateDTO>())).Returns(oneVSAllCompetition);
            _gameFormatRepositoryMock.Setup(o => o.GetById(It.IsAny<Guid>())).Returns(Task.FromResult((GameFormat?)oneVSAllCompetition.GameFormat));
            CreateOneVSAllCompetition command = new(_mapper.Map<CompetitionCreateDTO>(oneVSAllCompetition));
            CreateOneVSAllCompetitionHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object,
                GetLogger<CreateOneVSAllCompetitionHandler>());

            await Assert.ThrowsAsync<APArgumentException>(async () => await handler.Handle(command, default));
        }

        [Fact]
        public async Task Test_CreateOneVSAllCompetitionHandler_RollbackIsCalled_throws_Exception()
        {
            OneVSAllCompetition oneVSAllCompetition = APBuilder.CreateBasicOneVSAllCompetition().Get();
            _mapperMock.Setup(o => o.Map<OneVSAllCompetition>(It.IsAny<CompetitionCreateDTO>())).Returns(oneVSAllCompetition);
            _gameFormatRepositoryMock.Setup(o => o.GetById(It.IsAny<Guid>())).Returns(Task.FromResult((GameFormat?)oneVSAllCompetition.GameFormat));
            _competitionRepositoryMock.Setup(o => o.Create(It.IsAny<OneVSAllCompetition>())).Throws<Exception>();
            CreateOneVSAllCompetition command = new(_mapper.Map<CompetitionCreateDTO>(oneVSAllCompetition));
            CreateOneVSAllCompetitionHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object,
                GetLogger<CreateOneVSAllCompetitionHandler>());

            await Assert.ThrowsAsync<Exception>(async () => await handler.Handle(command, default));

            _unitOfWorkMock.Verify(o => o.RollbackTransactionAsync(), Times.Once);
        }
    }
}
