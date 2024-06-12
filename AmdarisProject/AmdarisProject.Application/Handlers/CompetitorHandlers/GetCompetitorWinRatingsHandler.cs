using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Dtos.ResponseDTOs.GetDTOs;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models;
using AmdarisProject.Domain.Models.CompetitorModels;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AmdarisProject.Application.Handlers.CompetitorHandlers
{
    public record GetCompetitorWinRatings(Guid CompetitorId) : IRequest<Dictionary<string, double>>;
    public class GetCompetitorWinRatingsHandler(IUnitOfWork unitOfWork, IMapper mapper,
        ILogger<GetCompetitorWinRatingsHandler> logger)
        : IRequestHandler<GetCompetitorWinRatings, Dictionary<string, double>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<GetCompetitorWinRatingsHandler> _logger = logger;

        public async Task<Dictionary<string, double>> Handle(GetCompetitorWinRatings request, CancellationToken cancellationToken)
        {
            Competitor competitor = await _unitOfWork.CompetitorRepository.GetById(request.CompetitorId)
                ?? throw new APNotFoundException(Tuple.Create(nameof(request.CompetitorId), request.CompetitorId));

            IEnumerable<GameType> gameTypes = await _unitOfWork.GameTypeRepository.GetAll();

            Dictionary<string, double> winRatings = gameTypes.ToDictionary(gameType => gameType.Name,
                gameType => _unitOfWork.MatchRepository.GetCompetitorWinRatingForGameType(competitor.Id, gameType.Id).Result);

            _logger.LogInformation("Got competitor {CompetitorName} win ratings! (count={gameTypesCount})",
                [competitor.Name, winRatings.Count()]);

            return winRatings;
        }
    }
}
