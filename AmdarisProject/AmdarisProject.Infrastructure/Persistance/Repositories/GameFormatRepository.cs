﻿using AmdarisProject.Application.Abstractions.RepositoryAbstractions;
using AmdarisProject.Domain.Models;

namespace AmdarisProject.Infrastructure.Persistance.Repositories
{
    public class GameFormatRepository(AmdarisProjectDBContext dbContext)
        : GenericRepository<GameFormat>(dbContext), IGameFormatRepository
    {
    }
}
