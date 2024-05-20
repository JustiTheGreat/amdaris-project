using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Dtos.DisplayDTOs.CompetitorDisplayDTOs;
using AmdarisProject.Application.Dtos.ResponseDTOs;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models.CompetitionModels;
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
                    Competitor = _mapper.Map<CompetitorDisplayDTO>(competitor),
                    Wins = competition.GetCompetitorWins(competitor.Id),
                    Points = competition.GetCompetitorPoints(competitor.Id)
                })
                .OrderByDescending(entry => entry.Wins)
                .ThenByDescending(entry => entry.Points)
                .ThenBy(entry => entry.Competitor.Name)
                .ToList();

            _logger.LogInformation("Got competition {CompetitorName} ranking (Count = {Count})!",
                [competition.Name, ranking.Count()]);

            return ranking;
        }
    }
}
