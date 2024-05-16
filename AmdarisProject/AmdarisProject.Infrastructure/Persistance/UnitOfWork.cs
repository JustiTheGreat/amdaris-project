using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Abstractions.RepositoryAbstractions;

namespace AmdarisProject.Infrastructure.Persistance
{
    public class UnitOfWork(AmdarisProjectDBContext dbContext, ICompetitionRepository competitionRepository,
        ICompetitorRepository competitorRepository, IGameFormatRepository gameFormatRepository,
        IMatchRepository matchRepository, IPointRepository pointRepository, ITeamPlayerRepository TeamPlayerRepository)
        : IUnitOfWork
    {
        private readonly AmdarisProjectDBContext _dbContext = dbContext;
        public ICompetitionRepository CompetitionRepository { get; private set; } = competitionRepository;
        public ICompetitorRepository CompetitorRepository { get; private set; } = competitorRepository;
        public IGameFormatRepository GameFormatRepository { get; private set; } = gameFormatRepository;
        public IMatchRepository MatchRepository { get; private set; } = matchRepository;
        public IPointRepository PointRepository { get; private set; } = pointRepository;
        public ITeamPlayerRepository TeamPlayerRepository { get; private set; } = TeamPlayerRepository;

        public async Task SaveAsync() => await _dbContext.SaveChangesAsync();

        public async Task BeginTransactionAsync() => await _dbContext.Database.BeginTransactionAsync();

        public async Task CommitTransactionAsync() => await _dbContext.Database.CommitTransactionAsync();

        public async Task RollbackTransactionAsync() => await _dbContext.Database.RollbackTransactionAsync();
    }
}
