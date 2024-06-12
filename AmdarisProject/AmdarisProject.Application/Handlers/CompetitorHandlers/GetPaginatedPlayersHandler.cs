using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Common.Models;
using AmdarisProject.Application.Dtos.ResponseDTOs.DisplayDTOs;
using AmdarisProject.Domain.Models.CompetitorModels;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AmdarisProject.Application.Handlers.CompetitorHandlers
{
    public record GetPaginatedPlayers(PagedRequest PagedRequest) : IRequest<PaginatedResult<CompetitorDisplayDTO>>;
    public class GetPaginatedPlayersHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetPaginatedPlayersHandler> logger)
        : IRequestHandler<GetPaginatedPlayers, PaginatedResult<CompetitorDisplayDTO>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<GetPaginatedPlayersHandler> _logger = logger;

        public async Task<PaginatedResult<CompetitorDisplayDTO>> Handle(GetPaginatedPlayers request, CancellationToken cancellationToken)
        {
            Tuple<IEnumerable<Player>, int> players = await _unitOfWork.CompetitorRepository.GetPaginatedPlayers(request.PagedRequest);
            IEnumerable<CompetitorDisplayDTO> mapped = _mapper.Map<IEnumerable<CompetitorDisplayDTO>>(players.Item1);
            PaginatedResult<CompetitorDisplayDTO> response = new()
            {
                Items = mapped,
                PageSize = request.PagedRequest.PageSize,
                PageIndex = request.PagedRequest.PageIndex,
                Total = players.Item2
            };

            _logger.LogInformation("Got paged players (Count = {Count})!", [response.Items.Count()]);

            return response;
        }
    }
}
