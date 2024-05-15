using AmdarisProject.Application.Common.Abstractions;
using AmdarisProject.Application.Dtos.DisplayDTOs.CompetitorDisplayDTOs;
using AmdarisProject.Domain.Models.CompetitorModels;
using MapsterMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using OnlineBookShop.Application.Common.Models;

namespace AmdarisProject.Application.Handlers.CompetitorHandlers
{
    public record GetPagedPlayers(PagedRequest PagedRequest) : IRequest<PaginatedResult<PlayerDisplayDTO>>;
    public class GetPagedPlayersHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetPagedPlayersHandler> logger)
        : IRequestHandler<GetPagedPlayers, PaginatedResult<PlayerDisplayDTO>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<GetPagedPlayersHandler> _logger = logger;

        public async Task<PaginatedResult<PlayerDisplayDTO>> Handle(GetPagedPlayers request, CancellationToken cancellationToken)
        {
            IEnumerable<Player> players = await _unitOfWork.CompetitorRepository.GetPagedPlayers(request.PagedRequest);
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
