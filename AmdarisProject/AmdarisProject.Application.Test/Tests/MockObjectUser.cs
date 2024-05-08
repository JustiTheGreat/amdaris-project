using AmdarisProject.Application.Abstractions;
using AmdarisProject.Presentation;
using MapsterMapper;
using Microsoft.Extensions.Logging;
using Moq;

namespace AmdarisProject.Application.Test.Tests
{
    public abstract class MockObjectUser
    {
        protected readonly Mock<ICompetitionRepository> _competitionRepositoryMock = new();
        protected readonly Mock<ICompetitorRepository> _competitorRepositoryMock = new();
        protected readonly Mock<IGameFormatRepository> _gameFormatRepositoryMock = new();
        protected readonly Mock<IMatchRepository> _matchRepositoryMock = new();
        protected readonly Mock<IPointRepository> _pointRepositoryMock = new();
        protected readonly Mock<ITeamPlayerRepository> _teamPlayerRepositoryMock = new();
        protected readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        protected readonly Mock<IMapper> _mapperMock = new();
        protected readonly int _numberOfModelsInAList = 2;
        private static bool _mapsterWasConfigured = false;
        protected MockObjectUser()
        {
            if (!_mapsterWasConfigured)
            {
                _mapsterWasConfigured = true; ;
                MapsterConfiguration.ConfigureMapster();
            }
        }

        protected ILogger<T> GetLogger<T>() => new Mock<ILogger<T>>().Object;
    }
}
