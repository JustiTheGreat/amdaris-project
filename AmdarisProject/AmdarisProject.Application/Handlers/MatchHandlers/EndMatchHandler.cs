using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Dtos.ResponseDTOs;
using AmdarisProject.Application.Utils;
using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models;
using AmdarisProject.Domain.Models.CompetitorModels;
using MapsterMapper;
using MediatR;

namespace AmdarisProject.Application.Handlers.MatchHandlers
{
    public record EndMatch(Guid MatchId, MatchStatus Status) : IRequest<MatchResponseDTO>;
    public class EndMatchHandler(IUnitOfWork unitOfWork, IMapper mapper)
        : IRequestHandler<EndMatch, MatchResponseDTO>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<MatchResponseDTO> Handle(EndMatch request, CancellationToken cancellationToken)
        {
            if (request.Status is not MatchStatus.FINISHED
                || request.Status is not MatchStatus.SPECIAL_WIN_COMPETITOR_ONE
                || request.Status is not MatchStatus.SPECIAL_WIN_COMPETITOR_TWO)
                throw new ArgumentException(nameof(request.Status));

            Match match = await _unitOfWork.MatchRepository.GetById(request.MatchId)
                ?? throw new APNotFoundException(Tuple.Create(nameof(request.MatchId), request.MatchId));

            if (match.Status is not MatchStatus.STARTED)
                throw new APIllegalStatusException(match.Status);

            if (match.CompetitorOnePoints is null || match.CompetitorTwoPoints is null || match.Competition.WinAt is null)
                throw new AmdarisProjectException("Cannot end this type of match!");

            bool matchHasACompetitorWithTheWinningScore = match.CompetitorOnePoints == match.Competition.WinAt
                || match.CompetitorTwoPoints == match.Competition.WinAt;

            if (request.Status is MatchStatus.FINISHED && !matchHasACompetitorWithTheWinningScore)
                throw new AmdarisProjectException($"Match {match.Id} doesn't have a competitor with the winning number of points!");

            Match updated;

            try
            {
                await _unitOfWork.BeginTransactionAsync();
                match.Status = request.Status;
                match.EndTime = DateTime.Now;
                //match.CompetitorOnePoints = await GetCompetitorMatchPointsUtil(match, match.CompetitorOne.Id);
                //match.CompetitorTwoPoints = await GetCompetitorMatchPointsUtil(match, match.CompetitorTwo.Id);
                match.Winner = GetMatchWinner(match);
                updated = await _unitOfWork.MatchRepository.Update(match);

                await HandlerUtils.CreateCompetitionMatchesUtil(_unitOfWork, _mapper, updated.Competition.Id);

                await _unitOfWork.SaveAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }

            updated = await _unitOfWork.MatchRepository.GetById(updated.Id)
                ?? throw new APNotFoundException(Tuple.Create(nameof(updated.Id), updated.Id));

            //TODO remove
            Console.WriteLine($"Competition {match.Competition.Name}: Match between " +
                $"{match.CompetitorOne.Name} and {match.CompetitorTwo.Name} now has status {match.Status} and score " +
                $"{updated.CompetitorOnePoints}-{updated.CompetitorTwoPoints}!");
            //

            MatchResponseDTO response = _mapper.Map<MatchResponseDTO>(updated);
            return response;
        }

        private static Competitor? GetMatchWinner(Match match)
            => match.Status is MatchStatus.SPECIAL_WIN_COMPETITOR_ONE ? match.CompetitorOne
            : match.Status is MatchStatus.SPECIAL_WIN_COMPETITOR_TWO ? match.CompetitorTwo
            : match.Status is MatchStatus.FINISHED ?
                (match.CompetitorOnePoints > match.CompetitorTwoPoints ? match.CompetitorOne
                : match.CompetitorOnePoints < match.CompetitorTwoPoints ? match.CompetitorTwo
                : null)
            : throw new APIllegalStatusException(match.Status);

        //private async Task<uint> GetCompetitorMatchPointsUtil(Match match, Guid competitorId)
        //{
        //    if (match.Status is not MatchStatus.STARTED
        //        || match.Status is not MatchStatus.FINISHED
        //        || match.Status is not MatchStatus.SPECIAL_WIN_COMPETITOR_ONE
        //        || match.Status is not MatchStatus.SPECIAL_WIN_COMPETITOR_TWO)
        //        throw new APIllegalStatusException(match.Status);

        //    return match.CompetitorOne.Id.Equals(competitorId) ? (uint)match.CompetitorOnePoints!
        //        : match.CompetitorTwo.Id.Equals(competitorId) ? (uint)match.CompetitorTwoPoints!
        //        : match.CompetitorOne is Team teamOne ?
        //            (await _unitOfWork.PointRepository.GetByPlayersAndMatch(teamOne.Players.GetIds(), match.Id))
        //            .Select(point => point.Value)
        //            .Aggregate((point1, point2) => point1 + point2)
        //        : match.CompetitorTwo is Team teamTwo ?
        //            (await _unitOfWork.PointRepository.GetByPlayersAndMatch(teamTwo.Players.GetIds(), match.Id))
        //            .Select(point => point.Value)
        //            .Aggregate((point1, point2) => point1 + point2)
        //        : throw new AmdarisProjectException("Trying to get points for a competitor not in match!");
        //}
    }
}
