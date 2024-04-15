using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Dtos.ResponseDTOs;
using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models;
using AmdarisProject.Domain.Models.CompetitorModels;
using MapsterMapper;
using MediatR;

namespace AmdarisProject.Application.Handlers.MatchHandlers
{
    public record StartMatch(ulong MatchId) : IRequest<MatchResponseDTO>;
    public class StartMatchHandler(IUnitOfWork unitOfWork, IMapper mapper)
        : IRequestHandler<StartMatch, MatchResponseDTO>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<MatchResponseDTO> Handle(StartMatch request, CancellationToken cancellationToken)
        {
            Match match = await _unitOfWork.MatchRepository.GetById(request.MatchId)
                ?? throw new APNotFoundException(Tuple.Create(nameof(request.MatchId), request.MatchId));

            if (match.Competition.Status is not CompetitionStatus.STARTED)
                throw new APIllegalStatusException(match.Competition.Status);

            bool anotherMatchIsBeingPlayed = match.Competition.Matches.Any(match => match.Status is MatchStatus.STARTED);

            if (anotherMatchIsBeingPlayed)
                throw new AmdarisProjectException("Cannot start a match while another one is being played!");

            if (match.Status is not MatchStatus.NOT_STARTED)
                throw new APIllegalStatusException(match.Status);

            Match updated;

            try
            {
                await _unitOfWork.BeginTransactionAsync();

                DateTime now = DateTime.Now;
                match.Status = MatchStatus.STARTED;
                match.StartTime = now;
                updated = await _unitOfWork.MatchRepository.Update(match);

                await InitializePointsForMatchPlayers(match);

                bool lateStart = match.StartTime is not null && now > match.StartTime;

                if (lateStart)
                    await UpdateUnstartedMatchesStartTimes(match);

                await _unitOfWork.SaveAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }

            updated = await _unitOfWork.MatchRepository.GetById(match.Id)
                ?? throw new APNotFoundException(Tuple.Create(nameof(request.MatchId), match.Id));

            //TODO remove
            Console.WriteLine($"Competition {match.Competition.Name}: " +
                $"Match between {match.CompetitorOne.Name} and {match.CompetitorTwo.Name} has started!");
            //

            MatchResponseDTO response = _mapper.Map<MatchResponseDTO>(updated);
            return response;
        }

        private async Task InitializePointsForMatchPlayers(Match match)
        {
            async Task CreatePoint(Player player)
            {
                Point point = new()
                {
                    Value = 0,
                    Player = player,
                    Match = match,
                };

                Point created = await _unitOfWork.PointRepository.Create(point);
            }

            async Task CreatePointsForTeamPlayers(Team team)
            {
                IEnumerable<Player> players = await _unitOfWork.CompetitorRepository.GetTeamPlayers(team.Id);
                foreach (Player player in players)
                    await CreatePoint(player);
            }

            if (match.Competition.CompetitorType is CompetitorType.PLAYER)
            {
                await CreatePoint((Player)match.CompetitorOne);
                await CreatePoint((Player)match.CompetitorTwo);
            }
            else
            {
                await CreatePointsForTeamPlayers((Team)match.CompetitorOne);
                await CreatePointsForTeamPlayers((Team)match.CompetitorTwo);
            }
        }

        private async Task UpdateUnstartedMatchesStartTimes(Match match)
        {
            IEnumerable<Match> matches = await _unitOfWork.MatchRepository
                .GetNotStartedByCompetitionOrderedByStartTime(match.Competition.Id);
            int i = 0;

            foreach (Match m in matches)
            {
                m.StartTime = ((DateTime)match.StartTime!).AddSeconds(
                        (double)((ulong)++i * (match.Competition.DurationInSeconds! + match.Competition.BreakInSeconds!)));
                await _unitOfWork.MatchRepository.Update(m);
            }
        }
    }
}
