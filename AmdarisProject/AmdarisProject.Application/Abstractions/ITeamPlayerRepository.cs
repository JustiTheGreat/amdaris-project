﻿using AmdarisProject.Domain.Models.CompetitorModels;

namespace AmdarisProject.Application.Abstractions
{
    public interface ITeamPlayerRepository : IGenericRepository<TeamPlayer>
    {
        Task<TeamPlayer?> GetByTeamAndPlayer(Guid teamId, Guid playerId);

        Task<bool> TeamHasTheRequiredNumberOfActivePlayers(Guid teamId, ushort requiredNumberOfActivePlayers);
    }
}