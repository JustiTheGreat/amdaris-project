﻿using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Models;

namespace AmdarisProject.Application.Services
{
    public interface IEndMatchService
    {
        Task<Match> End(Guid matchId, MatchStatus status);
    }
}