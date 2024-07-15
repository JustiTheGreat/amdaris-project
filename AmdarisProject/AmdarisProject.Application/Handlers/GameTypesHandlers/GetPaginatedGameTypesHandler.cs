using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Common.Models;
using AmdarisProject.Application.Dtos.ResponseDTOs.GetDTOs;
using AmdarisProject.Domain.Models;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AmdarisProject.Application.Handlers.GameTypesHandlers
{
    public record GetPaginatedGameTypes(PagedRequest PagedRequest)
       : IRequest<PaginatedResult<GameTypeGetDTO>>;
    public class GetPaginatedGameTypesHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetPaginatedGameTypesHandler> logger)
        : IRequestHandler<GetPaginatedGameTypes, PaginatedResult<GameTypeGetDTO>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<GetPaginatedGameTypesHandler> _logger = logger;

        public async Task<PaginatedResult<GameTypeGetDTO>> Handle(GetPaginatedGameTypes request, CancellationToken cancellationToken)
        {
            Tuple<IEnumerable<GameType>, int> gameTypes = await _unitOfWork.GameTypeRepository.GetPaginatedData(request.PagedRequest);
            IEnumerable<GameTypeGetDTO> mapped = _mapper.Map<IEnumerable<GameTypeGetDTO>>(gameTypes.Item1);
            PaginatedResult<GameTypeGetDTO> response = new()
            {
                Items = mapped,
                PageSize = request.PagedRequest.PageSize,
                PageIndex = request.PagedRequest.PageIndex,
                Total = gameTypes.Item2
            };

            _logger.LogInformation("Got paged game types (Count = {Count})!", [response.Items.Count()]);

            return response;
        }
    }
}
