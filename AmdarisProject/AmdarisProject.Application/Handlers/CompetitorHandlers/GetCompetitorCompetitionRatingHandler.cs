using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Utils;
using AmdarisProject.Domain.Models;
using MediatR;

namespace AmdarisProject.Application.Handlers.CompetitorHandlers
{
    public record GetCompetitorCompetitionRating(ulong CompetitorId, ulong CompetitionId) : IRequest<double>;
    public class GetCompetitorCompetitionRatingHandler(IUnitOfWork unitOfWork)
        : IRequestHandler<GetCompetitorCompetitionRating, double>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<double> Handle(GetCompetitorCompetitionRating request, CancellationToken cancellationToken)
        {
            IEnumerable<Match> playedMatches = await _unitOfWork.MatchRepository
                .GetAllByCompetitorAndCompetition(request.CompetitorId, request.CompetitionId);
            double rating = HandlerUtils.GetCompetitorWinRatingOfMatchesUtil(_unitOfWork, playedMatches, request.CompetitorId);
            return rating;
        }
    }
}
