using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Dtos.DisplayDTOs.CompetitorDisplayDTOs;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models.CompetitionModels;
using AmdarisProject.Domain.Models.CompetitorModels;
using MapsterMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AmdarisProject.Application.Handlers.CompetitorHandlers
{
    public record GetPlayersNotInCompetition(Guid CompetitionId) : IRequest<IEnumerable<PlayerDisplayDTO>>;
    public class GetPlayersNotInCompetitionHandler(IUnitOfWork unitOfWork, IMapper mapper,
        ILogger<GetPlayersNotInCompetitionHandler> logger)
        : IRequestHandler<GetPlayersNotInCompetition, IEnumerable<PlayerDisplayDTO>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<GetPlayersNotInCompetitionHandler> _logger = logger;

        public async Task<IEnumerable<PlayerDisplayDTO>> Handle(GetPlayersNotInCompetition request, CancellationToken cancellationToken)
        {
            Competition competition = await _unitOfWork.CompetitionRepository.GetById(request.CompetitionId)
                ?? throw new APNotFoundException(Tuple.Create(nameof(request.CompetitionId), request.CompetitionId));

            IEnumerable<Player> players = await _unitOfWork.CompetitorRepository.GetPlayersNotInCompetition(request.CompetitionId);

            _logger.LogInformation("Got all players not in competition {CompetitionName} (Count = {Count})!",
                [competition.Name, players.Count()]);

            IEnumerable<PlayerDisplayDTO> response = _mapper.Map<IEnumerable<PlayerDisplayDTO>>(players);
            return response;
        }
    }
}
