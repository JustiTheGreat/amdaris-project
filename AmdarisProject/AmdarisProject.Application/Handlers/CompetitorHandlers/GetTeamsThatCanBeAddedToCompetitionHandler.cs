using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Dtos.ResponseDTOs.DisplayDTOs;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models.CompetitionModels;
using AmdarisProject.Domain.Models.CompetitorModels;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AmdarisProject.Application.Handlers.CompetitorHandlers
{
    public record GetTeamsThatCanBeAddedToCompetition(Guid CompetitionId) : IRequest<IEnumerable<CompetitorDisplayDTO>>;
    public class GetTeamsThatCanBeAddedToCompetitionHandler(IUnitOfWork unitOfWork, IMapper mapper,
        ILogger<GetTeamsThatCanBeAddedToCompetitionHandler> logger)
        : IRequestHandler<GetTeamsThatCanBeAddedToCompetition, IEnumerable<CompetitorDisplayDTO>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<GetTeamsThatCanBeAddedToCompetitionHandler> _logger = logger;

        public async Task<IEnumerable<CompetitorDisplayDTO>> Handle(GetTeamsThatCanBeAddedToCompetition request, CancellationToken cancellationToken)
        {
            Competition competition = await _unitOfWork.CompetitionRepository.GetById(request.CompetitionId)
                ?? throw new APNotFoundException(Tuple.Create(nameof(request.CompetitionId), request.CompetitionId));

            IEnumerable<Team> teams = await _unitOfWork.CompetitorRepository
                .GetTeamsThatCanBeAddedToCompetition(request.CompetitionId, (uint)competition.GameFormat.TeamSize!);

            _logger.LogInformation("Got all teams that could be added to competition {CompetitionName} (Count = {Count})!",
                [competition.Name, teams.Count()]);

            IEnumerable<CompetitorDisplayDTO> response = _mapper.Map<IEnumerable<CompetitorDisplayDTO>>(teams);
            return response;
        }
    }
}
