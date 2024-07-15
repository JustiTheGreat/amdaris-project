using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Dtos.ResponseDTOs.DisplayDTOs;
using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models;
using AmdarisProject.Domain.Models.CompetitionModels;
using AmdarisProject.Domain.Models.CompetitorModels;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace AmdarisProject.Application.Services
{
    public class CompetionRankingService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CompetionRankingService> logger)
        : ICompetitionRankingService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<CompetionRankingService> _logger = logger;

        public async Task<IEnumerable<RankingItemDTO>> GetCompetitionRanking(Guid competitionId)
        {
            Competition competition = await _unitOfWork.CompetitionRepository.GetById(competitionId)
                ?? throw new APNotFoundException(Tuple.Create(nameof(competitionId), competitionId));

            IEnumerable<RankingItemDTO> ranking = competition.Competitors
                .Select(competitor => new RankingItemDTO()
                {
                    Id = competitor.Id,
                    Competitor = competitor.Name,
                    Wins = competition.GetCompetitorWins(competitor.Id),
                    Points = competition.GetCompetitorPoints(competitor.Id),
                    ProfilePicture = (competitor as Player)?.ProfilePictureUri,
                })
                .OrderByDescending(entry => entry.Wins)
                .ThenByDescending(entry => entry.Points)
                .ThenBy(entry => entry.Competitor)
                .ToList();

            _logger.LogInformation("Got competition {CompetitorName} ranking (Count = {Count})!",
                [competition.Name, ranking.Count()]);

            return ranking;
        }

        public async Task<IEnumerable<Competitor>> GetCompetitionWinners(Guid competitionId)
        {
            Competition competition = await _unitOfWork.CompetitionRepository.GetById(competitionId)
                ?? throw new APNotFoundException(Tuple.Create(nameof(competitionId), competitionId));

            if (competition.Status is not CompetitionStatus.FINISHED)
                throw new APIllegalStatusException(competition.Status);

            if (competition.CantContinue())
                return [];

            IEnumerable<Competitor> firstPlaceCompetitors = await GetCompetitionFirstPlaceCompetitors(competitionId);

            Dictionary<Competitor, int> numberOfVictoriesOverTheOthers = [];
            firstPlaceCompetitors.ToList().ForEach(competitor => numberOfVictoriesOverTheOthers.Add(competitor, 0));

            for (int i = 0; i < firstPlaceCompetitors.Count(); i++)
            {
                for (int j = i + 1; j < firstPlaceCompetitors.Count(); j++)
                {
                    Match? match = competition.GetMatchByTheTwoCompetitors(
                        firstPlaceCompetitors.ElementAt(i).Id, firstPlaceCompetitors.ElementAt(j).Id);

                    if (match is null || match.Winner is null)
                        continue;

                    if (match.Winner.Id.Equals(firstPlaceCompetitors.ElementAt(i).Id))
                        numberOfVictoriesOverTheOthers[firstPlaceCompetitors.ElementAt(i)]++;
                    else
                        numberOfVictoriesOverTheOthers[firstPlaceCompetitors.ElementAt(j)]++;
                }
            }

            firstPlaceCompetitors = firstPlaceCompetitors
                .OrderByDescending(competitor => numberOfVictoriesOverTheOthers[competitor])
                .ThenBy(competitor => competitor.Name);

            IEnumerable<Competitor> winners = firstPlaceCompetitors
                .Where(competitor => numberOfVictoriesOverTheOthers[competitor]
                    == numberOfVictoriesOverTheOthers[firstPlaceCompetitors.ElementAt(0)]).ToList();
            return winners;
        }

        private async Task<IEnumerable<Competitor>> GetCompetitionFirstPlaceCompetitors(Guid competitionId)
        {
            IEnumerable<RankingItemDTO> ranking = await GetCompetitionRanking(competitionId);
            int maxWinsOnCompetition = ranking.First().Wins;
            int maxPointsOnCompetition = ranking.First().Points;
            IEnumerable<Guid> firstPlaceCompetitorIds = ranking
                .Where(rankingItem => rankingItem.Wins == maxWinsOnCompetition && rankingItem.Points == maxPointsOnCompetition)
                .Select(rankingItem => rankingItem.Id)
                .ToList();
            IEnumerable<Competitor> firstPlaceCompetitors =
                await _unitOfWork.CompetitorRepository.GetByIds(firstPlaceCompetitorIds);

            _logger.LogInformation("Got competition {CompetitorId} first place competitors (Count = {Count})!",
                [competitionId, firstPlaceCompetitors.Count()]);

            return firstPlaceCompetitors;
        }
    }
}
