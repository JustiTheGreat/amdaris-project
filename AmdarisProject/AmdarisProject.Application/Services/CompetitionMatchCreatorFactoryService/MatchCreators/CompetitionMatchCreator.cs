using AmdarisProject.Application.Abstractions;
using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Extensions;
using AmdarisProject.Domain.Models;
using AmdarisProject.Domain.Models.CompetitionModels;
using AmdarisProject.Domain.Models.CompetitorModels;

namespace AmdarisProject.Application.Services.CompetitionMatchCreatorFactoryService.MatchCreators
{
    public abstract class CompetitionMatchCreator<T>(IUnitOfWork unitOfWork)
        : ICompetitionMatchCreator where T : Competition
    {
        protected readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<IEnumerable<Match>> CreateCompetitionMatches(Guid competitionId)
        {
            Competition competition = await _unitOfWork.CompetitionRepository.GetById(competitionId)
                ?? throw new APNotFoundException(Tuple.Create(nameof(competitionId), competitionId));

            if (!ShouldCreateMatches((T)competition))
                return [];

            IEnumerable<Match> createdMatches = await CreateMatches((T)competition);
            return createdMatches;
        }

        protected abstract bool ShouldCreateMatches(T competition);

        protected abstract Task<IEnumerable<Match>> CreateMatches(T competiton);

        protected async Task<Match> CreateMatch(string location, Guid competitorOneId,
            Guid competitorTwoId, Guid competitionId, ushort? stageLevel, ushort? stageIndex)
        {
            Competitor competitorOne = await _unitOfWork.CompetitorRepository.GetById(competitorOneId)
                ?? throw new APNotFoundException(Tuple.Create(nameof(competitorOneId), competitorOneId));

            Competitor competitorTwo = await _unitOfWork.CompetitorRepository.GetById(competitorTwoId)
                ?? throw new APNotFoundException(Tuple.Create(nameof(competitorTwoId), competitorTwoId));

            Competition competition = await _unitOfWork.CompetitionRepository.GetById(competitionId)
                ?? throw new APNotFoundException(Tuple.Create(nameof(competitionId), competitionId));

            if (competition.GameFormat.CompetitorType is CompetitorType.PLAYER
                    && (competitorOne is not Player || competitorTwo is not Player)
                || competition.GameFormat.CompetitorType is CompetitorType.TEAM
                    && (competitorOne is not Team || competitorTwo is not Team))
                throw new AmdarisProjectException("Competiors not matching the competition type!");

            if (competitorOne.Id.Equals(competitorTwo.Id))
                throw new AmdarisProjectException($"Trying to create a match with the same competitor on both sides!");

            if (!competition.ContainsCompetitor(competitorOne.Id))
                throw new AmdarisProjectException($"Competitor {competitorOne.Id} not registered to competition {competition.Id}!");

            if (!competition.ContainsCompetitor(competitorTwo.Id))
                throw new AmdarisProjectException($"Competitor {competitorTwo.Id} not registered to competition {competition.Id}!");

            DateTime? matchStartTime = null;

            if (competition.GameFormat.DurationInSeconds is not null)
            {
                if (competition.Matches.Count == 0)
                {
                    matchStartTime = competition.StartTime;
                }
                else
                {
                    DateTime lastStartTime = competition.Matches.Max(match => match.StartTime)
                        ?? throw new AmdarisProjectException("Null start time for a timed match!");

                    matchStartTime = lastStartTime.AddSeconds(
                        competition.GameFormat.DurationInSeconds! + competition.BreakInSeconds ?? 0);
                }
            }

            Match match = new()
            {
                Location = location,
                StartTime = matchStartTime,
                EndTime = null,
                Status = MatchStatus.NOT_STARTED,
                CompetitorOne = competitorOne,
                CompetitorTwo = competitorTwo,
                Competition = competition,
                CompetitorOnePoints = null,
                CompetitorTwoPoints = null,
                Winner = null,
                StageLevel = stageLevel,
                StageIndex = stageIndex,
                Points = [],
            };

            Match created = await _unitOfWork.MatchRepository.Create(match);
            return created;
        }
    }
}
