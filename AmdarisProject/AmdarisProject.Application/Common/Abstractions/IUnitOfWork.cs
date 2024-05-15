using AmdarisProject.Application.Common.Abstractions.RepositoryAbstractions;

namespace AmdarisProject.Application.Common.Abstractions
{
    public interface IUnitOfWork
    {
        ICompetitionRepository CompetitionRepository { get; }
        ICompetitorRepository CompetitorRepository { get; }
        IGameFormatRepository GameFormatRepository { get; }
        IMatchRepository MatchRepository { get; }
        IPointRepository PointRepository { get; }
        ITeamPlayerRepository TeamPlayerRepository { get; }

        Task SaveAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
