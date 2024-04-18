using AmdarisProject.Application.Abstractions;
using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models.CompetitorModels;
using MediatR;

namespace AmdarisProject.Application.Handlers.CompetitorHandlers
{
    public record GetCompetitorWinRatingForGameType(Guid CompetitorId, GameType GameType) : IRequest<double>;
    public class GetCompetitorWinRatingForGameTypeHandler(IUnitOfWork unitOfWork)
        : IRequestHandler<GetCompetitorWinRatingForGameType, double>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<double> Handle(GetCompetitorWinRatingForGameType request, CancellationToken cancellationToken)
        {
            Competitor competitor = await _unitOfWork.CompetitorRepository.GetById(request.CompetitorId)
                ?? throw new APNotFoundException(Tuple.Create(nameof(request.CompetitorId), request.CompetitorId));

            double rating = await _unitOfWork.MatchRepository.GetCompetitorWinRatingForGameType(competitor.Id, request.GameType);
            return rating;
        }
    }
}
