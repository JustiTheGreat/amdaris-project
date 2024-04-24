using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Dtos.DisplayDTOs.CompetitorDisplayDTOs;
using AmdarisProject.Application.ExtensionMethods;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models.CompetitionModels;
using AmdarisProject.Domain.Models.CompetitorModels;
using MapsterMapper;
using MediatR;

namespace AmdarisProject.Application.Handlers.CompetitorHandlers
{
    public record GetTeamsThatCanBeAddedToCompetition(Guid CompetitionId) : IRequest<IEnumerable<TeamDisplayDTO>>;
    public class GetTeamsThatCanBeAddedToCompetitionHandler(IUnitOfWork unitOfWork, IMapper mapper)
        : IRequestHandler<GetTeamsThatCanBeAddedToCompetition, IEnumerable<TeamDisplayDTO>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<IEnumerable<TeamDisplayDTO>> Handle(GetTeamsThatCanBeAddedToCompetition request, CancellationToken cancellationToken)
        {
            Competition competition = await _unitOfWork.CompetitionRepository.GetById(request.CompetitionId)
                ?? throw new APNotFoundException(Tuple.Create(nameof(request.CompetitionId), request.CompetitionId));

            IEnumerable<Team> teams = (await _unitOfWork.CompetitorRepository
                .GetFullTeamsWithTeamSizeNotInCompetition(request.CompetitionId, (ushort)competition.GameFormat.TeamSize!))
                .Where(team => team.Players.All(player =>
                    competition.Competitors.All(competitor => !competitor.IsOrContainsCompetitor(player.Id))))
                .ToList();
            IEnumerable<TeamDisplayDTO> response = _mapper.Map<List<TeamDisplayDTO>>(teams);
            return response;
        }
    }
}
