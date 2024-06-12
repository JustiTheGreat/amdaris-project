using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Dtos.ResponseDTOs.DisplayDTOs;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models.CompetitorModels;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AmdarisProject.Application.Handlers.CompetitorHandlers
{
    public record GetPlayersNotInTeam(Guid TeamId) : IRequest<IEnumerable<CompetitorDisplayDTO>>;
    public class GetPlayersNotInTeamHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetPlayersNotInTeamHandler> logger)
        : IRequestHandler<GetPlayersNotInTeam, IEnumerable<CompetitorDisplayDTO>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<GetPlayersNotInTeamHandler> _logger = logger;

        public async Task<IEnumerable<CompetitorDisplayDTO>> Handle(GetPlayersNotInTeam request, CancellationToken cancellationToken)
        {
            Team team = await _unitOfWork.CompetitorRepository.GetTeamById(request.TeamId)
                ?? throw new APNotFoundException(Tuple.Create(nameof(request.TeamId), request.TeamId));

            IEnumerable<Player> players = await _unitOfWork.CompetitorRepository.GetPlayersNotInTeam(request.TeamId);

            _logger.LogInformation("Got all players not in team {TeamName} (Count = {Count})!",
                [team.Name, players.Count()]);

            IEnumerable<CompetitorDisplayDTO> response = _mapper.Map<IEnumerable<CompetitorDisplayDTO>>(players);
            return response;
        }
    }
}
