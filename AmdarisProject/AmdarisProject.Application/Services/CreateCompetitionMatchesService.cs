using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Services.CompetitionMatchCreatorServices;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models;
using AmdarisProject.Domain.Models.CompetitionModels;
using AmdarisProject.Domain.Models.CompetitorModels;

namespace AmdarisProject.Application.Services
{
    public class CreateCompetitionMatchesService(IUnitOfWork unitOfWork, ICompetitionRankingService competionRankingService,
        ICompetitionMatchCreatorFactoryService competitionMatchCreatorFactoryService)
        : ICreateCompetitionMatchesService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ICompetitionRankingService _competitionRankingService = competionRankingService;
        private readonly ICompetitionMatchCreatorFactoryService _competitionMatchCreatorFactoryService = competitionMatchCreatorFactoryService;

        public async Task<IEnumerable<Match>> CreateCompetitionMatches(Guid competitionId)
        {
            IEnumerable<Match> createdMatches = [];

            Competition competition = await _unitOfWork.CompetitionRepository.GetById(competitionId)
                ?? throw new APNotFoundException(Tuple.Create(nameof(competitionId), competitionId));

            bool allMatchesWerePlayed = !(await _unitOfWork.MatchRepository.GetUnfinishedByCompetition(competition.Id)).Any();

            if (!allMatchesWerePlayed)
                return createdMatches;

            IEnumerable<Competitor> firstPlaceCompetitors = 
                await _competitionRankingService.GetCompetitionFirstPlaceCompetitors(competition.Id);

            if (firstPlaceCompetitors.Count() <= 1)
                return createdMatches;

            CompetitionMatchCreator competitionMatchCreatorService =
                _competitionMatchCreatorFactoryService.GetCompetitionMatchCreatorService(competition.GetType());

            //TODO it's not right to use first place competitors for tournament competition
            createdMatches = await competitionMatchCreatorService.CreateMatches(competition, firstPlaceCompetitors);

            return createdMatches;
        }
    }
}
