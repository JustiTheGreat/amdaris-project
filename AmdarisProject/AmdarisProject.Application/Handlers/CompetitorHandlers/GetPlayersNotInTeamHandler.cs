using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Common.Models;
using AmdarisProject.Application.Dtos.ResponseDTOs.DisplayDTOs;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models;
using AmdarisProject.Domain.Models.CompetitorModels;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AmdarisProject.Application.Handlers.CompetitorHandlers
{
    public record GetPlayersNotInTeam(Guid TeamId, PagedRequest PagedRequest) : IRequest<PaginatedResult<CompetitorDisplayDTO>>;
    public class GetPlayersNotInTeamHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetPlayersNotInTeamHandler> logger)
        : IRequestHandler<GetPlayersNotInTeam, PaginatedResult<CompetitorDisplayDTO>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<GetPlayersNotInTeamHandler> _logger = logger;

        public async Task<PaginatedResult<CompetitorDisplayDTO>> Handle(GetPlayersNotInTeam request, CancellationToken cancellationToken)
        {
            Team team = await _unitOfWork.CompetitorRepository.GetTeamById(request.TeamId)
                ?? throw new APNotFoundException(Tuple.Create(nameof(request.TeamId), request.TeamId));

            Tuple<IEnumerable<Player>, int> players =
                await _unitOfWork.CompetitorRepository.GetPlayersNotInTeam(request.TeamId, request.PagedRequest);
            IEnumerable<CompetitorDisplayDTO> mapped = _mapper.Map<IEnumerable<CompetitorDisplayDTO>>(players.Item1);
            PaginatedResult<CompetitorDisplayDTO> response = new()
            {
                Items = mapped,
                PageSize = request.PagedRequest.PageSize,
                PageIndex = request.PagedRequest.PageIndex,
                Total = players.Item2
            };

            _logger.LogInformation("Got players not in team {TeamName} (Count = {Count})!",
                [team.Name, response.Items.Count()]);

            return response;
        }
    }
}
