using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Dtos.DisplayDTOs.CompetitorDisplayDTOs;
using AmdarisProject.Domain.Models.CompetitorModels;
using MapsterMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AmdarisProject.Application.Handlers.CompetitorHandlers
{
    public record GetAllTeams() : IRequest<IEnumerable<TeamDisplayDTO>>;
    public class GetAllTeamsHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetAllTeamsHandler> logger)
        : IRequestHandler<GetAllTeams, IEnumerable<TeamDisplayDTO>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<GetAllTeamsHandler> _logger = logger;

        public async Task<IEnumerable<TeamDisplayDTO>> Handle(GetAllTeams request, CancellationToken cancellationToken)
        {
            IEnumerable<Team> teams = await _unitOfWork.CompetitorRepository.GetAllTeams();
            _logger.LogInformation("Got all teams (Count = {Count})!", [teams.Count()]);
            IEnumerable<TeamDisplayDTO> response = _mapper.Map<IEnumerable<TeamDisplayDTO>>(teams);
            return response;
        }
    }
}
