using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Utils;
using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Models;
using MediatR;

namespace AmdarisProject.Application.Handlers.CompetitorHandlers
{
    public record GetCompetitorRating(Guid CompetitorId, GameType GameType) : IRequest<double>;
    public class GetCompetitiorRatingHandler(IUnitOfWork unitOfWork)
        : IRequestHandler<GetCompetitorRating, double>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<double> Handle(GetCompetitorRating request, CancellationToken cancellationToken)
        {
            IEnumerable<Match> playedMatches = await _unitOfWork.MatchRepository
                .GetAllByCompetitorAndGameType(request.CompetitorId, request.GameType);
            double rating = HandlerUtils.GetCompetitorWinRatingOfMatchesUtil(_unitOfWork, playedMatches, request.CompetitorId);
            return rating;
        }
    }
}
