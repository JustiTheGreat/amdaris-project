using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Dtos.ResponseDTOs.CompetitorResponseDTOs;
using AmdarisProject.Application.ExtensionMethods;
using AmdarisProject.Application.Services;
using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models;
using AmdarisProject.Domain.Models.CompetitionModels;
using AmdarisProject.Domain.Models.CompetitorModels;
using MapsterMapper;
using MediatR;

namespace AmdarisProject.handlers.competition
{
    public record GetCompetitionWinner(Guid CompetitionId) : IRequest<CompetitorResponseDTO>;
    public class GetCompetitionWinnerHandler(IUnitOfWork unitOfWork, IMapper mapper,
        ICompetitionRankingService competitionRankingService)
        : IRequestHandler<GetCompetitionWinner, CompetitorResponseDTO>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ICompetitionRankingService _competitionRankingService = competitionRankingService;

        public async Task<CompetitorResponseDTO> Handle(GetCompetitionWinner request, CancellationToken cancellationToken)
        {
            Competition competition = await _unitOfWork.CompetitionRepository.GetById(request.CompetitionId)
                ?? throw new APNotFoundException(Tuple.Create(nameof(request.CompetitionId), request.CompetitionId));

            if (competition.Status is not CompetitionStatus.FINISHED)
                throw new APIllegalStatusException(competition.Status);

            IEnumerable<Competitor> firstPlaceCompetitors =
                await _competitionRankingService.GetCompetitionFirstPlaceCompetitors(request.CompetitionId);

            Dictionary<Competitor, int> numberOfVictoriesOverTheOthers = [];

            firstPlaceCompetitors.ToList().ForEach(competitor => numberOfVictoriesOverTheOthers.Add(competitor, 0));

            for (int i = 0; i < firstPlaceCompetitors.Count(); i++)
            {
                for (int j = i + 1; j < firstPlaceCompetitors.Count(); j++)
                {
                    Match match = await _unitOfWork.MatchRepository.GetMatchByCompetitionAndTheTwoCompetitors(competition.Id,
                        firstPlaceCompetitors.ElementAt(i).Id, firstPlaceCompetitors.ElementAt(j).Id)
                        ?? throw new APNotFoundException([Tuple.Create(competition.Name, competition.Id),
                            Tuple.Create(firstPlaceCompetitors.ElementAt(i).Name, firstPlaceCompetitors.ElementAt(i).Id),
                            Tuple.Create(firstPlaceCompetitors.ElementAt(j).Name, firstPlaceCompetitors.ElementAt(j).Id)]);

                    if (match.Winner is null) throw new AmdarisProjectException("Match without winner!");

                    if (match.Winner.IsOrContainsCompetitor(firstPlaceCompetitors.ElementAt(i).Id))
                        numberOfVictoriesOverTheOthers[firstPlaceCompetitors.ElementAt(i)]++;
                    else
                        numberOfVictoriesOverTheOthers[firstPlaceCompetitors.ElementAt(j)]++;
                }
            }

            Competitor winner = firstPlaceCompetitors
                .OrderByDescending(competitor => numberOfVictoriesOverTheOthers[competitor])
                .ThenBy(competitor => competitor.Name).First();

            CompetitorResponseDTO response = winner is Player ? _mapper.Map<PlayerResponseDTO>(winner)
                : winner is Team ? _mapper.Map<TeamResponseDTO>(winner)
                : throw new AmdarisProjectException(nameof(winner));
            return response;
        }
    }
}
