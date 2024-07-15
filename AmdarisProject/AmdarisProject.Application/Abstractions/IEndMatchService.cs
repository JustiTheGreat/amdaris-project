using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Models;

namespace AmdarisProject.Application.Abstractions
{
    public interface IEndMatchService
    {
        Task<Match> End(Guid matchId, MatchStatus status);
    }
}