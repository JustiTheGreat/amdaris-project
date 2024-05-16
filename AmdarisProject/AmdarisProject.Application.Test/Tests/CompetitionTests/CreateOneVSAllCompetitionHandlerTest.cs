using AmdarisProject.Application.Dtos.RequestDTOs.CreateDTOs;
using AmdarisProject.Application.Dtos.ResponseDTOs.CompetitionResponseDTOs;
using AmdarisProject.Application.Handlers.CompetitionHandlers;
using AmdarisProject.Application.Test.ModelBuilders;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models;
using AmdarisProject.Domain.Models.CompetitionModels;
using AmdarisProject.Presentation.Test.Tests;
using Moq;

namespace AmdarisProject.Application.Test.Tests.CompetitionTests
{
    //public class CreateOneVSAllCompetitionHandlerTest : ApplicationTestBase
    //{
    //    [Fact]
    //    public async Task Test_CreateOneVSAllCompetitionHandler_Success()
    //    {
    //        OneVSAllCompetition model = APBuilder.CreateBasicOneVSAllCompetition().Get();
    //        _unitOfWorkMock.Setup(o => o.CompetitionRepository).Returns(_competitionRepositoryMock.Object);
    //        _unitOfWorkMock.Setup(o => o.GameFormatRepository).Returns(_gameFormatRepositoryMock.Object);
    //        _mapperMock.Setup(o => o.Map<OneVSAllCompetition>(It.IsAny<CompetitionCreateDTO>())).Returns(model);
    //        _gameFormatRepositoryMock.Setup(o => o.GetById(It.IsAny<Guid>())).Returns(Task.FromResult((GameFormat?)model.GameFormat));
    //        _competitionRepositoryMock.Setup(o => o.Create(It.IsAny<OneVSAllCompetition>())).Returns(Task.FromResult((Competition)model));
    //        _mapperMock.Setup(o => o.Map<OneVSAllCompetitionGetDTO>(It.IsAny<OneVSAllCompetition>()))
    //            .Returns(model.Adapt<OneVSAllCompetitionGetDTO>());
    //        CreateOneVSAllCompetition command = new(model.Adapt<CompetitionCreateDTO>());
    //        CreateOneVSAllCompetitionHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object,
    //            GetLogger<CreateOneVSAllCompetitionHandler>());

    //        OneVSAllCompetitionGetDTO response = await handler.Handle(command, default);

    //        AssertResponse.OneVSAllCompetitionGetDTO(model, response, createOneVSAllCompetition: true);
    //    }

    //    [Fact]
    //    public async Task Test_CreateOneVSAllCompetitionHandler_GameFormatNotFound_throws_APNotFoundException()
    //    {
    //        OneVSAllCompetition model = APBuilder.CreateBasicOneVSAllCompetition().Get();
    //        _unitOfWorkMock.Setup(o => o.GameFormatRepository).Returns(_gameFormatRepositoryMock.Object);
    //        _mapperMock.Setup(o => o.Map<OneVSAllCompetition>(It.IsAny<CompetitionCreateDTO>())).Returns(model);
    //        _gameFormatRepositoryMock.Setup(o => o.GetById(It.IsAny<Guid>())).Returns(Task.FromResult((GameFormat?)null));
    //        CreateOneVSAllCompetition command = new(model.Adapt<CompetitionCreateDTO>());
    //        CreateOneVSAllCompetitionHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object,
    //            GetLogger<CreateOneVSAllCompetitionHandler>());

    //        await Assert.ThrowsAsync<APNotFoundException>(async () => await handler.Handle(command, default));
    //    }

    //    public static TheoryData<OneVSAllCompetition> TimeConstraints => new()
    //    {
    //        APBuilder.CreateBasicOneVSAllCompetition().SetBreakInMinutes(null)
    //            .SetGameFormat(APBuilder.CreateBasicGameFormat().SetDurationInMinutes(5).Get()).Get(),
    //        APBuilder.CreateBasicOneVSAllCompetition().SetBreakInMinutes(5)
    //            .SetGameFormat(APBuilder.CreateBasicGameFormat().SetDurationInMinutes(null).Get()).Get()
    //    };

    //    [Theory]
    //    [MemberData(nameof(TimeConstraints))]
    //    public async Task Test_CreateOneVSAllCompetitionHandler_TimeConstraints_throws_APArgumentException(OneVSAllCompetition model)
    //    {
    //        _unitOfWorkMock.Setup(o => o.GameFormatRepository).Returns(_gameFormatRepositoryMock.Object);
    //        _mapperMock.Setup(o => o.Map<OneVSAllCompetition>(It.IsAny<CompetitionCreateDTO>())).Returns(model);
    //        _gameFormatRepositoryMock.Setup(o => o.GetById(It.IsAny<Guid>())).Returns(Task.FromResult((GameFormat?)model.GameFormat));
    //        CreateOneVSAllCompetition command = new(model.Adapt<CompetitionCreateDTO>());
    //        CreateOneVSAllCompetitionHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object,
    //            GetLogger<CreateOneVSAllCompetitionHandler>());

    //        await Assert.ThrowsAsync<APArgumentException>(async () => await handler.Handle(command, default));
    //    }

    //    [Fact]
    //    public async Task Test_CreateOneVSAllCompetitionHandler_RollbackIsCalled_throws_Exception()
    //    {
    //        OneVSAllCompetition model = APBuilder.CreateBasicOneVSAllCompetition().Get();
    //        _unitOfWorkMock.Setup(o => o.CompetitionRepository).Returns(_competitionRepositoryMock.Object);
    //        _unitOfWorkMock.Setup(o => o.GameFormatRepository).Returns(_gameFormatRepositoryMock.Object);
    //        _mapperMock.Setup(o => o.Map<OneVSAllCompetition>(It.IsAny<CompetitionCreateDTO>())).Returns(model);
    //        _gameFormatRepositoryMock.Setup(o => o.GetById(It.IsAny<Guid>())).Returns(Task.FromResult((GameFormat?)model.GameFormat));
    //        _competitionRepositoryMock.Setup(o => o.Create(It.IsAny<OneVSAllCompetition>())).Throws<Exception>();
    //        CreateOneVSAllCompetition command = new(model.Adapt<CompetitionCreateDTO>());
    //        CreateOneVSAllCompetitionHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object,
    //            GetLogger<CreateOneVSAllCompetitionHandler>());

    //        await Assert.ThrowsAsync<Exception>(async () => await handler.Handle(command, default));

    //        _unitOfWorkMock.Verify(o => o.RollbackTransactionAsync(), Times.Once);
    //    }
    //}
}
