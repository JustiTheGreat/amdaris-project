using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Common.Models;
using AmdarisProject.Application.Dtos.DisplayDTOs.CompetitorDisplayDTOs;
using AmdarisProject.Domain.Models.CompetitorModels;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AmdarisProject.Application.Handlers.CompetitorHandlers
{
    public record GetPaginatedPlayers(PagedRequest PagedRequest) : IRequest<PaginatedResult<PlayerDisplayDTO>>;
    public class GetPaginatedPlayersHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetPaginatedPlayersHandler> logger)
        : IRequestHandler<GetPaginatedPlayers, PaginatedResult<PlayerDisplayDTO>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<GetPaginatedPlayersHandler> _logger = logger;

        public async Task<PaginatedResult<PlayerDisplayDTO>> Handle(GetPaginatedPlayers request, CancellationToken cancellationToken)
        {
            IEnumerable<Player> players = await _unitOfWork.CompetitorRepository.GetPaginatedPlayers(request.PagedRequest);
            IEnumerable<PlayerDisplayDTO> mapped = _mapper.Map<IEnumerable<PlayerDisplayDTO>>(players);
            PaginatedResult<PlayerDisplayDTO> response = new()
            {
                Items = mapped,
                PageSize = request.PagedRequest.PageSize,
                PageIndex = request.PagedRequest.PageIndex,
                Total = mapped.Count()
            };

            _logger.LogInformation("Got paged players (Count = {Count})!", [response.Items.Count()]);

            return response;
        }
    }
}
