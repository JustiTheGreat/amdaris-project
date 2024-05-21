
using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Abstractions.RepositoryAbstractions;
using AmdarisProject.Application.Common.Models;
using AmdarisProject.Domain.Enums;
using AmdarisProject.TestUtils;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;

namespace AmdarisProject.Application.Test.Tests
{
    public abstract class ApplicationTestBase
    {
        protected readonly Mock<ICompetitionRepository> _competitionRepositoryMock = new();
        protected readonly Mock<ICompetitorRepository> _competitorRepositoryMock = new();
        protected readonly Mock<IGameFormatRepository> _gameFormatRepositoryMock = new();
        protected readonly Mock<IGameTypeRepository> _gameTypeRepositoryMock = new();
        protected readonly Mock<IMatchRepository> _matchRepositoryMock = new();
        protected readonly Mock<IPointRepository> _pointRepositoryMock = new();
        protected readonly Mock<ITeamPlayerRepository> _teamPlayerRepositoryMock = new();
        protected readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        protected readonly Mock<IMapper> _mapperMock = new();
        protected readonly IMapper _mapper = AutoMapperConfiguration.GetMapper();
        protected readonly int _numberOfModelsInAList = 2;
        protected readonly PagedRequest _pagedRequest;

        protected ApplicationTestBase()
        {
            _unitOfWorkMock.Setup(o => o.CompetitionRepository).Returns(_competitionRepositoryMock.Object);
            _unitOfWorkMock.Setup(o => o.CompetitorRepository).Returns(_competitorRepositoryMock.Object);
            _unitOfWorkMock.Setup(o => o.GameFormatRepository).Returns(_gameFormatRepositoryMock.Object);
            _unitOfWorkMock.Setup(o => o.GameTypeRepository).Returns(_gameTypeRepositoryMock.Object);
            _unitOfWorkMock.Setup(o => o.MatchRepository).Returns(_matchRepositoryMock.Object);
            _unitOfWorkMock.Setup(o => o.PointRepository).Returns(_pointRepositoryMock.Object);
            _unitOfWorkMock.Setup(o => o.TeamPlayerRepository).Returns(_teamPlayerRepositoryMock.Object);

            _pagedRequest = new PagedRequest()
            {
                PageIndex = 0,
                PageSize = _numberOfModelsInAList,
                ColumnNameForSorting = string.Empty,
                SortDirection = SortDirection.ASC,
            };
        }

        protected ILogger<T> GetLogger<T>() => new Mock<ILogger<T>>().Object;
    }
}
