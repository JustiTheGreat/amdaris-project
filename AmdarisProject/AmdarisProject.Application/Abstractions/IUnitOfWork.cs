namespace AmdarisProject.Application.Abstractions
{
    public interface IUnitOfWork
    {
        ICompetitionRepository CompetitionRepository { get; }
        ICompetitorRepository CompetitorRepository { get; }
        IMatchRepository MatchRepository { get; }
        IPointRepository PointRepository { get; }

        Task SaveAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
