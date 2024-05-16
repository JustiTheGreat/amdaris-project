using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Dtos.DisplayDTOs.CompetitorDisplayDTOs;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models.CompetitionModels;
using AmdarisProject.Domain.Models.CompetitorModels;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AmdarisProject.Application.Handlers.CompetitorHandlers
{
    public record GetTeamsThatCanBeAddedToCompetition(Guid CompetitionId) : IRequest<IEnumerable<TeamDisplayDTO>>;
    public class GetTeamsThatCanBeAddedToCompetitionHandler(IUnitOfWork unitOfWork, IMapper mapper,
        ILogger<GetTeamsThatCanBeAddedToCompetitionHandler> logger)
        : IRequestHandler<GetTeamsThatCanBeAddedToCompetition, IEnumerable<TeamDisplayDTO>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<GetTeamsThatCanBeAddedToCompetitionHandler> _logger = logger;

        public async Task<IEnumerable<TeamDisplayDTO>> Handle(GetTeamsThatCanBeAddedToCompetition request, CancellationToken cancellationToken)
        {
            Competition competition = await _unitOfWork.CompetitionRepository.GetById(request.CompetitionId)
                ?? throw new APNotFoundException(Tuple.Create(nameof(request.CompetitionId), request.CompetitionId));

            IEnumerable<Team> teams = (await _unitOfWork.CompetitorRepository
                .GetTeamsNotInCompetition(request.CompetitionId))
                .Where(team =>
                    _unitOfWork.TeamPlayerRepository.TeamHasTheRequiredNumberOfActivePlayers(team.Id, (uint)competition.GameFormat.TeamSize!).Result
                    && team.Players.All(player => competition.Competitors.All(competitor => !competitor.IsOrContainsCompetitor(player.Id))))
                .ToList();
            _logger.LogInformation("Got all teams that could be added to competition {CompetitionName} (Count = {Count})!",
                [competition.Name, teams.Count()]);
            IEnumerable<TeamDisplayDTO> response = _mapper.Map<IEnumerable<TeamDisplayDTO>>(teams);
            return response;
        }
    }
}
