using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Dtos;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models.CompetitionModels;
using AmdarisProject.Domain.Models.CompetitorModels;

namespace AmdarisProject.Application.Services
{
    public class CompetionRankingService(IUnitOfWork unitOfWork) : ICompetitionRankingService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<IEnumerable<RankingItemDTO>> GetCompetitionRanking(Guid competitionId)
        {
            Competition competition = await _unitOfWork.CompetitionRepository.GetById(competitionId)
                ?? throw new APNotFoundException(Tuple.Create(nameof(competitionId), competitionId));

            List<RankingItemDTO> ranking = competition.Competitors
                .Select(competitor => new RankingItemDTO(
                    competitor.Id,
                    competitor.Name,
                    (uint)GetCompetitorCompetitionWins(competition, competitor.Id),
                    (uint)GetCompetitorCompetitionPoints(competition, competitor.Id)
                ))
                .OrderByDescending(entry => entry.Wins)
                .ThenByDescending(entry => entry.Points)
                .ThenBy(entry => entry.CompetitorName)
                .ToList();

            List<RankingItemDTO> competitorsWithTheSamePositionInRanking = [];

            return ranking;
        }

        public async Task<IEnumerable<Competitor>> GetCompetitionFirstPlaceCompetitors(Guid competitionId)
        {
            IEnumerable<RankingItemDTO> ranking = await GetCompetitionRanking(competitionId);
            uint maxWinsOnCompetition = ranking.First().Wins;
            uint maxPointsOnCompetition = ranking.First().Points;
            IEnumerable<Guid> firstPlaceCompetitorIds = ranking
                .Where(rankingItem => rankingItem.Wins == maxWinsOnCompetition && rankingItem.Points == maxPointsOnCompetition)
                .Select(rankingItem => rankingItem.CompetitorId)
                .ToList();
            IEnumerable<Competitor> firstPlaceCompetitors = await _unitOfWork.CompetitorRepository.GetByIds(firstPlaceCompetitorIds);
            return firstPlaceCompetitors;
        }

        private int GetCompetitorCompetitionWins(Competition competition, Guid competitorId)
            => competition.Matches.Where(match => match.Winner != null && match.Winner.Id.Equals(competitorId)).Count();

        private int GetCompetitorCompetitionPoints(Competition competition, Guid competitorId)
            => competition.Matches.Where(match => match.CompetitorOnePoints is not null && match.CompetitorTwoPoints is not null
                        && (match.CompetitorOne.Id.Equals(competitorId) || match.CompetitorTwo.Id.Equals(competitorId)))
                    .Select(match => match.CompetitorOne.Id.Equals(competitorId)
                        ? (int)match.CompetitorOnePoints! : (int)match.CompetitorTwoPoints!)
                    .Aggregate(0, (result, point) => result + point);
    }
}
