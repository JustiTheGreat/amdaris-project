using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Dtos.ResponseDTOs.CompetitionResponseDTOs;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models;
using AmdarisProject.Domain.Models.CompetitionModels;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AmdarisProject.handlers.competition
{
    public record StartCompetition(Guid CompetitionId) : IRequest<CompetitionGetDTO>;
    public class StartCompetitionHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<StartCompetitionHandler> logger)
        : IRequestHandler<StartCompetition, CompetitionGetDTO>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<StartCompetitionHandler> _logger = logger;

        public async Task<CompetitionGetDTO> Handle(StartCompetition request, CancellationToken cancellationToken)
        {
            Competition competition = await _unitOfWork.CompetitionRepository.GetById(request.CompetitionId)
                ?? throw new APNotFoundException(Tuple.Create(nameof(request.CompetitionId), request.CompetitionId));

            competition.Start();

            Competition updated;

            try
            {
                await _unitOfWork.BeginTransactionAsync();
                updated = await _unitOfWork.CompetitionRepository.Update(competition);
                await UpdateMatchesStartTimes(competition);
                await _unitOfWork.SaveAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }

            _logger.LogInformation("Competition {CompetitionName} has started!", [competition.Name]);

            CompetitionGetDTO response = updated is OneVSAllCompetition ? _mapper.Map<OneVSAllCompetitionGetDTO>(updated)
                : updated is TournamentCompetition ? _mapper.Map<TournamentCompetitionGetDTO>(updated)
                : throw new APException("Unexpected competition type!");
            return response;
        }

        private async Task UpdateMatchesStartTimes(Competition competition)
        {
            if (competition.ActualizedStartTime <= competition.Matches[0].ActualizedStartTime) return;

            int i = 0;

            foreach (Match match in competition.Matches)
            {
                match.ActualizedStartTime = competition.ActualizedStartTime!
                    .AddMinutes((ulong)i++ * (competition.GameFormat.DurationInMinutes! + competition.BreakInMinutes ?? 0));
                match.ActualizedEndTime = ((DateTimeOffset)match.ActualizedStartTime).AddMinutes(competition.GameFormat.DurationInMinutes ?? 0);
                await _unitOfWork.MatchRepository.Update(match);
            }

            _logger.LogInformation("Updated the starting time of the matches of competition {Competition} (Count = {Count})!",
                    [competition.Name, competition.Matches.Count()]);
        }
    }
}
