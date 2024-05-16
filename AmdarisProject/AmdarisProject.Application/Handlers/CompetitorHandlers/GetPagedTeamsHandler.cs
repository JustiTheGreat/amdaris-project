using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Common.Models;
using AmdarisProject.Application.Dtos.DisplayDTOs.CompetitorDisplayDTOs;
using AmdarisProject.Domain.Models.CompetitorModels;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AmdarisProject.Application.Handlers.CompetitorHandlers
{
    public record GetPaginatedTeams(PagedRequest PagedRequest) : IRequest<PaginatedResult<TeamDisplayDTO>>;
    public class GetPagedTeamsHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetPagedTeamsHandler> logger)
        : IRequestHandler<GetPaginatedTeams, PaginatedResult<TeamDisplayDTO>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<GetPagedTeamsHandler> _logger = logger;

        public async Task<PaginatedResult<TeamDisplayDTO>> Handle(GetPaginatedTeams request, CancellationToken cancellationToken)
        {
            IEnumerable<Team> teams = await _unitOfWork.CompetitorRepository.GetPagedTeams(request.PagedRequest);
            IEnumerable<TeamDisplayDTO> mapped = _mapper.Map<IEnumerable<TeamDisplayDTO>>(teams);
            PaginatedResult<TeamDisplayDTO> response = new()
            {
                Items = mapped,
                PageSize = request.PagedRequest.PageSize,
                PageIndex = request.PagedRequest.PageIndex,
                Total = mapped.Count()
            };

            _logger.LogInformation("Got paged teams (Count = {Count})!", [response.Items.Count()]);

            return response;
        }
    }
}
