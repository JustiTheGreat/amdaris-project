using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Dtos.DisplayDTOs.CompetitorDisplayDTOs;
using AmdarisProject.Domain.Models.CompetitorModels;
using MapsterMapper;
using MediatR;

namespace AmdarisProject.Application.Handlers.CompetitorHandlers
{
    public record GetPlayersNotInCompetition(Guid CompetitionId) : IRequest<IEnumerable<PlayerDisplayDTO>>;
    public class GetPlayersNotInCompetitionHandler(IUnitOfWork unitOfWork, IMapper mapper)
        : IRequestHandler<GetPlayersNotInCompetition, IEnumerable<PlayerDisplayDTO>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<IEnumerable<PlayerDisplayDTO>> Handle(GetPlayersNotInCompetition request, CancellationToken cancellationToken)
        {
            IEnumerable<Player> players = await _unitOfWork.CompetitorRepository.GetPlayersNotInCompetition(request.CompetitionId);
            IEnumerable<PlayerDisplayDTO> response = _mapper.Map<List<PlayerDisplayDTO>>(players);
            return response;
        }
    }
}
