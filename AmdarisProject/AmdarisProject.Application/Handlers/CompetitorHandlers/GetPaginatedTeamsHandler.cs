using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Common.Models;
using AmdarisProject.Application.Dtos.ResponseDTOs.DisplayDTOs;
using AmdarisProject.Domain.Models.CompetitorModels;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AmdarisProject.Application.Handlers.CompetitorHandlers
{
    public record GetPaginatedTeams(PagedRequest PagedRequest) : IRequest<PaginatedResult<CompetitorDisplayDTO>>;
    public class GetPaginatedTeamsHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetPaginatedTeamsHandler> logger)
        : IRequestHandler<GetPaginatedTeams, PaginatedResult<CompetitorDisplayDTO>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<GetPaginatedTeamsHandler> _logger = logger;

        public async Task<PaginatedResult<CompetitorDisplayDTO>> Handle(GetPaginatedTeams request, CancellationToken cancellationToken)
        {
            Tuple<IEnumerable<Team>, int> teams = await _unitOfWork.CompetitorRepository.GetPaginatedTeams(request.PagedRequest);
            IEnumerable<CompetitorDisplayDTO> mapped = _mapper.Map<IEnumerable<CompetitorDisplayDTO>>(teams.Item1);
            PaginatedResult<CompetitorDisplayDTO> response = new()
            {
                Items = mapped,
                PageSize = request.PagedRequest.PageSize,
                PageIndex = request.PagedRequest.PageIndex,
                Total = teams.Item2
            };

            _logger.LogInformation("Got paged teams (Count = {Count})!", [response.Items.Count()]);

            return response;
        }
    }
}
