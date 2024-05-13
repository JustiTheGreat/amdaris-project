using AmdarisProject.Application.Dtos.ResponseDTOs;
using AmdarisProject.Application.Handlers.MatchHandlers;
using AmdarisProject.Application.Services.CompetitionMatchCreatorFactoryService.MatchCreatorService;
using AmdarisProject.Application.Services.CompetitionMatchCreatorServices;
using AmdarisProject.Application.Test.ModelBuilders;
using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Presentation.Test.Tests;
using Mapster;
using MapsterMapper;
using Moq;
using Match = AmdarisProject.Domain.Models.Match;

namespace AmdarisProject.Application.Test.Tests.MatchTests
{
    public class CancelMatchHandlerTest : MockObjectUser
    {
        private readonly Mock<ICompetitionMatchCreatorFactoryService> _competitionMatchCreatorFactoryServiceMock = new();
        private readonly Mock<ICompetitionMatchCreatorService> _competitionMatchCreatorMock = new();

        [Fact]
        public async Task Test_CancelMatchHandler_Success()
        {
            MatchBuilder matchBuilder = APBuilder.CreateBasicMatch();
            Match match = matchBuilder.Get();
            _unitOfWorkMock.Setup(o => o.MatchRepository).Returns(_matchRepositoryMock.Object);
            _matchRepositoryMock.Setup(o => o.GetById(It.IsAny<Guid>())).Returns(Task.FromResult((Match?)match));
            match = matchBuilder.Clone().SetStatus(MatchStatus.CANCELED).Get();
            _matchRepositoryMock.Setup(o => o.Update(It.IsAny<Match>())).Returns(Task.FromResult(match));
            _competitionMatchCreatorFactoryServiceMock.Setup(o => o.GetCompetitionMatchCreator(It.IsAny<Type>()))
                .Returns(_competitionMatchCreatorMock.Object);
            _competitionMatchCreatorMock.Setup(o => o.CreateCompetitionMatches(It.IsAny<Guid>()))
                .Returns(Task.FromResult(It.IsAny<IEnumerable<Match>>()));
            _mapperMock.Setup(o => o.Map<MatchGetDTO>(It.IsAny<Match>())).Returns(match.Adapt<MatchGetDTO>());
            CancelMatch command = new(match.Id);
            CancelMatchHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object,
                _competitionMatchCreatorFactoryServiceMock.Object, GetLogger<CancelMatchHandler>());

            MatchGetDTO response = await handler.Handle(command, default);

            AssertResponse.MatchMatchGetDTO(match, response);
        }

        [Fact]
        public async Task Test_CancelMatchHandler_MatchNotFound_throws_APNotFoundException()
        {
            Match match = APBuilder.CreateBasicMatch().Get();
            _unitOfWorkMock.Setup(o => o.MatchRepository).Returns(_matchRepositoryMock.Object);
            _matchRepositoryMock.Setup(o => o.GetById(It.IsAny<Guid>())).Returns(Task.FromResult((Match?)null));
            CancelMatch command = new(match.Id);
            CancelMatchHandler handler = new(_unitOfWorkMock.Object, It.IsAny<IMapper>(),
                It.IsAny<ICompetitionMatchCreatorFactoryService>(), GetLogger<CancelMatchHandler>());

            await Assert.ThrowsAsync<APNotFoundException>(async () => await handler.Handle(command, default));
        }

        [Fact]
        public async Task Test_CancelMatchHandler_RollbackIsCalled_throws_Exception()
        {
            Match match = APBuilder.CreateBasicMatch().Get();
            _unitOfWorkMock.Setup(o => o.MatchRepository).Returns(_matchRepositoryMock.Object);
            _matchRepositoryMock.Setup(o => o.GetById(It.IsAny<Guid>())).Returns(Task.FromResult((Match?)match));
            _matchRepositoryMock.Setup(o => o.Update(It.IsAny<Match>())).Throws<Exception>();
            CancelMatch command = new(match.Id);
            CancelMatchHandler handler = new(_unitOfWorkMock.Object, It.IsAny<IMapper>(),
                It.IsAny<ICompetitionMatchCreatorFactoryService>(), GetLogger<CancelMatchHandler>());

            await Assert.ThrowsAsync<Exception>(async () => await handler.Handle(command, default));

            _unitOfWorkMock.Verify(x => x.RollbackTransactionAsync(), Times.Once());
        }
    }
}
