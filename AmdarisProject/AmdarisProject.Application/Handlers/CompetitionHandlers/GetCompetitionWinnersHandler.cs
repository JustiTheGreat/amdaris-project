using AmdarisProject.Application.Common.Abstractions;
using AmdarisProject.Application.Dtos.DisplayDTOs.CompetitorDisplayDTOs;
using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Extensions;
using AmdarisProject.Domain.Models;
using AmdarisProject.Domain.Models.CompetitionModels;
using AmdarisProject.Domain.Models.CompetitorModels;
using MapsterMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AmdarisProject.handlers.competition
{
    public record GetCompetitionWinners(Guid CompetitionId) : IRequest<IEnumerable<CompetitorDisplayDTO>>;
    public class GetCompetitionWinnersHandler(IUnitOfWork unitOfWork, IMapper mapper,
        ICompetitionRankingService competitionRankingService, ILogger<GetCompetitionWinnersHandler> logger)
        : IRequestHandler<GetCompetitionWinners, IEnumerable<CompetitorDisplayDTO>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ICompetitionRankingService _competitionRankingService = competitionRankingService;
        private readonly ILogger<GetCompetitionWinnersHandler> _logger = logger;

        public async Task<IEnumerable<CompetitorDisplayDTO>> Handle(GetCompetitionWinners request, CancellationToken cancellationToken)
        {
            Competition competition = await _unitOfWork.CompetitionRepository.GetById(request.CompetitionId)
                ?? throw new APNotFoundException(Tuple.Create(nameof(request.CompetitionId), request.CompetitionId));

            if (competition.Status is not CompetitionStatus.FINISHED)
                throw new APIllegalStatusException(competition.Status);

            if (competition.CantContinue())
                return [];

            IEnumerable<Competitor> firstPlaceCompetitors =
                await _competitionRankingService.GetCompetitionFirstPlaceCompetitors(request.CompetitionId);

            Dictionary<Competitor, int> numberOfVictoriesOverTheOthers = [];
            firstPlaceCompetitors.ToList().ForEach(competitor => numberOfVictoriesOverTheOthers.Add(competitor, 0));

            for (int i = 0; i < firstPlaceCompetitors.Count(); i++)
            {
                for (int j = i + 1; j < firstPlaceCompetitors.Count(); j++)
                {
                    //TODO call the repository or navigate on the object?
                    Match? match = await _unitOfWork.MatchRepository.GetMatchByCompetitionAndTheTwoCompetitors(competition.Id,
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

            _logger.LogInformation("Got competition {CompetitorName} winners (Count = {Count})!",
                [competition.Name, winners.Count()]);

            IEnumerable<CompetitorDisplayDTO> response =
                competition.GameFormat.CompetitorType is CompetitorType.PLAYER ? _mapper.Map<IEnumerable<PlayerDisplayDTO>>(winners)
                : competition.GameFormat.CompetitorType is CompetitorType.TEAM ? _mapper.Map<IEnumerable<TeamDisplayDTO>>(winners)
                : throw new AmdarisProjectException(nameof(winners));
            return response;
        }
    }
}
