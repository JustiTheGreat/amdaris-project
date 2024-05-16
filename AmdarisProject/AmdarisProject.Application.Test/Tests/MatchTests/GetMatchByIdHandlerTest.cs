using AmdarisProject.Application.Dtos.ResponseDTOs;
using AmdarisProject.Application.Handlers.MatchHandlers;
using AmdarisProject.Application.Test.ModelBuilders;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Presentation.Test.Tests;
using Moq;
using Match = AmdarisProject.Domain.Models.Match;

namespace AmdarisProject.Application.Test.Tests.MatchTests
{
    //public class GetMatchByIdHandlerTest : ApplicationTestBase
    //{
    //    [Fact]
    //    public async Task Test_GetMatchByIdHandler_Success()
    //    {
    //        Match match = APBuilder.CreateBasicMatch().Get();
    //        _unitOfWorkMock.Setup(o => o.MatchRepository).Returns(_matchRepositoryMock.Object);
    //        _matchRepositoryMock.Setup(o => o.GetById(It.IsAny<Guid>())).Returns(Task.FromResult((Match?)match));
    //        _mapperMock.Setup(o => o.Map<MatchGetDTO>(It.IsAny<Match>())).Returns(match.Adapt<MatchGetDTO>());
    //        GetMatchById command = new(match.Id);
    //        GetMatchByIdHandler handler = new(_unitOfWorkMock.Object, _mapperMock.Object, GetLogger<GetMatchByIdHandler>());

    //        MatchGetDTO response = await handler.Handle(command, default);

    //        AssertResponse.MatchMatchGetDTO(match, response);
    //    }

    //    [Fact]
    //    public async Task Test_GetMatchByIdHandler_MatchNotFound_throws_APNotFoundException()
    //    {
    //        Match match = APBuilder.CreateBasicMatch().Get();
    //        _unitOfWorkMock.Setup(o => o.MatchRepository).Returns(_matchRepositoryMock.Object);
    //        _matchRepositoryMock.Setup(o => o.GetById(It.IsAny<Guid>())).Returns(Task.FromResult((Match?)null));
    //        GetMatchById command = new(match.Id);
    //        GetMatchByIdHandler handler = new(_unitOfWorkMock.Object, It.IsAny<IMapper>(), GetLogger<GetMatchByIdHandler>());

    //        await Assert.ThrowsAsync<APNotFoundException>(async () => await handler.Handle(command, default));
    //    }
    //}
}
