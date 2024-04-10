using AmdarisProject.Application.Abstractions;

namespace AmdarisProject.Infrastructure
{
    public class UnitOfWork(AmdarisProjectDBContext dbContext, ICompetitionRepository competitionRepository,
        ICompetitorRepository competitorRepository, IMatchRepository matchRepository, IPointRepository pointRepository,
        IStageRepository stageRepository) : IUnitOfWork
    {
        private readonly AmdarisProjectDBContext _dbContext = dbContext;
        public ICompetitionRepository CompetitionRepository { get; private set; } = competitionRepository;
        public ICompetitorRepository CompetitorRepository { get; private set; } = competitorRepository;
        public IMatchRepository MatchRepository { get; private set; } = matchRepository;
        public IPointRepository PointRepository { get; private set; } = pointRepository;
        public IStageRepository StageRepository { get; private set; } = stageRepository;

        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync()
        {
            await _dbContext.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            await _dbContext.Database.CommitTransactionAsync();
        }

        public async Task RollbackTransactionAsync()
        {
            await _dbContext.Database.RollbackTransactionAsync();
        }
    }
}
