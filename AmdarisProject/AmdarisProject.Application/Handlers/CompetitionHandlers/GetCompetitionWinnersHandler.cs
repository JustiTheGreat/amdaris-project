using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Dtos.ResponseDTOs.DisplayDTOs;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models.CompetitionModels;
using AmdarisProject.Domain.Models.CompetitorModels;
using AutoMapper;
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

            IEnumerable<Competitor> winners = await _competitionRankingService.GetCompetitionWinners(competition.Id);

            _logger.LogInformation("Got competition {CompetitorName} winners (Count = {Count})!",
                [competition.Name, winners.Count()]);

            IEnumerable<CompetitorDisplayDTO> response = _mapper.Map<IEnumerable<CompetitorDisplayDTO>>(winners);
            return response;
        }
    }
}
