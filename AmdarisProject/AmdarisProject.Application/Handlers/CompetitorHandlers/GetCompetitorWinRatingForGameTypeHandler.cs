using AmdarisProject.Application.Abstractions;
using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models;
using AmdarisProject.Domain.Models.CompetitorModels;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AmdarisProject.Application.Handlers.CompetitorHandlers
{
    public record GetCompetitorWinRatingForGameType(Guid CompetitorId, Guid GameTypeId) : IRequest<double>;
    public class GetCompetitorWinRatingForGameTypeHandler(IUnitOfWork unitOfWork, ILogger<GetCompetitorWinRatingForGameTypeHandler> logger)
        : IRequestHandler<GetCompetitorWinRatingForGameType, double>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ILogger<GetCompetitorWinRatingForGameTypeHandler> _logger = logger;

        public async Task<double> Handle(GetCompetitorWinRatingForGameType request, CancellationToken cancellationToken)
        {
            Competitor competitor = await _unitOfWork.CompetitorRepository.GetById(request.CompetitorId)
                ?? throw new APNotFoundException(Tuple.Create(nameof(request.CompetitorId), request.CompetitorId));

            GameType gameType = await _unitOfWork.GameTypeRepository.GetById(request.GameTypeId)
                ?? throw new APNotFoundException(Tuple.Create(nameof(request.GameTypeId), request.GameTypeId));

            double rating = await _unitOfWork.MatchRepository.GetCompetitorWinRatingForGameType(competitor.Id, gameType.Id);

            _logger.LogInformation("Got competitor {CompetitorName} win rating for game type {GameType}!",
                [competitor.Name, gameType.Name]);

            return rating;
        }
    }
}
