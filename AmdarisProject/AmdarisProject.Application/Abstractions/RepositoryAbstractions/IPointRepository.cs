﻿using AmdarisProject.Domain.Models;

namespace AmdarisProject.Application.Abstractions.RepositoryAbstractions
{
    public interface IPointRepository : IGenericRepository<Point>
    {
        Task<Point?> GetByMatchAndPlayer(Guid playerId, Guid matchId);
    }
}
