using AmdarisProject.Application.Common.Abstractions;
using AmdarisProject.Application.Dtos.DisplayDTOs;
using AmdarisProject.Domain.Models.CompetitionModels;
using MapsterMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using OnlineBookShop.Application.Common.Models;

namespace AmdarisProject.Application.Handlers.CompetitionHandlers
{
    public record GetPagedCompetitions(PagedRequest PagedRequest) : IRequest<PaginatedResult<CompetitionDisplayDTO>>;
    public class GetPagedCompetitionsHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetPagedCompetitionsHandler> logger)
        : IRequestHandler<GetPagedCompetitions, PaginatedResult<CompetitionDisplayDTO>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<GetPagedCompetitionsHandler> _logger = logger;

        public async Task<PaginatedResult<CompetitionDisplayDTO>> Handle(GetPagedCompetitions request, CancellationToken cancellationToken)
        {
            IEnumerable<Competition> competitions = await _unitOfWork.CompetitionRepository.GetPagedData(request.PagedRequest);
            IEnumerable<CompetitionDisplayDTO> mapped = _mapper.Map<IEnumerable<CompetitionDisplayDTO>>(competitions);
            PaginatedResult<CompetitionDisplayDTO> response = new()
            {
                Items = mapped,
                PageSize = request.PagedRequest.PageSize,
                PageIndex = request.PagedRequest.PageIndex,
                Total = mapped.Count()
            };

            _logger.LogInformation("Got paged competitions (Count = {Count})!", [response.Items.Count()]);

            return response;
        }
    }
}
