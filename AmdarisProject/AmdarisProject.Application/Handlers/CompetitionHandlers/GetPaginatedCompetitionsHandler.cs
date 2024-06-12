using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Common.Models;
using AmdarisProject.Application.Dtos.ResponseDTOs.DisplayDTOs;
using AmdarisProject.Domain.Models.CompetitionModels;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AmdarisProject.Application.Handlers.CompetitionHandlers
{
    public record GetPaginatedCompetitions(PagedRequest PagedRequest) : IRequest<PaginatedResult<CompetitionDisplayDTO>>;
    public class GetPaginatedCompetitionsHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetPaginatedCompetitionsHandler> logger)
        : IRequestHandler<GetPaginatedCompetitions, PaginatedResult<CompetitionDisplayDTO>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<GetPaginatedCompetitionsHandler> _logger = logger;

        public async Task<PaginatedResult<CompetitionDisplayDTO>> Handle(GetPaginatedCompetitions request, CancellationToken cancellationToken)
        {
            Tuple<IEnumerable<Competition>, int> competitions = await _unitOfWork.CompetitionRepository.GetPaginatedData(request.PagedRequest);
            IEnumerable<CompetitionDisplayDTO> mapped = _mapper.Map<IEnumerable<CompetitionDisplayDTO>>(competitions.Item1);
            PaginatedResult<CompetitionDisplayDTO> response = new()
            {
                Items = mapped,
                PageSize = request.PagedRequest.PageSize,
                PageIndex = request.PagedRequest.PageIndex,
                Total = competitions.Item2
            };

            _logger.LogInformation("Got paged competitions (Count = {Count})!", [response.Items.Count()]);

            return response;
        }
    }
}
